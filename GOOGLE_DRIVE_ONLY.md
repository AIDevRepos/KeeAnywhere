# KeeAnywhere 2.1.0 — Google Drive only for KeePass 2.62

This is a reduced, independently maintained branch based on upstream tag `v2.1.0`.

## Changes

- Removed the AWS SDK Analyzer PLGX project entry rejected by KeePass 2.62's path validation.
- Registered only Google Drive (`gdrive`) and Google Drive restricted (`gdrive-r`).
- Excluded other storage provider source files and unused DLL references from the PLGX project.
- Removed the unconditional Microsoft.Graph.Core assembly preload.
- Kept existing storage type enum values to avoid altering saved account identifiers.

## Build evidence (2026-11-05)

- GitHub Actions Windows 2022 build: **SUCCESS**.
- Run: https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42016928529
- GitHub Actions PLGX size: 1,333,362 bytes.
- GitHub Actions PLGX SHA-256: `F5C845A1E84597DE361400554966A10A3563252514C891D566742BF37CE0E96C`.
- Local Google Drive test PLGX size: 1,333,464 bytes.
- Local test PLGX SHA-256: `ABA27314B0C38255277531F5FF49FD4CD320826E4448DBDAAE93A17E9838EA0C`.
- Static inspection: PLGX record parsing, all per-file GZip streams, embedded csproj XML, and rejection of traversal paths passed.
- The local test PLGX reuses the Google OAuth application settings embedded in the user's provided official v2.1.0 PLGX. Credentials were **not** published in this repository.

## Important reproduction note

Upstream public `v2.1.0` source deliberately contains **dummy Google OAuth client values**. Consequently, a PLGX built straight from the public branch by GitHub Actions is **not configured for Google Drive OAuth sign-in**, despite successful compilation.

To produce an authenticated package, supply appropriate OAuth application credentials securely at build time (e.g. GitHub Actions Secrets); do not commit credentials to GitHub. The local test binary was prepared separately using settings from the user's previously provided original release file.

## User test confirmation (2026-11-05)

The user reported that the previously delivered **Google Drive-only 1.27 MiB local test PLGX** works normally with KeePass 2.62. This is user-reported runtime confirmation, not an independently executed test; separate detailed results for sign-in, open, and save were not supplied. The confirmed file is the locally configured test PLGX with SHA-256 `ABA27314B0C38255277531F5FF49FD4CD320826E4448DBDAAE93A17E9838EA0C`.

## Selective upstream fix (pending fresh user runtime test)

Applied the upstream local OAuth callback listener fallback: keep `127.0.0.1` as the first choice and try `localhost` only if binding fails. The older IdentityModel client and its form-post response handling are intentionally unchanged to preserve compatibility.

Upstream v2.2.0 changed the OAuth library and other providers' SDKs; **none of those bulk dependency upgrades** were merged. The four Google Drive provider source files do not differ between upstream `v2.1.0` and `master`.

The new branch-produced PLGX still needs a KeePass 2.62 runtime test after this fix. The GitHub build uses placeholder Google OAuth app values and cannot replace the previously working locally configured package without secure OAuth configuration.

### Selective upstream port build (2026-11-05)

- Windows CI run: https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42022778142 — **SUCCESS**.
- PLGX size: **1,333,474 bytes** (about 1.27 MiB).
- PLGX SHA-256: `68A0C0832A8A26F552AAA7C5B6B2654FA64BCC1483C47C3BD095FBD09F89733A`.
- Artifact name: `KeeAnywhere-2.1.0-GoogleDriveOnly-KeePass262`.
- This CI output still has public-source dummy Google OAuth values; it is **not the same binary** the user confirmed working. Runtime verification of this newly ported build remains pending.

## Variant identification and menu fix (2026-11-05)

- Internal assembly and KeePass entry class: `KeeAnywhereGDrive` / `KeeAnywhereGDriveExt` (distinct from upstream `KeeAnywhere` and the LE full variant).
- Plugin title `KeeAnywhere-GDrive`, assembly/file version `2.1.0.1`, informational version `2.1.0-GDrive.1`.
- Tools menu, cloud file actions, and settings dialog visibly identify `KeeAnywhere-GDrive`.
- Each loaded plugin contributes one settings menu entry; when upgrading remove all earlier KeeAnywhere PLGX/DLL copies from the entire KeePass installation directory, install exactly one version, clear cache only if stale, and restart. No automatic exclusion of other installed plugins.
- GitHub public-source build retains placeholder Google OAuth settings. New packages must receive working OAuth configuration privately before testing Google Drive authentication. Never commit OAuth credentials.
- Previous known-good packages are unchanged; this distinct-identity build requires new KeePass runtime verification.

