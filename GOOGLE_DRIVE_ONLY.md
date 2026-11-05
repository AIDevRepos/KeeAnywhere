# KeeAnywhere 2.1.0 — Google Drive only (KeePass 2.62)

Derived from upstream tag `v2.1.0` as a separate branch. The purpose is to create a smaller PLGX without changing the Google Drive logic.

- KeePass 2.62 fix: remove AWS SDK analyzer that causes the PLGX path check to reject the package.
- Register only Google Drive (`gdrive`) and Google Drive restricted (`gdrive-r`).
- Exclude other storage providers' C# files, resource files, and unused project DLL references.
- Remove the unconditional `Microsoft.Graph.Core` assembly preload.
- Keep `StorageType` enum values for backward compatibility of stored account configuration.

Build with GitHub Actions using Windows 2022, .NET Framework 4.6.1 reference assemblies, and the project's PlgxTool.

Validation status: compile/packaging checks required. KeePass 2.62 and Google Drive open/save testing must be done on Windows; not yet confirmed.
