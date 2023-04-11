using System.Linq.Expressions;
using Azure.Data.Tables;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Azure;
using static LanguageExt.Prelude;
using static Storage.Table.Helper.AzureTableStorageWrapper;

namespace Storage.Table.Helper;

internal class TableService : ITableService
{
    private readonly IAzureClientFactory<TableServiceClient> _factory;

    public TableService(IAzureClientFactory<TableServiceClient> factory) => _factory = factory;

    public async Task<TableOperation> UpsertAsync<T>(
        string category,
        string table,
        T data,
        bool createNew,
        CancellationToken token
    ) where T : ITableEntity =>
        (
            await (
                from _1 in ValidateEmptyString(category)
                from _2 in ValidateEmptyString(table)
                from tc in TableClient(_factory, category, table)
                from op in Upsert(tc, data, token, createNew)
                select op
            ).Run()
        ).Match(
            op => op,
            err =>
                TableOperation.Failure(
                    TableOperationError.New(err.Code, err.Message, err.ToException())
                )
        );

    public async Task<TableOperation> GetEntityAsync<T>(
        string category,
        string table,
        string partitionKey,
        string rowKey,
        CancellationToken token
    ) where T : class, ITableEntity =>
        (
            await (
                from _1 in ValidateEmptyString(category)
                from _2 in ValidateEmptyString(table)
                from _3 in ValidateEmptyString(partitionKey)
                from _4 in ValidateEmptyString(rowKey)
                from tc in TableClient(_factory, category, table)
                from op in GetAsync<T>(tc, partitionKey, rowKey, token)
                select op
            ).Run()
        ).Match(
            operation => operation,
            err =>
                TableOperation.Failure(
                    TableOperationError.New(err.Code, err.Message, err.ToException())
                )
        );

    public async Task<TableOperation> GetEntityListAsync<T>(
        string category,
        string table,
        Expression<Func<T, bool>> filter,
        CancellationToken token
    ) where T : class, ITableEntity =>
        (
            await (
                from tc in TableClient(_factory, category, table)
                from records in Aff(async () => await tc.QueryAsync<T>(filter).ToListAsync(token))
                select records
            ).Run()
        ).Match(
            TableOperation.GetEntities,
            err =>
                TableOperation.Failure(
                    TableOperationError.New(err.Code, err.Message, err.ToException())
                )
        );

    private static Eff<Unit> ValidateEmptyString(string s) =>
        from _1 in guardnot(
                string.IsNullOrWhiteSpace(s),
                Error.New(ErrorCodes.Invalid, ErrorMessages.Invalid)
            )
            .ToEff()
        select unit;

    private static Eff<TableClient> TableClient(
        IAzureClientFactory<TableServiceClient> factory,
        string category,
        string table
    ) =>
        from sc in GetServiceClient(factory, category)
        from tc in GetTableClient(sc, table)
        select tc;
}
