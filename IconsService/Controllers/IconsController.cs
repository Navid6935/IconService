using Application.Dtos.Icon;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IconsService.Controllers;

public class IconsController : BaseController
{
    private readonly IIconService _iconService;

    public IconsController(IIconService iconService)
    {
        _iconService = iconService;
    }
    [HttpPost("[action]")]
    public async Task<IActionResult> AddIcon([FromBody]AddIconRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.AddIcon(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteIcon([FromQuery]DeleteIconRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.DeleteIcon(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
    [HttpPut("[action]")]
    public async Task<IActionResult> UpdateIcon([FromBody]UpdateIconRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.UpdateIcon(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetIconByName([FromQuery]GetIconByNameRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.GetIconByName(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetIconById([FromQuery]GetIconByIdRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.GetIconById(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllIcons([FromQuery]GetAllIconsRequestDto requestDto,[FromHeader]string username)
    {
        var response = await _iconService.GetAllIcons(requestDto,username);
        return StatusCode(response.StatusCode, response.Result);
    }
}