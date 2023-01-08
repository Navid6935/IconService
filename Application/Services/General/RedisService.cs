using System.Text;
using Application.Common.Statics;
using Application.DTOs.Internal;
using Application.Interfaces.General;
using ExtensionMethods;

namespace Application.Services.General;

public class RedisService : IRedisService
{
    private List<RedisServiceDTO> _transActionList = new();
    private readonly IHttpClientFactory _clientFactory;
   

    public RedisService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public void CreateTransaction()
    {
        _transActionList.Clear();
    }

    public bool StringSetAsync(string key, object value)
    {
        _transActionList.Add(new RedisServiceDTO()
        {
            Key = key,
            Value = value.Serialize(),
            IsPassed = false,
            Type = RedisType.Set,
            OldValue = ""
        });
        return true;
    }
    public bool KeyDeleteAsync(string key)
    {
        _transActionList.Add(new RedisServiceDTO()
        {
            Key = key,
            Value = "",
            IsPassed = false,
            Type = RedisType.Delete,
            OldValue = ""
        });
        return true;
    }
    public async Task<bool> ExecuteAsync()
    {
        try
        {
            var client = _clientFactory.CreateClient();
            HttpContent content = new StringContent(_transActionList.Serialize(), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(StaticData.RedisTransactionUrl + "redis", content).Result.Content.ReadAsStringAsync();
            if (result.Equals("NO Connect")) throw new Exception("NO Connect to Redis server");
            return result == "OK";
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var result = await client.GetAsync(StaticData.RedisTransactionUrl + "redis/" + key.Trim()).Result.Content.ReadAsStringAsync();
            if(result.Equals("NO Connect")) throw new Exception("NO Connect to Redis server");
            return result.Deserialize<T>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

    }

    public async Task<bool> ExistAsync(string key)
    {
        var client = _clientFactory.CreateClient();
        HttpContent content = new StringContent(_transActionList.Serialize(), Encoding.UTF8, "application/json");
        var result = await client.GetAsync(StaticData.RedisTransactionUrl + "redis/" + key.Trim()).Result.Content.ReadAsStringAsync();

        return !String.IsNullOrEmpty(result);
    }


}

