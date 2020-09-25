using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace CommonLibrary
{
    public static class Common
    {
        public static class Regex
        {
            public const string UserName = @"^[a - zA - Z0 - 9] + (?:[_ -]?[a - zA - Z0 - 9]) *$";
            public const string Password = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$";
            public const string Email = @"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$";
        }


        public static string ReadFromConfig(string section, string parameter, string configFile = "AppSettings.json")
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), configFile);
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            return root.GetSection(section).GetSection(parameter).Value;
        }

        public static int GetUserId(System.Security.Claims.ClaimsPrincipal user)
        {
            var userData = user.Claims.FirstOrDefault(claim => claim.Type == System.Security.Claims.ClaimTypes.UserData);
            if (userData == null)
                return 0;

            int userId=0;
            int.TryParse(userData.Value, out userId);

            return userId;
        }

        

    }
}
