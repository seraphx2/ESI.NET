# SpecCheck

Compares the live ESI OpenAPI document against what `ESI.NET/Logic/*.cs` actually
implements, and exits non-zero on drift. Run on a schedule by
`.github/workflows/spec-check.yml`; a failed run emails the maintainer.

```sh
dotnet run --project tools/SpecCheck            # live spec, default source
dotnet run --project tools/SpecCheck -- --strict
dotnet run --project tools/SpecCheck -- --spec ./openapi.json --source ./ESI.NET/Logic
```

| Flag | Default | |
| --- | --- | --- |
| `--spec <url\|path>` | `https://esi.evetech.net/meta/openapi.json` | OpenAPI 3.1 document |
| `--source <dir>` | auto-detected (`ESI.NET/Logic` under the repo root) | Logic sources to scan |
| `--strict` | off | warnings fail the build too |

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
- **Missing** — in the spec but not implemented → **warning** (error under
  `--strict`).
- **Parameter-name drift** — same route shape, different `{param}` name →
  **warning**.

## Tier 2 — schema

_(next commit)_ For every covered endpoint, walk the `EsiResponse<T>` model with
reflection and compare it to the resolved 200 schema: properties present in the
spec but not the model, properties in the model the spec no longer has, and type
mismatches (`int` vs `int64`, scalar vs array, enum value drift).

## Files

| | |
| --- | --- |
| `Spec.cs` | loads the OpenAPI document, enumerates operations, resolves `$ref` |
| `Wrapper.cs` | Roslyn scan of `Logic/*.cs` + reflection join → implemented endpoints |
| `CoverageCheck.cs` | Tier 1 diff |
| `SchemaCheck.cs` | Tier 2 diff _(next commit)_ |
| `Finding.cs` | severity + message |
| `Report.cs` | stdout + `$GITHUB_STEP_SUMMARY`, exit code |
| `Program.cs` | argument parsing, orchestration |
