using System.Collections.Generic;
using System.Linq;
using KeeAnywhere.StorageProviders.GoogleDrive;

namespace KeeAnywhere.StorageProviders
{
    public static class StorageRegistry
    {
        static StorageRegistry()
        {
            // Google Drive variants retain the original URI schemes and account types.
            var descriptors = new HashSet<StorageDescriptor>();
            descriptors.Add(new StorageDescriptor(StorageType.GoogleDrive, "Google Drive", "gdrive",
                account => new GoogleDriveStorageProvider(account),
                () => new GoogleDriveStorageConfigurator(false),
                PluginResources.GoogleDrive_16x16));
            descriptors.Add(new StorageDescriptor(StorageType.GoogleDriveRestricted, "Google Drive (restricted)", "gdrive-r",
                account => new GoogleDriveStorageProvider(account),
                () => new GoogleDriveStorageConfigurator(true),
                PluginResources.GoogleDrive_16x16));
            Descriptors = descriptors.ToArray();
        }

        public static IEnumerable<StorageDescriptor> Descriptors;
    }
}
