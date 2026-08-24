using Hirepath.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Hirepath.Application.Tests.Support;

/// <summary>
/// A real SQLite database for a test, kept in memory via an open connection.
/// Disposing closes the connection and drops the database. No network, real SQL.
/// </summary>
public sealed class SqliteContext : IDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteContext()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<HirepathDbContext>()
            .UseSqlite(_connection)
            .Options;

        Db = new HirepathDbContext(options);
        Db.Database.EnsureCreated();
    }

    public HirepathDbContext Db { get; }

    /// <summary>A fresh context over the same database, to prove data persisted
    /// rather than lingering in the change tracker.</summary>
    public HirepathDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<HirepathDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new HirepathDbContext(options);
    }

    public void Dispose()
    {
        Db.Dispose();
        _connection.Dispose();
    }
}
