using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QMGAzure
{
    public static class DocumentDBAppSettings
    {
        public const string ConnectionUri = "https://alexpscdb.documents.azure.com:443/";
        public const string PrimaryKey = "Jk8CJXJpSVC9D5NxR9ciqVeVq7sxkci5lo1I2aQhAH773zssV7ypJMl6zXrF3DdQi23il8n3e160vQ2T2PipVw";
        public const string SecondaryKey = "PBj2My6yA5MvdDgunzGgKf42qhIPtNMQqhiKBrvwEZ6Iw5EJqD8DPXCs5Ai5ISRgNg6XBk5uUX36KiVVuNvD2Q";
        public const string ConnectionString = "AccountEndpoint=https://alexpscdb.documents.azure.com:443/;AccountKey=Jk8CJXJpSVC9D5NxR9ciqVeVq7sxkci5lo1I2aQhAH773zssV7ypJMl6zXrF3DdQi23il8n3e160vQ2T2PipVw==;";
        public const string SecondaryConnectionString = "AccountEndpoint=https://alexpscdb.documents.azure.com:443/;AccountKey=PBj2My6yA5MvdDgunzGgKf42qhIPtNMQqhiKBrvwEZ6Iw5EJqD8DPXCs5Ai5ISRgNg6XBk5uUX36KiVVuNvD2Q==;";
    }

    public static class BlobStorageSettings
    {
        public const string AccountName = "qmgblobs";
        public const string Key1 = "2T6hEEyyOEkmiNqmXOjIBpud5MwU3C3+P8wuc6aD7ysgk/wxV8pPFWhY/KQ43BGrR42jjiBgF9lndk03fRoHoA==";
        public const string Key2 = "e/uqrm9HjfqeLWC6txxgWOr1nyYA+znDAMIs7YAXZqNH/kharmnWVzuaLC89lpKZXwB3JEswwwR5X45UYaHJ9A==";
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=qmgblobs;AccountKey=2T6hEEyyOEkmiNqmXOjIBpud5MwU3C3+P8wuc6aD7ysgk/wxV8pPFWhY/KQ43BGrR42jjiBgF9lndk03fRoHoA==";
    }

    public static class TableStorageSettings
    {
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=qmgtables;AccountKey=RH3EmTfPxJ4Y0bDf2hzel2GoN+pKriQCbh2WtN8X+L/qgZfBdxuYGB45EBRFz+IKObeFFMc8bPG0yQZ6cP3YVA==";
    }

    public static class RedisStorageSettings
    {
        public const string ConnectionString = "qmgrediscache.redis.cache.windows.net:6380,password=beSaRecMqNGWrES1pVKvQPzpNq6GJs1Omlmolc4KeB0=,ssl=True,abortConnect=False";
    }
}
