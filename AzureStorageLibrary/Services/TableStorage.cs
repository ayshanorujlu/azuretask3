using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : class, ITableEntity, new()
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        public TableStorage()
        {
            _tableServiceClient = new TableServiceClient(ConnectionStrings.AzureStorageConnectionString);
            _tableClient = _tableServiceClient.GetTableClient(typeof(TEntity).Name.ToLower());
            _tableClient.CreateIfNotExists();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            await _tableClient.AddEntityAsync(entity);
            return entity;
        }

        public async Task<IQueryable<TEntity>> All()
        {
            var entities = new List<TEntity>();
            await foreach (var entity in _tableClient.QueryAsync<TEntity>())
            {
                entities.Add(entity);
            }
            return entities.AsQueryable();
        }

        public async Task Delete(string partitionKey, string rowKey)
        {


            var entity = await _tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
            if (entity != null)
            {
                await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            }
            else
            {
                throw new Exception("Belirtilen varlık bulunamadı."); // Veya uygun bir istisna türü kullanılabilir
            }

            //await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<TEntity> Get(string rowKey, string partitionKey)
        {
            return await _tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
        }

        public async Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> query)
        {
            var entities = new List<TEntity>();
            await foreach (var entity in _tableClient.QueryAsync<TEntity>())
            {
                entities.Add(entity);
            }
            return entities.AsQueryable().Where(query);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            await _tableClient.UpdateEntityAsync(entity, entity.ETag);
            return entity;
        }
    }
}