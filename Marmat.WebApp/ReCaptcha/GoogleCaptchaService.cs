using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Marmat.WebApp.ReCaptcha
{
    public class GoogleCaptchaService
    {
        private readonly IOptionsMonitor<GoogleCaptchaConfig> config;

        public GoogleCaptchaService(IOptionsMonitor<GoogleCaptchaConfig> config) 
        {
            this.config = config;
        }


        public async Task<bool> VerifyToken(string token) 
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret=6Ldpr4IjAAAAABn_NT7ACSMEygfMkRm7FPQDG1F2&response={token}";

                using (var client = new HttpClient()) { 
                
                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        return false;
                    }

                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    var googleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    return googleResult.Success && googleResult.score >= 0.5;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
