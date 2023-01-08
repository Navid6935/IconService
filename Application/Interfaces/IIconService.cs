using Application.DTOs;
using Application.Dtos.Icon;

namespace Application.Interfaces;

public interface IIconService
{
    Task<ResponseDTO> AddIcon(AddIconRequestDto requestDto,string username);
    Task<ResponseDTO> DeleteIcon(DeleteIconRequestDto requestDto,string username);
    Task<ResponseDTO> UpdateIcon(UpdateIconRequestDto requestDto,string username);
    Task<ResponseDTO> GetIconByName(GetIconByNameRequestDto requestDto,string username);
    Task<ResponseDTO> GetIconById(GetIconByIdRequestDto requestDto,string username);
    Task<ResponseDTO> GetAllIcons(GetAllIconsRequestDto requestDto,string username);
}