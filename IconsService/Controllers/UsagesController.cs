using Application.Dtos.Usage;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IconsService.Controllers;

public class UsagesController : BaseController
{
    private readonly IUsageService _usageService;

    public UsagesController(IUsageService usageService)
    {
        _usageService = usageService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddUsage([FromBody]AddUsageRequestDto requestDto,[FromHeader]string username)
    {
        var response =await _usageService.AddUsage(requestDto, username);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetUsageById([FromQuery]GetUsageByIdRequestDto requestDto,[FromHeader]string username)
    {
        var response =await _usageService.GetUsageById(requestDto, username);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetUsagesByIconId([FromQuery]GetUsagesByIconIdRequestDto requestDto,[FromHeader]string username)
    {
        var response =await _usageService.GetUsagesByIconId(requestDto, username);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetUsagesByIp([FromQuery]GetUsagesByIpRequestDto requestDto,[FromHeader]string username)
    {
        var response =await _usageService.GetUsagesByIp(requestDto, username);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetUsagesByUsername([FromQuery]GetUsagesByUsernameRequestDto requestDto,[FromHeader]string username)
    {
        var response =await _usageService.GetUsagesByUsername(requestDto, username);
        return StatusCode(response.StatusCode, response);
    }
}