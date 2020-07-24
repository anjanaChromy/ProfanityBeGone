using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;
using ProfanityBeGone.Api.Extensions;

namespace ProfanityBeGone.Api.Repositories
{
    public abstract class BaseTableStorageRepository<T> where T : TableEntity, new()
    {
        private readonly IOptions<AppSettings> _options;

        protected BaseTableStorageRepository(IOptions<AppSettings> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected abstract string TableName { get; }

        protected async Task<bool> AddAsync(T entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            var success = false;

            try
            {
                if (IsValidEntity(entity))
                {
                    var cloudTable = await this.GetTableReferenceAsync().ConfigureAwait(false);
                    var operation = TableOperation.Insert(entity);
                    var result = await cloudTable.ExecuteAsync(operation).ConfigureAwait(false);

                    success = result.HttpStatusCode < 300 && result.HttpStatusCode >= 200;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return success;
        }

        protected async Task<bool> DeleteEntityAsync(T entity)
        {
            entity.ShouldNotBeNull(nameof(entity));

            var success = false;

            try
            {
                var cloudTable = await this.GetTableReferenceAsync().ConfigureAwait(false);
                var operation = TableOperation.Delete(entity);
                var result = await cloudTable.ExecuteAsync(operation).ConfigureAwait(false);

                success = result.HttpStatusCode < 300 && result.HttpStatusCode >= 200;
            }
            catch (Exception e)
            {
                throw;
            }

            return success;
        }

        protected async Task<IEnumerable<T>> GetAsync(string partitionKey, string filterConditions = null)
        {
            partitionKey.ShouldNotBeNullOrWhitespace(nameof(partitionKey));

            var entities = new List<T>();

            try
            {
                var cloudTable = await this.GetTableReferenceAsync().ConfigureAwait(false);

                var partitionKeyQuery = TableQuery.GenerateFilterCondition(
                    "PartitionKey",
                    QueryComparisons.Equal,
                    partitionKey);

                var filteredConditions = partitionKeyQuery;

                if (!string.IsNullOrWhiteSpace(filterConditions))
                {
                    filteredConditions = TableQuery.CombineFilters(
                        filterConditions,
                        TableOperators.And,
                        partitionKeyQuery);
                }

                TableContinuationToken token = null;
                var query = new TableQuery<T>().Where(filteredConditions);

                do
                {
                    var result = await cloudTable.ExecuteQuerySegmentedAsync(query, token).ConfigureAwait(false);

                    entities.AddRange(result.Results);

                    token = result.ContinuationToken;
                }
                while (token != null);
            }
            catch (Exception e)
            {
                throw;
            }

            return entities;
        }

        protected async Task<T> GetEntityAsync(string partitionKey, string rowKey)
        {
            partitionKey.ShouldNotBeNullOrWhitespace(nameof(partitionKey));
            rowKey.ShouldNotBeNullOrWhitespace(nameof(rowKey));

            T entity = null;

            try
            {
                var cloudTable = await this.GetTableReferenceAsync().ConfigureAwait(false);
                var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var result = await cloudTable.ExecuteAsync(operation).ConfigureAwait(false);

                if (result.HttpStatusCode != HttpStatusCode.NotFound.GetHashCode() && result.Result is T)
                {
                    entity = (T) result.Result;
                }
            }
            catch (Exception e)
            {
                throw;
            }

            return entity;
        }

        protected abstract bool IsValidEntity(T entity);

        private async Task<CloudTable> GetTableReferenceAsync()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(_options.Value.AzureStorageConnectionString);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var cloudTable = cloudTableClient.GetTableReference(TableName);

            await cloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);

            return cloudTable;
        }
    }
}