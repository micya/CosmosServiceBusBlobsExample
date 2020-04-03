using Microsoft.Azure.Cosmos;
using System;
using System.Threading.Tasks;

namespace MicrosoftCSA.HelperClasses
{
    public sealed class CosmosHelper
    {
        private static readonly Lazy<CosmosHelper> lazy = new Lazy<CosmosHelper>(() => new CosmosHelper());

        public static CosmosHelper Instance { get { return lazy.Value; } }

        private static CosmosClient cosmosClient = null;

        private CosmosHelper()
        {
        }

        public void ConnectToCosmosDB(string cosmosEndPointURL, string cosmosAuthKey)
        {
            cosmosClient = new CosmosClient(cosmosEndPointURL, cosmosAuthKey);
        }

        public async Task CreateItem<T>(string cosmosDBName, string cosmosContainer, string cosmosPartitionKey, T cosmosItemObject)
        {
            DatabaseResponse dbtask = await cosmosClient.CreateDatabaseIfNotExistsAsync(cosmosDBName);
            ContainerResponse cntask = await dbtask.Database.CreateContainerIfNotExistsAsync(cosmosContainer, cosmosPartitionKey, 400);
            ItemResponse<T> itemResponse = await cntask.Container.CreateItemAsync<T>(cosmosItemObject);
        }

        public T GetItem<T>(string cosmosDBName, string cosmosContainer, string itemId, string partitionKey)
        {
            T cosmosItem = default(T);
            Database db = cosmosClient.GetDatabase(cosmosDBName);
            Container cont = db.GetContainer(cosmosContainer);
            Task<ItemResponse<T>> itemResponse = cont.ReadItemAsync<T>(itemId, new PartitionKey(partitionKey));
            itemResponse.Wait();
            if (itemResponse.Result != null)
            {
                cosmosItem = itemResponse.Result.Resource;
            }

            return cosmosItem;
        }
    }
}