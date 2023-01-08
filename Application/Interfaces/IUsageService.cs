using Application.DTOs;
using Application.Dtos.Usage;

namespace Application.Interfaces;

public interface IUsageService
{
    Task<ResponseDTO> AddUsage(AddUsageRequestDto requestDto);
    Task<ResponseDTO> GetUsagesBIconId(GetUsagesByIconIdRequestDto requestDto);
    Task<ResponseDTO> GetUsagesBIp(GetUsagesByIpRequestDto requestDto);
    Task<ResponseDTO> GetUsagesBUsername(GetUsagesByUsernameRequestDto requestDto);
}