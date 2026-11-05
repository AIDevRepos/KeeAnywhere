# KeeAnywhere Lite Full — KeePass 2.62

**Full multi-provider compatibility branch**, independently maintained from upstream tag `v2.1.0`. The label **Lite** distinguishes this older, smaller full-featured baseline from the newer, larger upstream `master`. It does *not* mean Google Drive-only.

## Branch roles

- **Lite / Full (~6.52 MiB originally):** `lite-full-2.1.0-keepass-2.62`, all upstream v2.1.0 providers retained.
- **Google Drive Only (~1.27 MiB):** `google-drive-only-2.1.0-keepass-2.62`, only Google Drive standard and restricted providers registered.
- **Upstream tracking:** `master`, never merge into the small branches indiscriminately.

## Patch and scope

The original 2.62 compatibility change removes the AWS SDK S3 code analyzer entry from `KeeAnywhere/KeeAnywhere.csproj`. KeePass 2.62 refuses the `..\\packages\\AWSSDK.S3.3.5.8\\analyzers\\dotnet\\cs\\AWSSDK.S3.CodeAnalysis.dll` path in a PLGX package. Subsequent selected upstream fixes are limited to the OAuth loopback callback fallback and the Dropbox empty-root-path handling; DLL references and the v2.1.0 dependency baseline remain unchanged.

## Original user-tested binary

- Filename: `KeeAnywhere-2.1.0-KeePass-2.62-FIX.plgx`
- File size: **6,838,779 bytes** (approximately 6.52 MiB)
- SHA-256: `38868c1392c105dc74269f14fbfc8e149573b7b7fb89c18b77d5cbc7f69f876b`
- User reported it **loads and works normally** in KeePass 2.62.
- This branch preserves the corresponding **upstream 2.1.0 source plus compatible project fix**. It does **not** claim to contain the byte-identical original tested PLGX.

## Build

GitHub Actions `.github/workflows/lite-full-plgx.yml` builds the PLGX on Windows with NuGet, MSBuild and legacy PlgxTool. Build validation and checksums are generated in the workflow run.

First independent source-branch build (**PASS**, 2026-11-05):
- Run: https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42020235861
- PLGX output size: **6,838,306 bytes** (about 6.52 MiB)
- SHA-256: `E9CAAF665BAE57E961E2BC83FF21D1B4D0785F76C92EEAFEDC7C1B648EBC4EDE`
- GitHub Actions artifact: `KeeAnywhere-Lite-Full-2.1.0-KeePass262`
- This output is **not byte-identical** to the earlier working 6,838,779-byte PLGX and has public-source placeholder Google OAuth settings. No new KeePass/Google Drive runtime claim is made.


**Google OAuth reproducibility limitation:** The public upstream source includes placeholder OAuth client configuration. The original user-tested PLGX was patched from the official distributed binary, which contained functional application configuration. The GitHub Actions artifact generated from this branch may therefore compile and load but **cannot be assumed to authenticate with Google Drive** until appropriate OAuth configuration is provided securely. Never commit application secrets to this public repository.

## Upstream updates

Review upstream changes individually. Cherry-pick or port only relevant fixes to each maintenance branch; preserve the v2.1.0 dependency baseline unless an update is necessary. Rebuild and test independently. Do not merge the modern `master` wholesale into either small branch.

## Verification status

Original patched binary: user runtime load reported **PASS**. GitHub branch build and the resulting artifact's OAuth/runtime functionality must be validated separately.

## Selective upstream ports (2026-11-05)

- OAuth callback loopback: try `127.0.0.1` first, and `localhost` only when binding fails. Kept the v2.1.0 IdentityModel options and form-post handling instead of importing incompatible v2.2.0 OAuth API changes.
- Dropbox: guard empty/null root paths before indexing the first character (upstream `master` fix).
- Not ported: OneDrive / Microsoft.Graph 5, Box 5, AWS SDK and cryptography/dependency upgrades, or other API-rewrite changes that require incompatible dependencies. Google Drive provider code was unchanged upstream.
- Baseline: original working 6,838,779-byte PLGX remains a distinct, unmodified test binary. Fresh source-branch build and KeePass runtime verification needed after these ports; the public CI build still uses placeholder OAuth values.
