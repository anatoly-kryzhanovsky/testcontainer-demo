using System.Net;
using System.Net.Http.Json;
using App.Services;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using FluentAssertions;

namespace App.Tests;

public class ApplicationTest: IAsyncLifetime
{
    private readonly INetwork _network = new NetworkBuilder()
        .WithName($"app-{Guid.NewGuid():D}")
        .Build();

    private IContainer _appContainer;
    private IContainer _keyDbContainer;

    [Fact]
    public async Task QueryApi_Add_ShouldReturn200()
    {
        //arrange
        var httpClient = new HttpClient();
        
        //act
        var response = await httpClient.PostAsync($"http://{_appContainer.Hostname}:{_appContainer.GetMappedPublicPort(8080)}/queries?query=my_query", new StringContent(""));

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task QueryApi_GetTop_ShouldReturncorrectResults()
    {
        //arrange
        var httpClient = new HttpClient();
        
        //act
        var addResponse = await httpClient.PostAsync($"http://{_appContainer.Hostname}:{_appContainer.GetMappedPublicPort(8080)}/queries?query=my_query", new StringContent(""));
        var getResponse = await httpClient.GetAsync($"http://{_appContainer.Hostname}:{_appContainer.GetMappedPublicPort(8080)}/queries/top");
        
        //assert
        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var data = await getResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<QueryStat>>();
        data.Should().BeEquivalentTo(new[]
        {
            new QueryStat("my_query", 1)
        });
    }

    public async Task InitializeAsync()
    {
        await _network.CreateAsync();
        
        var appImage = new ImageFromDockerfileBuilder()
            .WithName($"app-api-{Guid.NewGuid():D}")
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), "")
            .Build();

        await appImage.CreateAsync();

        _keyDbContainer = new ContainerBuilder()
            .WithName($"keydb-{Guid.NewGuid():D}")
            .WithImage("eqalpha/keydb")
            .WithNetwork(_network)
            .WithNetworkAliases("keydb")
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(6379))
            .Build();
        
        _appContainer = new ContainerBuilder()
            .WithName($"api-{Guid.NewGuid():D}")
            .WithImage(appImage)
            .WithNetwork(_network)
            .WithPortBinding(8080, true)
            .WithEnvironment("ASPNETCORE_URLS", "http://+:8080")
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
            .WithEnvironment("Storage__Endpoint", "keydb:6379")
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilHttpRequestIsSucceeded(r => r
                    .ForPort(8080)
                    .ForPath("/queries/top")))
            .Build();

        await _keyDbContainer.StartAsync();
        await _appContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _appContainer.DisposeAsync().AsTask();
        await _keyDbContainer.DisposeAsync().AsTask();
    }
}