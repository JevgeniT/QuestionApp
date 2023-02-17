namespace QuestionApp.Models;

public class AssetResponse
{
    public Asset[] Assets { get; set; }
}

public class Asset
{
    public string AssetName { get; set; }
    public string AssetSymbol { get; set; }
    public decimal? MarketCap { get; set; }
}

public class PriceResponse
{
    public Market[] Markets { get; set; }
}

public class Market
{
    public string MarketSymbol { get; set; }
    public Ticker Ticker { get; set; }
}

public class Ticker
{
    public decimal LastPrice { get; set; }
}

public class CurrencyDto
{
    public decimal? LastPrice { get; set; }
    public string Name { get; set; }
    public string Source { get; set; }
}