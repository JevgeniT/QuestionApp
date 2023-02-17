using System.Collections.Concurrent;
using GraphQL.Client.Abstractions;
using QuestionApp.Models;
using static QuestionApp.Extensions.Extensions;

namespace QuestionApp;

public interface IAssetQueryService
{
    /// <summary>
    /// Returns 100 cryptocurrencies and their prices in descending order 
    /// </summary>
    /// <returns>List cryptocurrencies</returns>
    Task<IEnumerable<Market>> GetAssetPricesAsync();
}


public class AssetQueryService : IAssetQueryService
{
    private readonly IGraphQLClient _client;
    
    public AssetQueryService(IGraphQLClient client)
    {
        _client = client;
    }

    private async Task<AssetResponse> GetAllAssets()
    {
        var query = AssetQuery();
        
        var assetResponse = await _client.SendQueryAsync<AssetResponse>(query);

        return assetResponse.Data;
    }
    
    public async Task<IEnumerable<Market>> GetAssetPricesAsync()
    {
        var assets = await GetAllAssets();   
        var batches = SplitIntoBatches(assets.Assets.Take(100), 20);
        var bag = new ConcurrentBag<Market>();
        
        foreach (var batch in batches)
        {
            var batchTask = batch.Select(
                async x =>
                {
                    var priceQuery = await _client.SendQueryAsync<PriceResponse>(PriceQuery(x.AssetSymbol));

                    return priceQuery.Data.Markets.FirstOrDefault(new Market { MarketSymbol = x.AssetSymbol });
                });

            var completedBatch = await Task.WhenAll(batchTask);

            foreach (var response in completedBatch) bag.Add(response);
        }

        return bag.OrderByDescending(x => x?.Ticker?.LastPrice);
    }
}