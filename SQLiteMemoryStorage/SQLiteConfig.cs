// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.KernelMemory.SQLite;

/// <summary>
/// SQLite configuration
/// </summary>
public class SQLiteConfig
{
    /// <summary>
    /// Connection string required to connect to SQLite
    /// </summary>
    public string ConnString { get; set; } = string.Empty;
}
