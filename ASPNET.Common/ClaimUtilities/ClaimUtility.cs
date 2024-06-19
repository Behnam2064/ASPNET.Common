using DOTNET.Common.Reflections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ASPNET.Common.ClaimUtilities
{
    public class ClaimUtility
    {
        public static long? GetUserId(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                long userId = long.Parse(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value);
                return userId;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public static string GetUserEmail(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                return claimsIdentity.FindFirst(ClaimTypes.Email).Value;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public static List<string> GetRolse(ClaimsPrincipal User)
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                List<string> rolse = new List<string>();
                foreach (var item in claimsIdentity.Claims.Where(p => p.Type.EndsWith("role")))
                {
                    rolse.Add(item.Value);
                }
                return rolse;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static async Task SignOutAsync(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// It supports multiple roles.
        /// Send the property with the name Roles and type string.
        /// like
        /// Roles = Admin,Manager,....
        /// Separator => ,
        /// </summary>
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public static async Task SignInAsync(SignInArguments sign)
        {
            List<System.Reflection.PropertyInfo> PropertiesSignInModel = sign.SingInModel.GetType().GetProperties().ToList();

            List<System.Reflection.FieldInfo> Constants = ReflectionHelper.GetConstants(typeof(ClaimTypes));

            List<Claim> claims = new List<Claim>();


            foreach (System.Reflection.PropertyInfo item in PropertiesSignInModel)
            {
                System.Reflection.FieldInfo? Constant = Constants.FirstOrDefault(x => x.Name.Equals(item.Name));
                if (Constant == null)
                    continue;

                string? constValue = Constant.GetValue(null)?.ToString();

                string? PropModeValue = item.GetValue(sign.SingInModel)?.ToString();

                claims.Add(new Claim(constValue, PropModeValue));
            }

            if (PropertiesSignInModel.Any(x=> x.Name == "Roles"))
            {
                System.Reflection.PropertyInfo? rolesProperty = PropertiesSignInModel.FirstOrDefault(x => x.Name == "Roles");
                string rolesStringValue = rolesProperty.GetValue(sign.SingInModel)?.ToString(); // Like Admin,Manager,...

                string[] rolesArray = rolesStringValue.Split(","); // Array of roles 

                var roleClaimValue = Constants.FirstOrDefault(x => x.Name.Equals("Role"));
                string? constValue = roleClaimValue.GetValue(null)?.ToString();
                foreach (string role in rolesArray)
                    claims.Add(new Claim(constValue, role));
            }

            //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
         
            await sign.HttpContext.SignInAsync(principal, sign.AuthenticationProperties);


        }
    }

    public class SignInArguments
    {
        /// <summary>
        /// It supports multiple roles.
        /// Send the property with the name Roles and type string.
        /// like
        /// Roles = Admin,Manager,....
        /// Separator => ,
        /// </summary>
        public required object SingInModel { get; set; }
        public required HttpContext HttpContext { get; set; }
        public required AuthenticationProperties AuthenticationProperties { get; set; }
    }
}
