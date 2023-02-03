using Application.Dtos.Page;
using Application.Interfaces;
using IconsService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace PagesService.Controllers;

public class PagesController : BaseController
{
    private readonly IPageService _PageService;

    public PagesController(IPageService PageService)
    {
        _PageService = PageService;
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllPages([FromQuery]GetAllPagesRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.GetAllPages(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetPageByName([FromQuery]GetPageByNameRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.GetPageByName(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetPageById([FromQuery]GetPageByIdRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.GetPageById(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> AddPage([FromBody]AddPageRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.AddPage(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdatePage([FromBody]UpdatePageRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.UpdatePage(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeletePage([FromQuery]DeletePageRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _PageService.DeletePage(requestDto,username);
        return StatusCode(response.StatusCode, response);
    }
}