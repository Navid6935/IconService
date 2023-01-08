using System.Net;
using Application.Common;
using Application.Common.Statics;
using Application.DTOs;
using Application.Dtos.Icon.Response;
using Application.Dtos.Usage;
using Application.Dtos.Usage.Response;
using Application.Interfaces;
using Application.Interfaces.General;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UsageService : IUsageService
{
    private readonly IRedisService _redisService;
    private readonly IRepository<Usage> _usageRepository;
    private readonly ILogService<UsageService> _logService;
    
    public static string UsageByIdCacheKey= "Icons-UsageById-";
    public static string UsagesByUsernameCacheKey = "Icons-UsagesByUsername-";
    public static string UsagesByIpCacheKey = "Icons-UsagesByIp-";

    public UsageService(IRedisService redisService, IRepository<Usage> usageRepository, ILogService<UsageService> logService)
    {
        _redisService = redisService;
        _usageRepository = usageRepository;
        _logService = logService;
    }

    public async Task<ResponseDTO> AddUsage(AddUsageRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var usage = await GetUsageByUsernameAndIpAndIconId(username, requestDto.Ip, requestDto.IconId);
            
            _redisService.CreateTransaction();

            if (usage is not null)
            {
                usage.Count += requestDto.Count;
                usage.UpdatedAt = DateTime.UtcNow;
                await _usageRepository.UpdateEntity(usage);

                _redisService.KeyDeleteAsync(UsageByIdCacheKey + usage.Id);
            }
            else
            {
                usage = requestDto.Adapt<Usage>();
                usage.Username = username;

                await _usageRepository.AddEntity(usage);
            }
            
            
            _redisService.KeyDeleteAsync(UsagesByUsernameCacheKey + username);
            _redisService.KeyDeleteAsync(UsagesByIpCacheKey + requestDto.Ip);
            

            if (await _redisService.ExecuteAsync() && await _usageRepository.SaveChanges())
            {
                response = GenerateResponse(HttpStatusCode.OK, ReturnMessages.SuccessfulAdd("Usage"),
                    usage.Adapt<UsageResponseDto>());
                _logService.Info(LogType.response, response, username);
                return response;
            }

            response = GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.FailedAdd("Usage"));

            _logService.Info(LogType.response, response, username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }

    private async Task<Usage?> GetUsageByUsernameAndIpAndIconId(string username, string ip,Guid iconId)
    {
        try
        {
            return await _usageRepository.GetEntitiesByQuery().FirstOrDefaultAsync(u => u.IconId.Equals(iconId)
                && u.Username.Equals(username)
                && u.Ip.Equals(ip));
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return null;
        }
    }

    public async Task<ResponseDTO> GetUsageById(GetUsageByIdRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var usage = await GetFromCacheOrDatabaseById(requestDto);

            if (usage is null)
            {
                response = GenerateResponse(HttpStatusCode.BadRequest, ReturnMessages.NotExist("Usage"));
                _logService.Info(LogType.response, response,username);
                return response;
            }
            
            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usage"),
                usage.Adapt<UsageResponseDto>(),1);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private async Task<Usage?> GetFromCacheOrDatabaseById(GetUsageByIdRequestDto requestDto)
    {
        var usage = await _redisService.GetAsync<Usage>(UsageByIdCacheKey + requestDto.Id);

        if (usage is not null)
        {
            return usage;
        }
        usage = await _usageRepository.GetEntitiesById(requestDto.Id);
        if (usage is null)
        {
            return null;
        }
        _redisService.CreateTransaction();
        _redisService.StringSetAsync(UsageByIdCacheKey + requestDto.Id, usage,TimeSpan.FromDays(7));
        await _redisService.ExecuteAsync();
        return usage;
    }
    public async Task<ResponseDTO> GetUsagesByIconId(GetUsagesByIconIdRequestDto requestDto,string username)
    {
        try
        {
            _logService.Info(LogType.request, requestDto,username);
            
            var usages = _usageRepository.GetEntitiesByQuery()
                .Where(usage => usage.IconId.Equals(requestDto.IconId))
                .ToList();
            
            usages = Pagination.GetList<Usage>(usages, requestDto.Page, requestDto.PerPage);
            
            var usagesResponseDto = usages.Adapt<List<UsageResponseDto>?>();


            var response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usages By Icon Id"),
                usagesResponseDto, usages.Count,requestDto.Page,requestDto.PerPage);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message,username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }

    public async Task<ResponseDTO> GetUsagesByIp(GetUsagesByIpRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto,username);

            var usages =await _redisService.GetAsync<List<Usage>>(UsagesByIpCacheKey+requestDto.Ip);
            if (usages is not null)
            {
                response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usages By Ip"),
                    usages.Adapt<List<UsageResponseDto>?>(),usages.Count);
                _logService.Info(LogType.response, response,username);
                return response;
            }
            
            usages = _usageRepository.GetEntitiesByQuery()
                .Where(usage => usage.Ip.Equals(requestDto.Ip))
                .ToList();
            
            _redisService.CreateTransaction();
            _redisService.StringSetAsync(UsagesByIpCacheKey+requestDto.Ip, usages,TimeSpan.FromDays(7));
            await _redisService.ExecuteAsync();
            
            
            var usagesResponseDto = usages.Adapt<List<UsageResponseDto>?>();

            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usages By Ip"),
                usagesResponseDto, usages.Count);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message,username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }

    public async Task<ResponseDTO> GetUsagesByUsername(GetUsagesByUsernameRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto,username);

            var usages =await _redisService.GetAsync<List<Usage>>(UsagesByUsernameCacheKey+requestDto.Username);
            if (usages is not null)
            {
                usages = Pagination.GetList<Usage>(usages, requestDto.Page, requestDto.PerPage);
                response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usages By Username"),
                    usages.Adapt<List<UsageResponseDto>?>(),usages.Count);
                _logService.Info(LogType.response, response,username);
                return response;
            }

            usages = _usageRepository.GetEntitiesByQuery()
                .Where(usage => usage.Username.Equals(requestDto.Username)).ToList();

            _redisService.CreateTransaction();
            _redisService.StringSetAsync(UsagesByUsernameCacheKey+requestDto.Username, usages,TimeSpan.FromDays(7));
            await _redisService.ExecuteAsync();
            
            usages = Pagination.GetList<Usage>(usages, requestDto.Page, requestDto.PerPage);
            
            
            var usagesResponseDto = usages.Adapt<List<UsageResponseDto>?>();

            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Usages By Username"),
                usagesResponseDto, usages.Count);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message,username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private static ResponseDTO GenerateResponse(HttpStatusCode statusCode, string message, object? result = null,
        int total = 0, int page = 1, int perPage = 10)
        => new() {
            StatusCode = (int)statusCode, Message = new List<string>() { message }, Result = result, Total = total,
            Page = page, PerPage = perPage
        };
}