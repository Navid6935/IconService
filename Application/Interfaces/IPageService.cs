using Application.DTOs;
using Application.Dtos.Page;

namespace Application.Interfaces;

public interface IPageService
{
    Task<ResponseDTO> AddPage(AddPageRequestDto requestDto,string username);
    Task<ResponseDTO> DeletePage(DeletePageRequestDto requestDto,string username);
    Task<ResponseDTO> UpdatePage(UpdatePageRequestDto requestDto,string username);
    Task<ResponseDTO> GetPageByName(GetPageByNameRequestDto requestDto,string username);
    Task<ResponseDTO> GetPageById(GetPageByIdRequestDto requestDto,string username);
    Task<ResponseDTO> GetAllPages(GetAllPagesRequestDto requestDto,string username);
}