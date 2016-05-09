﻿using QMGAzure;
using QuoteMyGoods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Services
{
    public interface ILoggingService
    {
        void Log(string userId, string process);
    }
    public class LoggingService : ILoggingService
    {
        private MyDocumentDB _db { get; set; }

        public LoggingService()
        {
            _db = new MyDocumentDB("alexpscdb");
        }

        public async void Log(string userId, string process)
        {
            var log = new LoggingDocument(userId, process);
            await _db.CreateDocumentIfNotExists("alexpscdb", "logging", log);           
        } 
    }
}