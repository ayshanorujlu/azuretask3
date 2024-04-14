using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Models
{
    public class Store : ITableEntity
    {
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? Address { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; } = DateTimeOffset.MinValue;
        public ETag ETag { get; set; } = ETag.All;
    }
}
