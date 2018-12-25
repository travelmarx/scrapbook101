namespace Scrapbook101
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Scrapbook101.Models;

    public static class AppVariables
    {
        public static readonly string CategoryDocumentType = ConfigurationManager.AppSettings["categoryDocumentType"];
        public static readonly string ItemDocumentType = ConfigurationManager.AppSettings["itemDocumentType"];
        public static readonly string BingMapKey = ConfigurationManager.AppSettings["bingMapKey"];
        public static readonly string Endpoint = ConfigurationManager.AppSettings["endpont"];
        public static readonly string NoAssetImage = ConfigurationManager.AppSettings["noAssetImage"];
        public static List<CategoryItemDisplay> CategoryDisplayList { get; set; }
        public static List<CategoryFieldMapping> CategoryFieldMappingList { get; set; }
    }

    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static readonly bool InsertTestAssets = Boolean.Parse(ConfigurationManager.AppSettings["addTestAssets"]);
        private static DocumentClient client;

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"], new ConnectionPolicy { EnableEndpointDiscovery = false });
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
            CreateCategoryDocumentIfNotExistsAsync().Wait();
            if (InsertTestAssets)
            {
                InsertTestItemDocumentsAsync().Wait();
            }
            if (AppVariables.CategoryDisplayList == null)
            {
                GetCategoryListDocumentAsync();
            }
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCategoryDocumentIfNotExistsAsync()
        {
            try
            {
                // The GUID specified must match what is in /App_Data/categories-document.json
                var docUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, "49916d87-e565-4220-8806-b9e60c867ae6");
                await client.ReadDocumentAsync(docUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Insert category record
                    string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/categories-document.json";
                    JObject categoryDoc = JObject.Parse(File.ReadAllText(@path));
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), categoryDoc, null, true);
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task InsertTestItemDocumentsAsync()
        {
            string path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "/test-documents.json";
            JObject books = JObject.Parse(File.ReadAllText(@path));
            dynamic results  = Newtonsoft.Json.JsonConvert.DeserializeObject(books.ToString());
            foreach (var result in results.testItems)
            {
                var oneItem = Newtonsoft.Json.JsonConvert.DeserializeObject(result.ToString());
                var id = oneItem["id"].Value;
                oneItem["dateAdded"].Value = DateTime.UtcNow.ToString();
                oneItem["dateUpdated"].Value = DateTime.UtcNow.ToString();
                try
                {
                    var docUri = UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id);
                    await client.ReadDocumentAsync(docUri);
                }
                catch (DocumentClientException e)
                {
                    if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), oneItem, null, true);
                    }
                    else
                    { throw; }

                }
            }
        }

        public static async Task<bool> GetCategoryListDocumentAsync()
        {
            List<CategoryItemDisplay> categoryDisplayList = new List<CategoryItemDisplay>();
            List<CategoryFieldMapping> categoryFieldMappingList = new List<CategoryFieldMapping>();

            try
            {
                IDocumentQuery<Category> query = client.CreateDocumentQuery<Category>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId))
                    .Select(item => item)
                    .Where(item => item.Type == AppVariables.CategoryDocumentType)
                    .AsDocumentQuery();

                List<Category> results = new List<Category>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<Category>());
                }

                foreach (var category in results[0].Categories)
                {
                    // Categories object for dropdown lists
                    categoryDisplayList.Add(new CategoryItemDisplay { Name = category.Name, Id = category.Name });

                    // Categories object to keep track of only applicable object fields
                    CategoryFieldMapping mapping = new CategoryFieldMapping
                    {
                        Name = category.Name,
                        ActiveFields = new List<string>()
                    };

                    IList<System.Reflection.PropertyInfo> props = category.CategoryFields.GetType().GetProperties().ToList();
                    foreach (var prop in props)
                    {
                        if (prop.GetCustomAttributesData()[0].AttributeType.Name != "JsonIgnoreAttribute") // allow all to pass except indexer which is ignored
                        {
                            if (prop.GetValue(category.CategoryFields) != null)
                            {
                                mapping.ActiveFields.Add(prop.Name);
                            }
                        }
                    }
                    categoryFieldMappingList.Add(mapping);
                }
                AppVariables.CategoryDisplayList = categoryDisplayList;
                AppVariables.CategoryFieldMappingList = categoryFieldMappingList;
                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}