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

## Verification pending

KeePass 2.62 runtime load, Google Drive sign-in, opening a test KDBX, modifying and saving the KDBX have **not yet been independently or user verified** for this reduced variant. The previously tested v2.1.0 full plugin must not be treated as evidence of success for this build.
