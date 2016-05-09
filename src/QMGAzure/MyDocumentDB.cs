﻿using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace QMGAzure
{
    public class MyDocumentDB
    {
        private const string EndpointUri = DocumentDBAppSettings.ConnectionUri;
        private const string PrimaryKey = DocumentDBAppSettings.PrimaryKey;
        private DocumentClient _client;

        public MyDocumentDB(string databaseName)
        {
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
        }

        public async Task CreateDatabaseIfNotExists(string databaseName)
        {
            try
            {
                await _client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
                //database exists
            }
            catch (DocumentClientException e)
            {
                if(e.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDatabaseAsync(new Database { Id = databaseName });
                    //created database
                }
                else
                {
                    //other error
                    throw;
                }
            }
        }

        public async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            await CreateDatabaseIfNotExists(databaseName);
            try
            {
                await _client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
                //found collection
            }
            catch (DocumentClientException e)
            {
                if(e.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    await _client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(databaseName),
                        new DocumentCollection { Id = collectionName },
                        new RequestOptions { OfferThroughput = 400 });
                }
            }
        }

        public async Task CreateDocumentIfNotExists(string databaseName, string collectionName, Object obj, string id = "noId")
        {
            await CreateDatabaseIfNotExists(databaseName);
            await CreateDocumentCollectionIfNotExists(databaseName, collectionName);
            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, id));
                //found document
            }
            catch(DocumentClientException e)
            {
                if(e.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), obj);
                    //created document
                }
            }
        }

        public LoggingDocument GetDocById(string collectionName, string id)
        {
            //var query = _client.CreateDocumentQuery<LoggingDocument>(UriFactory.CreateDocumentCollectionUri("alexpscdb", collectionName),
            //    $"SELECT * FROM {collectionName} WHERE logging.id = {id}");

            var query = _client.CreateDocumentQuery<LoggingDocument>(UriFactory.CreateDocumentCollectionUri("alexpscdb", collectionName)).
                Where(l => l.id == id); 

            return query.AsEnumerable().FirstOrDefault();
        }

        public IEnumerable<LoggingDocument> GetDocsByUserId(string collectionName, string id)
        {
            IQueryable<LoggingDocument> query = _client.CreateDocumentQuery<LoggingDocument>(UriFactory.CreateDocumentCollectionUri("alexpscdb", collectionName)).
                Where(l => l.UserId == id);

            return query.ToList().OrderByDescending(l => l.Time.Date).ThenByDescending(l => l.Time.TimeOfDay);
        }

        public IEnumerable<LoggingDocument> GetAllDocs(string collectionName, int limit = 1000)
        {
            IQueryable<LoggingDocument> query = _client.CreateDocumentQuery<LoggingDocument>(UriFactory.CreateDocumentCollectionUri("alexpscdb", collectionName),
                $"SELECT * FROM {collectionName}");

            return query.ToList().OrderByDescending(l => l.Time.Date).ThenByDescending(l => l.Time.TimeOfDay);
        }
    }

    public class LoggingDocument
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string Process { get; set; }
        public DateTime Time { get; set; }

        public LoggingDocument(string userId, string process)
        {
            UserId = userId;
            Process = process;
            Time = DateTime.Now.ToUniversalTime();
        }
    }
}