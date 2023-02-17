using System.Diagnostics;
using System.Security.Cryptography;
using QuestionApp.Models;
using static System.String;
using static QuestionApp.Extensions.Extensions;

namespace QuestionApp.Services;

public interface IQuestionService
{
    /// <summary>
    /// Inverts given text by word's index
    /// </summary>
    /// <param name="text">Input text</param>
    /// <returns>Inversed text</returns>
    public string InvertText(string text);

    /// <summary>
    /// Runs simultaneously 1000 tasks 
    /// </summary>
    /// <returns>Elapsed time in milliseconds</returns>
    public Task<long> LongRunAsync();

    /// <summary>
    /// Calculates SHA256 hash of a file
    /// </summary>
    /// <returns>String representation of hash</returns>
    public string CalculateFileHash();

    /// <inheritdoc cref="AssetQueryService.GetAssetPricesAsync"/>
    Task<IEnumerable<CurrencyDto>> GetAssetPricesAsync();
}

public class QuestionService : IQuestionService
{
    private readonly IAssetQueryService _assetQueryService;
    private readonly string _path = Path.Combine(Environment.CurrentDirectory, Constants.FileName);

    public QuestionService(IAssetQueryService assetQueryService)
    {
        _assetQueryService = assetQueryService;
    }

    public string InvertText(string text)
    {
        if (IsNullOrEmpty(text) || IsNullOrWhiteSpace(text)) 
            throw new ArgumentException("Invalid input");
        
        var span = text.Split(" ").AsSpan();
        span.Reverse();
        return Join(" ", span.ToArray());
    }

    public async Task<long> LongRunAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        var tasks = Enumerable.Range(0, 1000).Select((_, i) => RunWithDelay(i)).ToArray();
        await Task.WhenAll(tasks);

        return stopwatch.ElapsedMilliseconds;
    }

    private static async Task<bool> RunWithDelay(int num)
    {
        await Task.Delay(100);
        return num > int.MinValue;
    }
    
    public string CalculateFileHash()
    {
        using var sha = SHA256.Create();
        using var stream = new FileStream(_path, FileMode.Open);
        var computedHash = sha.ComputeHash(stream);
        return BitConverter.ToString(computedHash).Replace("-", "");
    }

    public async Task<IEnumerable<CurrencyDto>> GetAssetPricesAsync() 
        => (await _assetQueryService.GetAssetPricesAsync()).Select(MapAssetToDto);
}