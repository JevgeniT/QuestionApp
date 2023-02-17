using GraphQL;
using QuestionApp.Models;

namespace QuestionApp.Extensions;

public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> SplitIntoBatches<T>(IEnumerable<T> input, int batchSize) =>
        input.Select((x, i) => new {Index = i, Item = x})
            .GroupBy(x => x.Index / batchSize)
            .Select(x => x.Select(v => v.Item));

    public static CurrencyDto MapAssetToDto(Market market)
    {
        var marketSymbol = market.MarketSymbol.Split(":");
        
        return new()
        {
            LastPrice = market?.Ticker?.LastPrice,
            Name = marketSymbol.LastOrDefault(),
            Source = marketSymbol.Length == 1 ? null : marketSymbol.FirstOrDefault()
        };
    }

    public static GraphQLRequest PriceQuery(string symbol) =>
        new($@"query price {{
                  markets(filter: {{baseSymbol:  {{_eq: ""{symbol}""}}, quoteSymbol: {{_eq: ""EUR""}}, exchangeSymbol: {{_eq: ""Binance""}}}}) {{
                    marketSymbol
                    ticker {{ lastPrice }}}}
                   }}");

    public static GraphQLRequest AssetQuery() =>
        new(@"query PageAssets {
                                  assets(sort: [{marketCapRank: ASC}]) {
                                    assetName
                                    assetSymbol
                                    marketCap
                                  }}");
}