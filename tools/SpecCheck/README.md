# SpecCheck

Compares the live ESI OpenAPI document against what `ESI.NET/Logic/*.cs` actually
implements, and exits non-zero on drift. Run on a schedule by
`.github/workflows/spec-check.yml`; a failed run emails the maintainer.

**Any finding fails the run.** There is no strict/lenient split — the check only
reports things worth acting on: an endpoint appeared or disappeared, or a
response's field set / shape / enum changed. Type width, `string`-vs-typed-enum,
and date-as-string are not checked; a model authored from the spec gets those
right, and they are not what "the API changed" looks like.

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
```

| Flag | Default | |
| --- | --- | --- |
| `--spec <url\|path>` | `https://esi.evetech.net/meta/openapi.json` | OpenAPI 3.1 document |
| `--source <dir>` | auto-detected (`ESI.NET/Logic` under the repo root) | Logic sources to scan |
| `--no-schema` | off | Tier 1 (coverage) only |

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
| `schema-shape` | error | model is an object where the spec is an array (or vice versa) |
| `schema-type` | error | `string`↔`number`, `integer` where the spec is `number`, … |
| `schema-missing-property` | warning | spec has a property the model does not bind |
| `schema-extra-property` | warning | model has a property that is in **no** schema for **any** endpoint using that model class |
| `schema-enum-drift` | warning | modelled enum values differ from the spec's |
| `schema-number-widening` | info | model is floating-point where the spec is `integer` |
| `schema-unverified` | info | `oneOf`/`anyOf`, dictionary, or a recursive `$ref` |

### Shared models

Many model classes back several endpoints, each returning a subset of the fields
(`Order` on public market orders has no `wallet_division`; character orders do).
Before the walk, SchemaCheck makes one pass over every endpoint's `(model, schema)`
tree pair and records, per CLR type, every schema property name that ever lines up
with it. A field then only counts as `schema-extra-property` if it appears in
**none** of that type's schemas — i.e. ESI dropped it everywhere. This is keyed by
CLR type, so a nested shared type (`ResolvedInfo` inside both `IDLookup` and the
`/universe/names` list) is resolved correctly. No allowlist entry needed.

## Files

| | |
| --- | --- |
| `Spec.cs` | loads the OpenAPI document, enumerates operations, resolves `$ref` |
| `Wrapper.cs` | Roslyn scan of `Logic/*.cs` + reflection join → implemented endpoints |
| `CoverageCheck.cs` | Tier 1 diff |
| `Node.cs` | shared structural view of a type (object / array / scalar / unknown) |
| `SchemaCheck.cs` | Tier 2 — schema→`Node`, CLR→`Node`, and the walk that compares them |
| `Finding.cs` | severity + message |
| `Report.cs` | stdout + `$GITHUB_STEP_SUMMARY`, exit code |
| `Program.cs` | argument parsing, allowlist load, orchestration |
| `allowlist.txt` | spec endpoints the wrapper intentionally does not implement |
