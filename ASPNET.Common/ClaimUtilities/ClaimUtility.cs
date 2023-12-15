using DOTNET.Common.Reflections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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

            /*
                                    {;
                                        new Claim(ClaimTypes.NameIdentifier,result.id.ToString()),;
                                        new Claim(ClaimTypes.Role, result.userRole.ToString()),;
                                        new Claim(ClaimTypes.Name, result.username),;

                                    };
            */


            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            /*AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.Now.AddMinutes(10),
                
            };
*/

            await sign.HttpContext.SignInAsync(principal, sign.AuthenticationProperties);


        }
    }

    public class SignInArguments
    {
        public required object SingInModel { get; set; }
        public required HttpContext HttpContext { get; set; }
        public required AuthenticationProperties AuthenticationProperties { get; set; }
    }
}
