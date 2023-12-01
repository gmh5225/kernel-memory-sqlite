// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.KernelMemory.Diagnostics;
using Microsoft.KernelMemory.MemoryStorage;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.Connectors.Memory.Sqlite;

namespace Microsoft.KernelMemory.SQLite;

/// <summary>
/// SQLite connector for Kernel Memory.
/// </summary>
public class SQLiteMemory : IMemoryDb
{
    private readonly ILogger<SQLiteMemory> _log;
    private readonly ITextEmbeddingGeneration _embeddingGenerator;
    private SqliteMemoryStore? _store = null;
    private readonly string _connString;

    /// <summary>
    /// Create a new instance of SQLite KM connector
    /// </summary>
    /// <param name="config">SQLite configuration</param>
    /// <param name="embeddingGenerator">Text embedding generator</param>
    /// <param name="log">Application logger</param>
    public SQLiteMemory(
        SQLiteConfig config,
        ITextEmbeddingGeneration embeddingGenerator,
        ILogger<SQLiteMemory>? log = null)
    {
        this._log = log ?? DefaultLogger<SQLiteMemory>.Instance;

        this._connString = config.ConnString;

        this._embeddingGenerator = embeddingGenerator;
        if (this._embeddingGenerator == null)
        {
            throw new SQLiteException("Embedding generator not configured");
        }
    }

    /// <inheritdoc />
    public async Task CreateIndexAsync(
        string index,
        int vectorSize,
        CancellationToken cancellationToken = default)
    {
        var store = await this.GetStoreAsync().ConfigureAwait(false);
        await store.CreateCollectionAsync(index, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<string>> GetIndexesAsync(
        CancellationToken cancellationToken = default)
    {
        var store = await this.GetStoreAsync().ConfigureAwait(false);
        var list = store.GetCollectionsAsync(cancellationToken).ConfigureAwait(false);
        var result = new List<string>();
        await foreach (string indexName in list.ConfigureAwait(false)) { result.Add(indexName); }

        return result;
    }

    /// <inheritdoc />
    public async Task DeleteIndexAsync(
        string index,
        CancellationToken cancellationToken = default)
    {
        var store = await this.GetStoreAsync().ConfigureAwait(false);
        await store.DeleteCollectionAsync(index, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<string> UpsertAsync(
        string index,
        MemoryRecord record,
        CancellationToken cancellationToken = default)
    {
        // Cannot use SK connector because the record schema is different
        // e.g. SK memory doesn't support tags and has a different payload structure.

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<(MemoryRecord, double)> GetSimilarListAsync(
        string index,
        string text,
        ICollection<MemoryFilter>? filters = null,
        double minRelevance = 0,
        int limit = 1,
        bool withEmbeddings = false,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (filters != null)
        {
            foreach (MemoryFilter filter in filters)
            {
                if (filter is SQLiteMemoryFilter extendedFilter)
                {
                    // use SQLiteMemoryFilter filtering logic
                }

                // use MemoryFilter filtering logic
            }
        }

        // Cannot use SK connector because the record schema is different
        // e.g. SK memory doesn't support tags and has a different payload structure.

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IAsyncEnumerable<MemoryRecord> GetListAsync(
        string index,
        ICollection<MemoryFilter>? filters = null,
        int limit = 1,
        bool withEmbeddings = false,
        CancellationToken cancellationToken = default)
    {
        if (filters != null)
        {
            foreach (MemoryFilter filter in filters)
            {
                if (filter is SQLiteMemoryFilter extendedFilter)
                {
                    // use SQLiteMemoryFilter filtering logic
                }

                // use MemoryFilter filtering logic
            }
        }

        // Cannot use SK connector because the record schema is different
        // e.g. SK memory doesn't support tags and has a different payload structure.

        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task DeleteAsync(
        string index,
        MemoryRecord record,
        CancellationToken cancellationToken = default)
    {
        var store = await this.GetStoreAsync().ConfigureAwait(false);
        await store.RemoveAsync(index, record.Id, cancellationToken).ConfigureAwait(false);
    }

    private async Task<SqliteMemoryStore> GetStoreAsync()
    {
        return this._store ??= await SqliteMemoryStore.ConnectAsync(this._connString).ConfigureAwait(false);
    }
}
