
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common.Statics;

namespace ExtensionMethods
{
    public static class Extensions
    {
       private static JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };
        /// <summary>
        /// Serialize Object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize(this object obj)
        { 
            return  JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Deserialize Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T Deserialize<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default !;
            return JsonSerializer.Deserialize<T>(obj, options)!;
           
        }

        /// <summary>
        /// Encypt String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Encrypt(this string obj)
        {
            return CryptographyService.EncryptString(obj);
        }

        /// <summary>
        /// Decrypt String
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Decrypt(this string obj)
        {
            return CryptographyService.DecryptString(obj);
        }
    }
}