## v2.1.0.1 named-variant build evidence (2026-11-05)

- Unique KeePass plugin identity: `KeeAnywhere-GDrive`, distinct internal assembly and plugin class.
- GitHub Windows Release CI: **SUCCESS**, run https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42029927725.
- Delivered private OAuth-configured test PLGX: `KeeAnywhere-GDrive_2.1.0.1_KeePass-2.62.plgx`.
- PLGX size: **1,333,628 bytes**; bundled files: **133**.
- SHA-256: `61C857A034D6AC3EC124E118D4F3179F09CD8C02404E02F9727620E2065A1B21`.
- Both PLGX source packaging and per-file GZip integrity checked; source preserved from the compiled branch except the original provider OAuth application configuration transferred from the previous working private package. No OAuth credentials committed.
- **Runtime status:** not yet tested in KeePass 2.62 after this renamed build. Earlier working packages remain the last user-confirmed baselines.
- Duplicate menu remediation: move older PLGX or DLL copies outside the entire KeePass installation directory, use only one variant at a time; clear plugin cache and restart if necessary.

## v2.1.0.2 embedded resources and plugin entry repair (2026-11-05)

- User reported both `2.1.0.1` variants fail KeePass 2.62 initialization with `MissingManifestResourceException` for `KeeAnywhere.PluginResources.resources`. These faulty binaries are withdrawn from use.
- Cause: renamed DLL assembly to `KeeAnywhereGDrive` while strongly typed resource classes and WinForms resources retain the original `KeeAnywhere` manifest prefix.
- Restored the original known-good identity triplet: `KeeAnywhere.dll`, `namespace KeeAnywhere`, `KeeAnywhereExt`, allowing the original manifest resource name to resolve. No provider or OAuth behavior was changed.
- Kept distinguishable user-facing identity: `KeeAnywhere-GDrive` in assembly title, menus and settings; version `2.1.0.2` (informational `2.1.0-GDrive.2`). Both variants are alternatives and must not be loaded simultaneously.
- CI now validates presence and basic access of `KeeAnywhere.PluginResources.resources` before upload. Runtime tests on KeePass 2.62 remain required for this build.

## v2.1.0.2 validated resource hotfix build (2026-11-05)

- CI build: **PASS**; GitHub Actions run https://github.com/AIDevRepos/KeeAnywhere/actions/runs/42033112214.
- The CI verifies `KeeAnywhere.PluginResources.resources` exists in `KeeAnywhere.dll` and can be opened as an embedded resource stream.
- Privately configured test package: `KeeAnywhere-GDrive_2.1.0.2_KeePass-2.62.plgx`, **1,333,609 bytes**, SHA-256 `4CE740FAC6D891AF842AE88892005AC83DAAFF78CBA6994465588F6F55263D2C`.
- The source package differs from the public CI package only in the Google Drive OAuth application settings (copied unchanged from the original user-tested official PLGX), plus a regenerated PLGX UUID to bypass old compiled cache. Public source contains no private configuration.
- Restored original `KeeAnywhere` internal assembly, namespace, and entry type; retained user-visible edition title and version `2.1.0.2`.
- The 2.1.0.1 renamed-assembly builds failed in the user's KeePass 2.62 and **must not be used**.
- Validation scope: Windows C# build + PLGX creation passed, resource manifest check passed, all packaged GZip entries and project metadata parsed successfully. **KeePass 2.62 runtime test of v2.1.0.2 remains pending user confirmation.**

## Installation discovery resolution (2026-11-05)

