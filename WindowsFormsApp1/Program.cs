using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.IO;

namespace WindowsFormsApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            

            string tokenPath = "auth.token";
            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5034")
            };
            httpClient.DefaultRequestHeaders.Add("x-api-key", "12345-abcdef-67890");

            if (File.Exists(tokenPath))
            {
                string token = File.ReadAllText(tokenPath);
                if (IsTokenValid(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    Application.Run(new Landing(httpClient));
                    return;
                }
                else
                {
                    Application.Run(new LoginForm(httpClient));
                }
            }

        }

        static bool IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var expClaim = jwt.Claims.FirstOrDefault(c => c.Type == "exp");
                if (expClaim != null && long.TryParse(expClaim.Value, out long exp))
                {
                    var expDate = DateTimeOffset.FromUnixTimeSeconds(exp);
                    return expDate > DateTimeOffset.UtcNow;
                }
            }
            catch { }

            return false;
        }
    }
}
