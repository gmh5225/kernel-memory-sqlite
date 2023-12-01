// Copyright (c) Microsoft. All rights reserved.

using System;

namespace Microsoft.KernelMemory.SQLite;

/// <summary>
/// Base exception for all the exceptions thrown by the SQLite connector for KernelMemory
/// </summary>
public class SQLiteException : KernelMemoryException
{
    /// <inheritdoc />
    public SQLiteException() { }

    /// <inheritdoc />
    public SQLiteException(string message) : base(message) { }

    /// <inheritdoc />
    public SQLiteException(string message, Exception? innerException) : base(message, innerException) { }
}
