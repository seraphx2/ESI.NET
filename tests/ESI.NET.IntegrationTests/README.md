# ESI.NET.IntegrationTests

Live tests against the real ESI API. **Not part of `ESI.NET.sln`** — so
`dotnet test ESI.NET.sln` (the normal CI path) never runs them. They run only
from `.github/workflows/integration.yml` (weekly + `workflow_dispatch`), and
`workflow_dispatch` is the manual pre-release check: dispatch, confirm green,
then merge `dev → master`.

## Two groups

| Collection | Needs | Covers |
| --- | --- | --- |
| `live` (`PublicSmokeTests`) | nothing | ~25 unauthenticated GETs across ~12 tags — the handler pipeline moves a request and a real payload deserialises into `EsiResponse<T>` |
| `live-auth` (`AuthProbeTests`) | `ESI_CLIENT_ID` + `ESI_SECRET_KEY` + `ESI_REFRESH_TOKEN` | token exchange → `Verify` (JWKS validation) → a bearer call → transparent refresh firing against live SSO. `[SkippableFact]` — skips when the three are unset. |

`LiveFixture` resolves a few entity ids *by name* via `/universe/ids` at start-up,
so nothing hard-codes an id that could drift. `LiveFixture.Call` retries a call
up to 3× on a transport error or 5xx.

## Run locally

```sh
# public smoke only
dotnet test tests/ESI.NET.IntegrationTests

# + auth probe
ESI_CLIENT_ID=… ESI_SECRET_KEY=… ESI_REFRESH_TOKEN=… dotnet test tests/ESI.NET.IntegrationTests
```

Mint the refresh token with `tools/MintToken`; the probe calls
`Wallet.CharacterWallet`, so the token needs `esi-wallet.read_character_wallet.v1`.

## Environment

| Variable | Default | |
| --- | --- | --- |
| `ESI_USER_AGENT` | a repo-identifying string | sent as `X-User-Agent` |
| `ESI_DATASOURCE` | `Tranquility` | |
| `ESI_CLIENT_ID` / `ESI_SECRET_KEY` / `ESI_REFRESH_TOKEN` | — | auth probe; all three or none |

## Caveat

A real refresh in CI can make EVE rotate the refresh token. If the probe starts
failing with `invalid_grant`, re-mint and update the `ESI_REFRESH_TOKEN` secret.
The probe prints a `::warning::` when it sees a rotation.
