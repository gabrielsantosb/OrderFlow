
using Microsoft.EntityFrameworkCore;
using OrderFlow.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

namespace OrderFlow.IntegrationTests.Fixtures;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer =
        new PostgreSqlBuilder("postgres:17-alpine")
            .WithDatabase("orderflow_tests")
            .WithUsername("orderflow")
            .WithPassword("test_password")
            .Build();

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        await using OrderFlowDbContext databaseContext = CreateDatabaseContext();

        await databaseContext.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }

    public OrderFlowDbContext CreateDatabaseContext()
    {
        DbContextOptions<OrderFlowDbContext> options =
            new DbContextOptionsBuilder<OrderFlowDbContext>()
                .UseNpgsql(_postgreSqlContainer.GetConnectionString())
                .Options;

        return new OrderFlowDbContext(options);
    }
}
