# MintToken

A local, run-once utility that walks the EVE SSO **authorization-code** flow in your
browser and prints a long-lived **refresh token**. That token is what the live auth
probe (and any manual pre-release check) uses to prove that token exchange, JWKS
validation, the bearer pipeline, and transparent refresh all still work against the
real SSO servers.

It is not packed and not shipped. It exists so re-minting later is `dotnet run`.

## One-time setup

1. Create (or reuse) an application at <https://developers.eveonline.com/>.
   - **Confidential client** — you need a **Client ID** *and* a **Secret Key**.
   - **Callback URL**: `http://localhost:8080/callback` (character for character; change
     the port with `ESI_CALLBACK_PORT` and register that instead).
   - **Scopes**: add whatever the probe will call. The default is just
     `esi-wallet.read_character_wallet.v1` — the token can do nothing else.
2. Use a throwaway / alt character if you don't want your main's wallet readable. A
   fresh free account works; the wallet endpoint returns `0.0` and still `200`s.

## Run

```sh
# from the repo root
ESI_CLIENT_ID=xxxx ESI_SECRET_KEY=yyyy dotnet run --project tools/MintToken
```

Anything omitted from the environment is prompted for (the secret key is masked).
The browser opens, you log in and authorize, the tool catches the redirect on
`localhost`, exchanges the code, calls `Verify()` to confirm the character, and
prints the refresh token.

It then offers to store the token as a GitHub Actions secret via `gh`
(`gh secret set ESI_REFRESH_TOKEN`, value piped over stdin so it never lands in
argv or shell history). Answer `y`, or skip it and copy the printed command.

## Environment variables

| Variable | Default | Meaning |
| --- | --- | --- |
| `ESI_CLIENT_ID` | *(prompt)* | Application Client ID |
| `ESI_SECRET_KEY` | *(masked prompt)* | Application Secret Key |
| `ESI_SCOPES` | `esi-wallet.read_character_wallet.v1` | space- or comma-separated scope list, or `all` to request every scope in the ESI spec (used by `probe.yml`) |
| `ESI_CALLBACK_PORT` | `8080` | loopback port; the callback becomes `http://localhost:<port>/callback` |
| `ESI_DATASOURCE` | `Tranquility` | `Tranquility` or `Serenity` |
| `ESI_SET_SECRET` | *(unset)* | `1` / `true` / `yes` → set the secret with `gh` without prompting |
| `ESI_SECRET_NAME` | `ESI_REFRESH_TOKEN` | secret name to write |
| `ESI_SECRET_REPO` | the `origin` remote | `owner/name` for `gh secret set --repo`; auto-resolved from `git remote get-url origin` (this repo has several GitHub remotes, so `gh` can't guess) |

## Notes

- The refresh token has no timer. It dies only if you revoke it, change the app's
  scopes, rotate the app secret, or CCP rotates it on use. Re-run this tool if the
  probe starts failing with `invalid_grant`.
- Fork pull requests never see repo secrets, so the probe must skip when
  `ESI_REFRESH_TOKEN` is absent.
