using Azure;
using Azure.Data.Tables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Models
{
    public class Product : ITableEntity
    {
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string? Color { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string? StoreKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.MinValue;
        public ETag ETag { get; set; } = ETag.All;
    }
}
