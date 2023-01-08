using Application.DTOs;
using Application.Dtos.Usage;
using Application.Interfaces;
using Application.Interfaces.General;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services;

public class UsageService : IUsageService
{
    private readonly IRedisService _redisService;
    private readonly IRepository<Usage> _iconRepository;
    private readonly ILogService<UsageService> _logService;

    public UsageService(IRedisService redisService, IRepository<Usage> iconRepository, ILogService<UsageService> logService)
    {
        _redisService = redisService;
        _iconRepository = iconRepository;
        _logService = logService;
    }

    public async Task<ResponseDTO> AddUsage(AddUsageRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> GetUsagesBIconId(GetUsagesByIconIdRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> GetUsagesBIp(GetUsagesByIpRequestDto requestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseDTO> GetUsagesBUsername(GetUsagesByUsernameRequestDto requestDto)
    {
        throw new NotImplementedException();
    }
}