- **User-confirmed cause of duplicate entries:** old KeeAnywhere PLGX copies were kept **elsewhere inside the KeePass installation directory**, not only under `Plugins`. In this KeePass 2.62 installation they were still discovered and loaded, producing three `KeeAnywhere Settings...` menu items. Restricting cleanup to `Plugins` was insufficient.
- **User-confirmed resolution:** move all inactive KeeAnywhere PLGX copies **completely outside the KeePass installation tree** (including its root, subfolders, and backup folders), leaving one chosen variant in `Plugins`; restart KeePass. User reported the issue resolved.
- This duplicate-loading cause is distinct from the genuine v2.1.0.1 resource-manifest failure caused by the renamed DLL. That build remains withdrawn; v2.1.0.2 restores the internal `KeeAnywhere` assembly/entry identity and has successful Windows CI/resource checks.
- **Validation boundary:** user reported that the duplicate-loading issue is resolved; a separate explicit KeePass 2.62 login/open/save test tied to the exact delivered **v2.1.0.2** hash has not been documented. Do not retroactively mark that specific binary fully tested.
- Keep variant names `KeeAnywhere-LE` and `KeeAnywhere-GDrive` in UI/version metadata; both variants are alternatives and must not be deployed together.

## v2.1.0.3 restricted Google Drive OAuth dependency repair (2026-11-18)

- **User-reported runtime blocker:** While authorizing `Google Drive (restricted)` in KeePass 2.62, `IdentityModel.OidcClient` fails with `FileNotFoundException: Microsoft.Extensions.Logging, Version=2.1.0.0`. This is a real runtime defect, not merely the old duplicate-plugin menu issue.
- **Confirmed cause:** Earlier Google Drive-only slimming retained the NuGet package declarations but removed the `.csproj` references and PLGX `References/` binaries for the `Microsoft.Extensions` runtime dependencies. The original full Lite PLGX includes them.
- **Minimal compatible repair:** Restore eight existing `Microsoft.Extensions` 2.1.0 references from the known working Lite v2.1.0 project: Configuration, Configuration.Abstractions, Configuration.Binder, DependencyInjection.Abstractions, Logging, Logging.Abstractions, Options, Primitives. No SDK version upgrades, provider restorations, or changes to Google OAuth application settings.
- The eight compressed DLL payloads together add about **132 KiB** before PLGX per-entry overhead, keeping the Google Drive-only edition lightweight.
- Bump GDrive edition assembly/file version to `2.1.0.3` (informational `2.1.0-GDrive.3`), keep the original `KeeAnywhere` internal assembly and resource identity; retain the `KeeAnywhere-GDrive` displayed variant name.
- CI adds a check that all eight DLLs are present in the Release output. The PLGX bundle must also be checked after the build. **KeePass 2.62 login/open/save behavior awaits new user confirmation**; do not transfer prior runtime PASS to the new version.
- The full Lite branch was not modified because it already includes these dependencies.

### v2.1.0.3 build and validation evidence (2026-11-18)

- Code fix: commit `c32b0624863e3d29cc3ee66cf8c17f8bd74c859d`; CI runtime smoke test: commit `72901e28969f1c2fd5775822238438d6cf4a4d16`.
- Windows CI run: https://github.com/AIDevRepos/KeeAnywhere/actions/runs/44295365431 — **SUCCESS**. Checked 8 Microsoft.Extensions 2.1.0 assemblies in Release output.
- **Key regression test:** On Windows PowerShell/.NET Framework, successfully loaded `IdentityModel.OidcClient.dll` and instantiated `IdentityModel.OidcClient.OidcClientOptions` (the constructor path that failed in the user's screenshot). This does not test live Google authorization.
- CI PLGX: **1,466,324 bytes**; SHA-256 `68C11FA7CB2A54495DB5E20D90ED3FFAEA370E03E9C287F8AFDB8D0EC8888243`; CI source has placeholder Google app credentials.
- Delivered private OAuth-configured PLGX: `KeeAnywhere-GDrive_2.1.0.3_KeePass-2.62-LoggingFix.plgx` — **1,466,411 bytes**; SHA-256 `37654FAA013B762085435C0079BFAAE9B25EBA2FF3594E980950BAF567CF6F89`.
- Static validation after OAuth app configuration transfer: 141 file entries, no duplicate or traversal paths, all GZip data streams valid, all eight required `Microsoft.Extensions` assemblies bundled. Only the two Google OAuth app constants were copied from the previously supplied working release; PLGX package UUID regenerated. No credentials were committed to the repository.
- **Awaiting user runtime validation:** KeePass 2.62 Google Drive restricted authorization, opening a test KDBX, saving changes. The earlier 2.1.0.2 GDrive build is known to fail this authorization path and is not recommended.
