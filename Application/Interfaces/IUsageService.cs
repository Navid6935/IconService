using Application.DTOs;
using Application.Dtos.Usage;

namespace Application.Interfaces;

public interface IUsageService
{
    Task<ResponseDTO> AddUsage(AddUsageRequestDto requestDto,string username);
    Task<ResponseDTO> GetUsageById(GetUsageByIdRequestDto requestDto,string username);
    Task<ResponseDTO> GetUsagesByIconId(GetUsagesByIconIdRequestDto requestDto,string username);
    Task<ResponseDTO> GetUsagesByIp(GetUsagesByIpRequestDto requestDto,string username);
    Task<ResponseDTO> GetUsagesByUsername(GetUsagesByUsernameRequestDto requestDto,string username);
}