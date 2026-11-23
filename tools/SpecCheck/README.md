# SpecCheck

Compares the live ESI OpenAPI document against what `ESI.NET/Logic/*.cs` actually
implements, and exits non-zero on drift. Run daily by
`.github/workflows/spec-check.yml`, which tracks drift in a single `spec-drift`
issue (see [In CI](#in-ci)).

**Any warning or error fails the run.** There is no strict/lenient split — the
check only reports things worth acting on: an endpoint appeared or disappeared,
a response's field set / shape / enum changed, or a field's type changed in a way
that would make deserialization throw (a wider integer, a date that became a
plain string, an enum that became free text). Info notes never fail a run. A
model that is *looser* than the spec (a `string` for a date or an enum, `long`
for `int32`) is not flagged: it cannot throw.

**Target: the latest published ESI compatibility date.** SpecCheck reads
`/meta/compatibility-dates`, takes the newest, and checks the wrapper against that
snapshot — not against the date the wrapper currently pins
(`ESI.NET.EsiVersion.CompatibilityDate`). So when CCP publishes a new date the run
goes red with the diff (new endpoints, changed shapes), which is the signal to do
a catch-up release: wrap/fix the changes, bump `EsiVersion.CompatibilityDate` to
match, and the run goes green again. When the two dates are equal the wrapper is
current.

Deliberate exceptions live in [`allowlist.txt`](allowlist.txt): spec endpoints
the wrapper intentionally does not implement. Shared model fields are **not**
listed there — SchemaCheck works those out itself (below).

```sh
dotnet run --project tools/SpecCheck            # live spec, default source
dotnet run --project tools/SpecCheck -- --spec ./openapi.json --source ./ESI.NET/Logic
dotnet run --project tools/SpecCheck -- --no-schema   # Tier 1 only
dotnet run --project tools/SpecCheck -- --probe       # + live-deserialize every public GET
dotnet run --project tools/SpecCheck -- --write-docs  # regenerate the XML docs + scope table
dotnet run --project tools/SpecCheck -- --check-docs  # fail if they are out of date
```

| Flag | Default | |
| --- | --- | --- |
| `--spec <url\|path>` | `https://esi.evetech.net/meta/openapi.json` | OpenAPI 3.1 document |
| `--source <dir>` | auto-detected (`ESI.NET/Logic` under the repo root) | Logic sources to scan |
| `--probe` | off | after the checks, fetch every public GET (path params from a fixture set) live and deserialize the real body into the wrapper's model — catches nullable fields the spec claims are non-null |
| `--no-schema` | off | Tier 1 (coverage) only |
| `--issue-body <path>` | off | on drift, write a GitHub issue body (the markdown report plus a drift fingerprint) to `<path>` |
| `--write-docs` | off | instead of the checks, write the XML docs and `EsiEndpointRequirements.g.cs` from the spec (see [Docs](#docs)) |
| `--check-docs` | off | instead of the checks, exit 1 if `--write-docs` would change any file |

## In CI

`spec-check.yml` passes `--issue-body` and keeps **one** open issue labelled
`spec-drift`. The fingerprint is a hash of the failing findings (order-independent),
stored in the issue body as `<!-- spec-drift-fingerprint: … -->`, so the same drift
is never reported twice:

| Run result | Issue | Run |
| --- | --- | --- |
| new drift | opened | fails |
| drift changed | body rewritten + one comment | fails |
| drift unchanged | untouched | passes (notice) |
| drift matches an issue closed as *not planned* | untouched — muted | passes (notice) |
| clean | open issue closed | passes |
| build failure / spec unreachable | untouched | fails |

Muting by closing as *not planned* covers that exact drift only. For a permanent
exception, add the endpoint to `allowlist.txt`.

The same workflow's `keepalive` job re-enables `spec-check.yml` and
`integration.yml` daily via the API. GitHub disables scheduled workflows after 60
days without repository activity, and a disabled workflow reports nothing.

## How it reads the wrapper

The HTTP method, route and security live *inside* each Logic method body, so they
come from a Roslyn syntax walk of every `Execute<T>(_client, _config,
RequestSecurity.x, HttpMethod.y, "/route/", …)` call. The response model `T`
comes from reflecting the built `ESI.NET` assembly (`Task<EsiResponse<T>>`) and is
joined back on `(class, method)`. One method (`SearchLogic.Query`) picks its route
from a local at runtime; its routes are read heuristically from the method body
and a scanner warning is emitted.

## Tier 1 — coverage

Set-difference on `(METHOD, path)` after normalising trailing slashes.

- **Orphaned** — implemented but absent from the spec → **error**. A latent 404;
  the endpoint was renamed or removed upstream.
- **Missing** — in the spec but not implemented → **warning**, unless the key is
  in `allowlist.txt`.
- **Parameter-name drift** — same route shape, different `{param}` name →
  **warning**.
- **Scanner note** — how SpecCheck read a route (e.g. `SearchLogic.Query`) →
  **info**, never fails.

## Tier 2 — schema

For every covered endpoint, `SchemaCheck` flattens the `EsiResponse<T>` model
(reflection) and the resolved 200 schema into a common `Node` tree and walks them
together.

| Finding | Severity | |
| --- | --- | --- |
| `schema-shape` | error | model is an object / array / dictionary where the spec is something else |
| `schema-type` | error | `string`↔`number`, `integer` where the spec is `number`, a `DateTime` where the spec is a plain string, … |
| `schema-int-width` | error | model is 32-bit where the spec is `int64` |
| `schema-enum-open` | error | model is a C# enum where the spec allows any string (`StringEnumConverter` throws on an undeclared value) |
| `schema-nullable` | error | spec allows `null`; the model is a non-nullable value type |
| `schema-missing-property` | warning | spec has a property the model does not bind |
| `schema-extra-property` | warning | model has a property that is in **no** schema for **any** endpoint using that model class |
| `schema-enum-drift` | warning | modelled enum values differ from the spec's |
| `schema-number-widening` | info | model is floating-point where the spec is `integer` |
| `schema-unverified` | info | the spec allows any value, a union has a non-object variant, or a `$ref` recurses before reaching an object |

The walk goes through the structures ESI uses for variable data:

- **Dictionaries.** A `Dictionary<string, T>` model is compared against the
  spec's `additionalProperties` schema, or, where the spec has fixed properties,
  each property against `T`. An `object`-typed value binds anything.
- **Tagged unions.** ESI writes these as a `oneOf` of single-key objects. The
  variants are merged into one object whose members are all optional, which is
  what a typed union class (`SovereigntyClaim`) binds, and what a dictionary
  model is checked against key by key.
- **Recursive types.** A type that contains itself (killmail items) becomes a
  cyclic graph rather than being cut off. The walk compares each (model, spec)
  pair once.

### Shared models

Many model classes back several endpoints, each returning a subset of the fields
(`Order` on public market orders has no `wallet_division`; character orders do).
Before the walk, SchemaCheck makes one pass over every endpoint's `(model, schema)`
tree pair and records, per CLR type, every schema property name that ever lines up
with it. A field then only counts as `schema-extra-property` if it appears in
**none** of that type's schemas — i.e. ESI dropped it everywhere. This is keyed by
CLR type, so a nested shared type (`ResolvedInfo` inside both `IDLookup` and the
`/universe/names` list) is resolved correctly. No allowlist entry needed.

## Docs

`--write-docs` writes ESI.NET's XML doc comments from the spec, so IntelliSense says
what ESI says - and keeps saying it as the spec moves. It uses the spec at the
**pinned** compatibility date (`EsiVersion.CompatibilityDate`), since the docs
describe the API the wrapper actually calls. Only `///` comments are touched; each
edit replaces one member's comment block and nothing else.

What it writes:

- **Every Logic method** that calls an endpoint: the spec's summary and description,
  then remarks for the route, the **required scope**, the **required corporation
  roles** (`x-required-roles`), pagination, and ESI's cache time
  (`x-client-cache-ttl`); a `<param>` per parameter (spec parameter or request-body
  field, matched by name) and `<returns>`.
- **Every model type and property** the endpoints return: the property's spec
  description (with the allowed values, for a string with an `enum`), and for each
  type which methods return it ("Returned by ...") or which model holds it ("Part of
  ...").
- **Each Logic class, its constructor, and its `EsiClient` / `IEsiClient` property**,
  named after the endpoints' spec tag.
- **`ESI.NET/EsiEndpointRequirements.g.cs`**: every authenticated route's scope and
  roles. The runtime reads it to refuse (with a `403`, unsent) a call the
  token cannot make, and to name the needed roles on a `403`.

Hand-written text lives in [`docs.json`](docs.json), never in the generated
comments (the next run would overwrite them). It is consulted where the spec has no
description or one that only restates the name ("Alliance's name"):

| Section | Key | Wins over |
| --- | --- | --- |
| `properties` | `Namespace.Type.Property` | everything |
| `fields` | JSON field name, for every model | a weak or missing spec description |
| `operations` | `"METHOD /path"`: `summary`, `remarks`, `returns`, `params` | the spec, for that operation |
| `parameters` | C# parameter name, for every method | a weak or missing spec description |

Each run writes `docs-report.txt` (ignored by git): members still undocumented, the
ones documented only by a weak spec description, and hand-written comments it
replaced. Anything outside the spec (`EsiClient`, `EsiConfig`, the handlers, SSO,
the enums) is documented by hand in the source. ESI.NET builds with
`GenerateDocumentationFile` and CS1591 on, so an undocumented public member is a
build warning.

The daily workflow runs `--check-docs`; it fails when a generated comment was edited
by hand or the spec's text changed. Fix it by running `--write-docs` and committing.

## Files

| | |
| --- | --- |
| `Spec.cs` | loads the OpenAPI document, enumerates operations, resolves `$ref` |
| `Wrapper.cs` | Roslyn scan of `Logic/*.cs` + reflection join → implemented endpoints |
| `CoverageCheck.cs` | Tier 1 diff |
| `Node.cs` | shared structural view of a type (object / array / scalar / unknown) |
| `SchemaCheck.cs` | Tier 2 — schema→`Node`, CLR→`Node`, and the walk that compares them |
| `Finding.cs` | severity + message |
| `Report.cs` | stdout + `$GITHUB_STEP_SUMMARY`, issue body + drift fingerprint, exit code |
| `Program.cs` | argument parsing, allowlist load, orchestration |
| `Docs/DocPlan.cs` | joins spec operations and schemas to Logic methods and model properties |
| `Docs/DocWriter.cs` | renders and splices the `///` comments; renders `EsiEndpointRequirements.g.cs` |
| `Docs/DocText.cs` | weak-description test, XML escaping, line wrapping |
| `Docs/DocOverrides.cs` | loads `docs.json` |
| `docs.json` | hand-written doc text (see [Docs](#docs)) |
| `allowlist.txt` | spec endpoints the wrapper intentionally does not implement |
