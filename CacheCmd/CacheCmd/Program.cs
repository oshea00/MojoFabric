﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationServer.Caching;

////////////////////////////////////////////////////
// Simple example of getting a cache reference and 
// add/removing/updating items while listening
// for these events on the cache.
////////////////////////////////////////////////////

namespace CacheCmd
{

    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set;}
        public DateTime Published { get; set; }
        public override string ToString()
        {
            return String.Format("{0} - {1}, {2}", Title, Author, Published);
        }
    }

    public class CacheUtil
    {
        private static DataCacheFactory _factory = null;
        private static DataCache _cache = null;

        public static DataCache GetCache(string name)
        {
            if (_cache != null)
                return _cache;

            if (String.IsNullOrEmpty(name) == true)
                name = "default";

            List<DataCacheServerEndpoint> servers = new List<DataCacheServerEndpoint>(1);
            servers.Add(new DataCacheServerEndpoint("sead-gbyd1r1-x2", 22233));
            servers.Add(new DataCacheServerEndpoint("tac-vdi087", 22233));
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();

            // Set notification properties in the config:
            configuration.NotificationProperties = new DataCacheNotificationProperties(10000, new TimeSpan(0, 0, 5));

            configuration.Servers = servers;
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties();
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);
            _factory = new DataCacheFactory(configuration);
            _cache = _factory.GetCache(name);
            return _cache;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cache = CacheUtil.GetCache("default");

            var changeAuthor = (args.Count() > 0) ? args[0] : "";

            cache.AddCacheLevelCallback(DataCacheOperations.ReplaceItem |
                                        DataCacheOperations.AddItem |
                                        DataCacheOperations.RemoveItem, CacheEvent);

            var book = (Book) cache.Get("book_01");

            if (book == null)
            {
                Console.WriteLine("Cache Empty - creating item...");
                cache.Add("book_01",
                          new Book() {Author = "Mike O'Shea", Title = "How to Use Appfabric", Published = DateTime.Now});
                book = (Book) cache.Get("book_01");
            }

            Console.WriteLine("Got: " + book);

            if (!String.IsNullOrEmpty(changeAuthor))
            {
                book.Author = changeAuthor;
                cache.Put("book_01", book);
            }

            var line = Console.ReadLine();

            cache.Remove("book_01");

        }

        static void CacheEvent(string cacheName, string regionName, string itemName, 
                               DataCacheItemVersion v, DataCacheOperations ops, DataCacheNotificationDescriptor d)
        {
            Console.WriteLine(String.Format("CacheEvent Occurred: {0} {1} {2} {3} {4} {5}",cacheName,regionName,itemName,v,ops,d));
        }

    }
}
