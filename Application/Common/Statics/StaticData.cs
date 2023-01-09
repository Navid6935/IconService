using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Application.Common.Statics
{
    public static class StaticData
    {

        public static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static string ToJson(object data)
        {
            try
            {
                return JsonSerializer.Serialize(data, JsonOptions);
            }
            catch (Exception)
            {
                return "NOErrorConvertObjectToJson";
            }
        }

        public static string? LogServer { get; set; }
        public static string? DatabaseConnectionString { get; set; }
        public static string? Redis { get; set; }
        public static string? LogServerName { get; set; }
        public static bool? IsDevelopment { get; set; } = false;
        public static string? RedisTransactionUrl { get; set; }
        public static int ServicePort { get; set; }
        public static string Version { get; set; } = "0.9.4.0";
       

        public static void Config(IConfiguration config)
        {

#if DEBUG
            ServicePort = 3030;
            var currentConfig = config.GetSection("Environment").Value.Trim().ToUpper();
            switch (currentConfig)
            {
                case "DEVELOPMENT":
                    LogServer = "tcp://192.168.0.164:32700/";
                    DatabaseConnectionString = "Server=127.0.0.1;Port=5433;Database=Icons;User Id= postgres;Password=159236;Include Error Detail=true";
                    Redis = "127.0.0.1:6380";
                    LogServerName = "LocalWebService";
                    RedisTransactionUrl = "http://192.168.0.231:6380/api/";
                    IsDevelopment = true;
                    break;
                case "TEST":
                    LogServer = "tcp://192.168.0.164:32700/";
                    DatabaseConnectionString = "Server=192.168.0.247;Port=5432;Database=GENERAL_DB;User Id= postgres;Password=Abc@1234;Include Error Detail=true";
                    Redis = "192.168.0.245:6379";
                    LogServerName = "localWebService";
                    RedisTransactionUrl = "http://192.168.0.231:6380/api/";
                    IsDevelopment = true;

                    break;
                case "RELEASE":
                    LogServer = "tcp://10.10.2.48:32700/";
                    DatabaseConnectionString = "Server=10.10.2.29;Port=5432;Database=GENERAL_DB;User Id= postgres;Password=Qweasd123;";
                    Redis = "10.10.2.29:6379";
                    LogServerName = "localWebService";
                    RedisTransactionUrl = "http://10.10.2.29:6380/api/";
                    IsDevelopment = false;

                    break;
                case "SQA":
                    LogServer = "tcp://192.168.1.78:32700/";
                    DatabaseConnectionString = "Server=192.168.1.80;Port=5432;Database=GENERAL_DB;User Id= postgres;Password=Abc@1234;";
                    Redis = "192.168.1.80:6379";
                    LogServerName = "localWebService";
                    RedisTransactionUrl = "http://192.168.1.80:6380/api/";
                    IsDevelopment = true;
                    break;
                case "SQA2":
                    LogServer = "tcp://192.168.1.78:32700/";
                    DatabaseConnectionString = "Server=192.168.1.80;Port=5432;Database=GENERAL_DB;User Id= postgres;Password=Abc@1234;";
                    Redis = "192.168.1.166:6379";
                    LogServerName = "localWebService";
                    RedisTransactionUrl = "http://192.168.1.166:6380/api/";
                    IsDevelopment = true;
                    break;
                case "PR":
                    LogServer = "tcp://192.168.159.23:32700/";
                    DatabaseConnectionString = "Server=192.168.159.23;Port=5432;Database=LWAAll-DeviceManager;User Id= postgres;Password=Abc@1234;";
                    Redis = "192.168.159.23:6379";
                    LogServerName = "LocalWebService";
                    RedisTransactionUrl = "http://192.168.159.23:6380/api/";
                    IsDevelopment = false;
                    break;
                default:
                    break;
            }
#else
                LogServer = System.Environment.GetEnvironmentVariable("SERVICE_LOG");
                DatabaseConnectionString = System.Environment.GetEnvironmentVariable("DB_PG_CONNECTION");
                Redis = System.Environment.GetEnvironmentVariable("DB_REDIS_CONNECTION");
                LogServerName = System.Environment.GetEnvironmentVariable("LOG_FOLDER_NAME");
                RedisTransactionUrl = System.Environment.GetEnvironmentVariable("SERVICE_REDIS");
                IsDevelopment = Boolean.Parse(System.Environment.GetEnvironmentVariable("IS_DEVELOPMENT")!);
                ServicePort =int.Parse( System.Environment.GetEnvironmentVariable("SERVICE_PORT")!);
                Version = System.Environment.GetEnvironmentVariable("VERSION");
#endif
        }
    }
}
