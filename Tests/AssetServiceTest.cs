using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Abstractions;
using NSubstitute;
using NUnit.Framework;
using QuestionApp;
using QuestionApp.Models;

namespace Tests;

public class AssetServiceTest
{
    private IAssetQueryService _assetQueryService;
    private IGraphQLClient _graphQlClient;
    
    [SetUp]
    public void Setup()
    {
        _graphQlClient = Substitute.For<IGraphQLClient>();
        _assetQueryService = new AssetQueryService(_graphQlClient);
        
        _graphQlClient.SendQueryAsync<AssetResponse>(null)
            .ReturnsForAnyArgs(Task.FromResult(new GraphQLResponse<AssetResponse>
            {
                Data = new AssetResponse
                {
                    Assets = new []{ 
                        new Asset
                        {
                            AssetName = "Bitcoin",
                            AssetSymbol = "BTC",
                            MarketCap = 460464399753
                        },
                        new Asset
                        {
                            AssetName = "Ethereum",
                            AssetSymbol = "ETH",
                            MarketCap = 204013289061
                        },
                        new Asset
                        {
                            AssetName = "ICO",
                            AssetSymbol = "ICO",
                            MarketCap = null
                        },
                    }
                }
            }));

        _graphQlClient.SendQueryAsync<PriceResponse>(null)
            .ReturnsForAnyArgs(Task.FromResult(new GraphQLResponse<PriceResponse>
            {
                Data = new PriceResponse
                {
                    Markets = new Market[]
                    {
                        new ()
                        {
                            MarketSymbol = "Binance:BTC/EUR",
                            Ticker = new Ticker
                            {
                                LastPrice = 42515
                            }
                        },
                        new ()
                        {
                            MarketSymbol = "Binance:ETH/EUR",
                            Ticker = new Ticker
                            {
                                LastPrice = 3326
                            }
                        },
                    }
                }
            }));
    }

    [Test]
    public async Task GetAssetPrices_ReturnsNonEmptyResponse() 
        => Assert.True((await _assetQueryService.GetAssetPricesAsync()).Count() == 3);

    
    [Test]
    public async Task GetAssetPrices_ReturnValidResponse()
    {
        var query = await _assetQueryService.GetAssetPricesAsync();
        var btc = query.FirstOrDefault(x => x.MarketSymbol.Contains("BTC"));
       
        Assert.Greater(btc.Ticker.LastPrice, decimal.One);
    }
}