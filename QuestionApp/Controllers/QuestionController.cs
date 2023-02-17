using Microsoft.AspNetCore.Mvc;
using QuestionApp.Models;
using QuestionApp.Services;

namespace QuestionApp.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;
    
    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    /// <summary>
    /// Inverts text
    /// </summary>
    /// <param name="text">Input text</param>
    /// <returns>Inverted text</returns>
    [HttpPost]
    [Route("~/one")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesErrorResponseType(typeof(ArgumentException))]
    public ActionResult<string> QuestionOne([FromQuery]string text) 
        => Ok(_questionService.InvertText(text));

    /// <summary>
    /// Runs multiple tasks in parallel
    /// </summary>
    /// <returns>Time in ms it took to complete</returns>
    [HttpGet]
    [Route("~/two")]
    [ProducesResponseType(typeof(string), 200)]
    public async Task<ActionResult<string>> GetQuestionTwo() 
        => Ok(new {took = await _questionService.LongRunAsync()});

    /// <summary>
    /// Calculates SHA256 hash of a file
    /// </summary>
    /// <returns>String representation of hash</returns>
    [HttpPost]
    [Route("~/three")]
    [ProducesResponseType(typeof(string), 200)]
    public ActionResult<string> GetQuestionThree() 
        => Ok(_questionService.CalculateFileHash());

    /// <summary>
    /// Returns 100 cryptocurrencies and their prices in descending order 
    /// </summary>
    /// <returns>List cryptocurrencies</returns>
    [HttpGet]
    [Route("~/four")]
    [ProducesResponseType(typeof(IEnumerable<CurrencyDto>), 200)]
    public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetQuestionFour() 
        => Ok(await _questionService.GetAssetPricesAsync());
}