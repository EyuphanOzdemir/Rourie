using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CommonLibrary
{
    //the methods and constants that can be used by other projects go here
    public static class Common
    {
        public static class Regex
        {
            public const string PASSWORD_REGEX= @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            public const string PASSWORD_ERROR_MESSAGE = "The password should be at least 8 characters, and include at least and only an alphabet and a number";
        }


        public static string ReadFromConfig(string section, string parameter, string configFile = "AppSettings.json")
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), configFile);
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            return root.GetSection(section).GetSection(parameter).Value;
        }
   

    }
}
