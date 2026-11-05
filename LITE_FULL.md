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

### Selective upstream port build (2026-11-05)

- Windows CI run: https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42022891756 — **SUCCESS**.
- PLGX size: **6,838,428 bytes** (about 6.52 MiB).
- PLGX SHA-256: `A08135114FEDC3B4CDFE2990375516F2F850FF20052D5157A94E3E8E1E585596`.
- Artifact name: `KeeAnywhere-Lite-Full-2.1.0-KeePass262`.
- The previous tested 6,838,779-byte full PLGX remains the user-confirmed baseline. GitHub CI output uses placeholder Google OAuth settings and has not been user runtime-tested after these ports.

## Variant identification and menu fix (2026-11-05)

- Internal assembly and KeePass entry class: `KeeAnywhereLE` / `KeeAnywhereLEExt` (distinct from the upstream `KeeAnywhere` and the other LE variant).
- Plugin title: `KeeAnywhere-LE`; assembly/file version: `2.1.0.1`; informational version: `2.1.0-LE.1`.
- Tools menu, cloud file actions, and settings dialog visibly identify `KeeAnywhere-LE`.
- Each plugin instance contributes one Settings item. Three identical Settings items indicate multiple copies loaded. On installation, move **all previous KeeAnywhere PLGX/DLL versions** out of the KeePass `Plugins` folder, including nested folders; install exactly one desired variant, clear Plugin Cache if stale, then restart KeePass. Do not attempt to auto-disable other plugins.
- New CI artifact retains publicly available placeholder OAuth settings, so it must be prepared with the working application's OAuth configuration before users can test Google Drive authentication. Do not commit OAuth secrets to the public GitHub repository.
- New named variants require new KeePass 2.62 runtime testing; earlier tested binaries remain independent known-good baselines.

## v2.1.0.1 named-variant build evidence (2026-11-05)

- Unique KeePass plugin identity: `KeeAnywhere-LE`, distinct internal assembly and plugin class.
- GitHub Windows Release CI: **SUCCESS**, run https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42029908970.
- Delivered private OAuth-configured test PLGX: `KeeAnywhere-LE_2.1.0.1_KeePass-2.62.plgx`.
- PLGX size: **6,838,941 bytes**; bundled files: **196**.
- SHA-256: `19547B10236C4E9D6098770D0E4CA44BAE925E100A6B341DE140A8436CABF99F`.
- Both PLGX source packaging and per-file GZip integrity checked; source preserved from the compiled branch except the original provider OAuth application configuration transferred from the previous working private package. No OAuth credentials committed.
- **Runtime status:** not yet tested in KeePass 2.62 after this renamed build. Earlier working packages remain the last user-confirmed baselines.
- Duplicate menu remediation: uninstall/move older PLGX or DLL copies outside `Plugins` (including nested folders), use only one variant at a time; clear plugin cache and restart if necessary.

## v2.1.0.2 embedded resources and plugin entry repair (2026-11-05)

- User reported both `2.1.0.1` variants fail KeePass 2.62 initialization with `MissingManifestResourceException` for `KeeAnywhere.PluginResources.resources`. These faulty binaries are withdrawn from use.
- Cause: renamed DLL assembly to `KeeAnywhereLE` while strongly typed resource classes and WinForms resources retain the original `KeeAnywhere` manifest prefix.
- Restored the original known-good identity triplet: `KeeAnywhere.dll`, `namespace KeeAnywhere`, `KeeAnywhereExt`, allowing the original manifest resource name to resolve. No provider or OAuth behavior was changed.
- Kept distinguishable user-facing identity: `KeeAnywhere-LE` in assembly title, menus and settings; version `2.1.0.2` (informational `2.1.0-LE.2`). Both variants are alternatives and must not be loaded simultaneously.
- CI now validates presence and basic access of `KeeAnywhere.PluginResources.resources` before upload. Runtime tests on KeePass 2.62 remain required for this build.

## v2.1.0.2 validated resource hotfix build (2026-11-05)

- CI build: **PASS**; GitHub Actions run https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42033103822.
- The CI verifies `KeeAnywhere.PluginResources.resources` exists in `KeeAnywhere.dll` and can be opened as an embedded resource stream.
- Privately configured test package: `KeeAnywhere-LE_2.1.0.2_KeePass-2.62.plgx`, **6,838,555 bytes**, SHA-256 `201FB1254834A780B699BA81B590FD9EFC1722529EE62EF8190485D02070DFA7`.
- The source package differs from the public CI package only in the Google Drive OAuth application settings (copied unchanged from the original user-tested official PLGX), plus a regenerated PLGX UUID to bypass old compiled cache. Public source contains no private configuration.
- Restored original `KeeAnywhere` internal assembly, namespace, and entry type; retained user-visible edition title and version `2.1.0.2`.
- The 2.1.0.1 renamed-assembly builds failed in the user's KeePass 2.62 and **must not be used**.
- Validation scope: Windows C# build + PLGX creation passed, resource manifest check passed, all packaged GZip entries and project metadata parsed successfully. **KeePass 2.62 runtime test of v2.1.0.2 remains pending user confirmation.**
