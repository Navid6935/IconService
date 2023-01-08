using System.Net;
using Application.Common;
using Application.Common.Statics;
using Application.DTOs;
using Application.Dtos.Icon;
using Application.Dtos.Icon.Response;
using Application.Interfaces;
using Application.Interfaces.General;
using Domain.Interfaces;
using Domain.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class IconService : IIconService
{
    private readonly IRedisService _redisService;
    private readonly IRepository<Icon> _iconRepository;
    private readonly ILogService<IconService> _logService;
    
    public static string IconByIdCacheKey= "Icons-IconById-";
    public static string IconByNameCacheKey= "Icons-IconByName-";
    public static string IconsListCacheKey = "Icons-IconsList";

    public IconService(IRedisService redisService, IRepository<Icon> iconRepository, ILogService<IconService> logService)
    {
        _redisService = redisService;
        _iconRepository = iconRepository;
        _logService = logService;
    }

    public async Task<ResponseDTO> AddIcon(AddIconRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            if (IsDuplicate(requestDto.Name))
            {
                response = GenerateResponse(HttpStatusCode.BadRequest, ReturnMessages.AlreadyExist("Icon"));
                _logService.Info(LogType.response, response, username);
                return response;
            }

            var icon = requestDto.Adapt<Icon>();

            await _iconRepository.AddEntity(icon);
            
            _redisService.CreateTransaction();
            _redisService.KeyDeleteAsync(IconsListCacheKey);
            

            if (await _redisService.ExecuteAsync() && await _iconRepository.SaveChanges())
            {
                response = GenerateResponse(HttpStatusCode.OK, ReturnMessages.SuccessfulAdd("Icon"),
                    icon.Adapt<IconResponseDto>());
                _logService.Info(LogType.response, response, username);
                return response;
            }

            response = GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.FailedAdd("Icon"));

            _logService.Info(LogType.response, response, username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private bool IsDuplicate(string name) =>
        _iconRepository.GetEntitiesByQuery().FirstOrDefault(d => d.Name.Equals(name,StringComparison.OrdinalIgnoreCase)) != null;
    
    
    

    public async Task<ResponseDTO> DeleteIcon(DeleteIconRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var icon = _iconRepository.GetEntitiesByQuery().FirstOrDefault(d => d.Id.Equals(requestDto.Id));
            if (icon is null)
            {
                response =GenerateResponse(HttpStatusCode.BadRequest,ReturnMessages.NotExist("Icon"));
                _logService.Error(LogType.response, response,username);
                return response;
            }

            await _iconRepository.RemoveEntity(icon);
            
            _redisService.CreateTransaction();
            _redisService.KeyDeleteAsync(IconsListCacheKey);
            _redisService.KeyDeleteAsync(IconByIdCacheKey + icon.Id);
            _redisService.KeyDeleteAsync(IconByNameCacheKey + icon.Name);

            if (await _redisService.ExecuteAsync() && await _iconRepository.SaveChanges())
            {
                response =GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulDelete("Icon"));
                _logService.Info(LogType.response, response);
                return response;
            }

            response = GenerateResponse(HttpStatusCode.InternalServerError,ReturnMessages.FailedDelete("Icon"));
              _logService.Error(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }

    public async Task<ResponseDTO> UpdateIcon(UpdateIconRequestDto requestDto, string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var icon = await _iconRepository.GetEntitiesById(requestDto.Id);
            
            if (icon is null)
            {
                response = GenerateResponse(HttpStatusCode.BadRequest,ReturnMessages.NotExist("Icon"));
                _logService.Info(LogType.response, response);
                return response;
            }
            
            if (IsDuplicate(requestDto.Name,requestDto.Id))
            {
                response = GenerateResponse(HttpStatusCode.BadRequest, ReturnMessages.AlreadyExist("Icon"));
                _logService.Info(LogType.response, response, username);
                return response;
            }
            
            UpdateModelAndDeleteCache(requestDto, icon);

            if (await _redisService.ExecuteAsync() && await _iconRepository.SaveChanges())
            {
                response = GenerateResponse(HttpStatusCode.OK, ReturnMessages.SuccessfulUpdate("Icon"),
                    icon.Adapt<IconResponseDto>());
                _logService.Info(LogType.response, response, username);
                return response;
            }

            response = GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.FailedUpdate("Icon"));
            
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private bool IsDuplicate(string name,Guid id) =>
        _iconRepository.GetEntitiesByQuery().FirstOrDefault(d => d.Name.Equals(name,StringComparison.OrdinalIgnoreCase) && !d.Id.Equals(id)) != null;
    
    private void UpdateModelAndDeleteCache(UpdateIconRequestDto requestDto, Icon icon)
    {
        _redisService.CreateTransaction();
        _redisService.KeyDeleteAsync(IconsListCacheKey);
        _redisService.KeyDeleteAsync(IconByIdCacheKey + icon.Id);
        _redisService.KeyDeleteAsync(IconByNameCacheKey + icon.Name);
        
        
        icon.Name = requestDto.Name;
        icon.DestinationUrl = requestDto.DestinationUrl;
        icon.ImageUrl = requestDto.ImageUrl;
        icon.Order = requestDto.Order;

        _iconRepository.UpdateEntity(icon);

        
    }
    

    public async Task<ResponseDTO> GetIconByName(GetIconByNameRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var icon = await GetFromCacheOrDatabaseByName(requestDto);

            if (icon is null)
            {
                response = GenerateResponse(HttpStatusCode.BadRequest, ReturnMessages.NotExist("Icon"));
                _logService.Info(LogType.response, response,username);
                return response;
            }
            
            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Icon"),
                icon.Adapt<IconResponseDto>(),1);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private async Task<Icon?> GetFromCacheOrDatabaseByName(GetIconByNameRequestDto requestDto)
    {
        var icon = await _redisService.GetAsync<Icon>(IconByNameCacheKey + requestDto.Name);

        if (icon is not null)
        {
            return icon;
        }
        icon = await _iconRepository.GetEntitiesByQuery().FirstOrDefaultAsync(i => i.Name.Equals(requestDto.Name));
        if (icon is null)
        {
            return null;
        }
        _redisService.CreateTransaction();
        _redisService.StringSetAsync(IconByNameCacheKey + requestDto.Name, icon);
        await _redisService.ExecuteAsync();
        return icon;
    }

    public async Task<ResponseDTO> GetIconById(GetIconByIdRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var icon = await GetFromCacheOrDatabaseById(requestDto);

            if (icon is null)
            {
                response = GenerateResponse(HttpStatusCode.BadRequest, ReturnMessages.NotExist("Icon"));
                _logService.Info(LogType.response, response,username);
                return response;
            }
            
            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Icon"),
                icon.Adapt<IconResponseDto>(),1);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
            return GenerateResponse(HttpStatusCode.InternalServerError, ReturnMessages.Exception);
        }
    }
    
    private async Task<Icon?> GetFromCacheOrDatabaseById(GetIconByIdRequestDto requestDto)
    {
        var icon = await _redisService.GetAsync<Icon>(IconByIdCacheKey + requestDto.Id);

        if (icon is not null)
        {
            return icon;
        }
        icon = await _iconRepository.GetEntitiesById(requestDto.Id);
        if (icon is null)
        {
            return null;
        }
        _redisService.CreateTransaction();
        _redisService.StringSetAsync(IconByIdCacheKey + requestDto.Id, icon);
        await _redisService.ExecuteAsync();
        return icon;
    }

    public async Task<ResponseDTO> GetAllIcons(GetAllIconsRequestDto requestDto,string username)
    {
        try
        {
            ResponseDTO response;
            _logService.Info(LogType.request, requestDto, username);

            var icons = await _redisService.GetAsync<List<Icon>>(IconsListCacheKey);
            if (icons is not null)
            {
                icons = Pagination.GetList<Icon>(icons, requestDto.Page, requestDto.PerPage);
                response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Icons"),
                    icons.Adapt<List<IconResponseDto>?>(),icons.Count,requestDto.Page,requestDto.PerPage);
                _logService.Info(LogType.response, response,username);
                return response;
            }
            
            var iconsQueryable = _iconRepository.GetEntitiesByQuery();
            
            _redisService.CreateTransaction();
            _redisService.StringSetAsync(IconsListCacheKey, iconsQueryable);
            await _redisService.ExecuteAsync();
            
            icons = Pagination.GetList<Icon>(iconsQueryable, requestDto.Page, requestDto.PerPage);

            response = GenerateResponse(HttpStatusCode.OK,ReturnMessages.SuccessfulGet("Icons"),
                icons.Adapt<List<IconResponseDto>?>(), icons.Count,requestDto.Page,requestDto.PerPage);
            _logService.Info(LogType.response, response,username);
            return response;
        }
        catch (Exception e)
        {
            _logService.Error(LogType.exception, e.Message, username);
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