using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MozzieAiSystems.Azure
{
    /// <summary>
    /// Azure Blob Settings
    /// </summary>
    public class AzureBlobSetings
    {
        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string ContainerName { get; }
        public string SASToken { get; }

        public AzureBlobSetings(string storageAccount, string storageKey, string containerName,string sasToken)
        {

            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrEmpty(containerName))
                throw new ArgumentNullException("ContainerName");
            if(string.IsNullOrEmpty(sasToken))
                throw new ArgumentNullException("SASToken");

            this.StorageAccount = storageAccount;

            this.StorageKey = storageKey;

            this.ContainerName = containerName;
            this.SASToken = sasToken;
        }
    }
}
