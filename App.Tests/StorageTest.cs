using App.Services;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace App.Tests;
/*
public class StorageTest: IAsyncLifetime
{
    private readonly IContainer _keyDbContainer = new ContainerBuilder()
        .WithName($"keydb-{Guid.NewGuid().ToString("D")}")
        .WithImage("eqalpha/keydb")
        .WithPortBinding(6379, true)
        .Build();

    [Fact]
    public async Task KeyDbStorage_AddNew_ShouldAddWithOne()
    {
        //arrange
        var settings = new Mock<IOptions<KeyDbSettings>>();
        settings
            .SetupGet(x => x.Value)
            .Returns(new KeyDbSettings
            {
                Endpoint = $"{_keyDbContainer.Hostname}:{_keyDbContainer.GetMappedPublicPort(6379)}"
            });

        var storage = new KeyDbStorage(settings.Object);

        //act
        var initialQueries = await storage.GetTopQueries();
        await storage.AddAsync("query");
        var resultQueries = await storage.GetTopQueries();

        //assert
        initialQueries.Should().BeEmpty();
        resultQueries.Should().BeEquivalentTo(new[]
        {
            new QueryStat("query", 1)
        });
    }
    
    [Fact]
    public async Task KeyDbStorage_AddExists_ShouldIncrement()
    {
        //arrange
        var settings = new Mock<IOptions<KeyDbSettings>>();
        settings
            .SetupGet(x => x.Value)
            .Returns(new KeyDbSettings
            {
                Endpoint = $"{_keyDbContainer.Hostname}:{_keyDbContainer.GetMappedPublicPort(6379)}"
            });

        var storage = new KeyDbStorage(settings.Object);

        //act
        var initialQueries = await storage.GetTopQueries();
        await storage.AddAsync("query");
        await storage.AddAsync("query");
        var resultQueries = await storage.GetTopQueries();

        //assert
        initialQueries.Should().BeEmpty();
        resultQueries.Should().BeEquivalentTo(new[]
        {
            new QueryStat("query", 2)
        });
    }
    
    [Fact]
    public async Task KeyDbStorage_GetTopQueries_ShouldReturnInCorrectOrder()
    {
        //arrange
        var settings = new Mock<IOptions<KeyDbSettings>>();
        settings
            .SetupGet(x => x.Value)
            .Returns(new KeyDbSettings
            {
                Endpoint = $"{_keyDbContainer.Hostname}:{_keyDbContainer.GetMappedPublicPort(6379)}"
            });

        var storage = new KeyDbStorage(settings.Object);

        //act
        var initialQueries = await storage.GetTopQueries();
        await storage.AddAsync("query1");
        await storage.AddAsync("query2");
        await storage.AddAsync("query2");
        var resultQueries = await storage.GetTopQueries();

        //assert
        initialQueries.Should().BeEmpty();
        resultQueries.Should().BeEquivalentTo(new[]
        {
            new QueryStat("query2", 2),
            new QueryStat("query1", 1)
        });
    }
    
    [Fact]
    public async Task KeyDbStorage_GetTopQueries_ShouldReturnOnlyTopFive()
    {
        //arrange
        var settings = new Mock<IOptions<KeyDbSettings>>();
        settings
            .SetupGet(x => x.Value)
            .Returns(new KeyDbSettings
            {
                Endpoint = $"{_keyDbContainer.Hostname}:{_keyDbContainer.GetMappedPublicPort(6379)}"
            });

        var storage = new KeyDbStorage(settings.Object);

        //act
        var initialQueries = await storage.GetTopQueries();
        for(int q = 0; q < 10; q++)
            for(int id = 0; id <= q; id++)
                await storage.AddAsync($"query{q}");
        var resultQueries = await storage.GetTopQueries();

        //assert
        initialQueries.Should().BeEmpty();
        resultQueries.Should().BeEquivalentTo(new[]
        {
            new QueryStat("query9", 10),
            new QueryStat("query8", 9),
            new QueryStat("query7", 8),
            new QueryStat("query6", 7),
            new QueryStat("query5", 6)
        });
    }

    public Task InitializeAsync()
    {
        return _keyDbContainer.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _keyDbContainer.DisposeAsync().AsTask();
    }
}*/