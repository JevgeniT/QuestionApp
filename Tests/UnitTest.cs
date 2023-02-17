using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using QuestionApp;
using QuestionApp.Services;
using GraphQL.Client.Abstractions;


namespace Tests;

public class Tests
{
    private IQuestionService _questionService;
    private IAssetQueryService _assetQueryService;
    
    [SetUp]
    public void Setup()
    {
        var gqlClient = Substitute.For<IGraphQLClient>();
        _assetQueryService = Substitute.For<AssetQueryService>(gqlClient);
        _questionService = new QuestionService(_assetQueryService);
    }

    [Test]
    public void InvertText_()
    {
        const string sourceText = "abc bcd";
        const string expectedText = "bcd abc";
        var inversed = _questionService.InvertText(sourceText);
        Assert.True(inversed == expectedText);
    }
    
    [Test]
    public void InvertText_ReturnsSource_WithOneWord()
    {
        const string sourceText = "abc";
        const string expectedText = "abc";
        var inversed = _questionService.InvertText(sourceText);
        Assert.True(inversed == expectedText);
    }
    
    [Test]
    public void InvertText_Throws_IfTextIsNullOrEmpty()
    {
        Assert.Throws<ArgumentException>(() => _questionService.InvertText(null));
        Assert.Throws<ArgumentException>(() => _questionService.InvertText(""));
        Assert.Throws<ArgumentException>(() => _questionService.InvertText(" "));
    }
    
    [Test]
    public async Task LongRunAsync_ShouldComplete_WithinAppropriateTime()
    {
        var timeTook = await _questionService.LongRunAsync();
        
        Assert.LessOrEqual(timeTook, 200);
    }
}