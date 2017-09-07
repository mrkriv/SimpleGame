using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace OneCGame
{
    public static class Api
    {
        private static CookieContainer CookieContainer = new CookieContainer();

        public struct ApiResponse
        {
            public HttpStatusCode StatusCode;
            public string Content;
        }

        public static async Task<ApiResponse> ApiRequest(string Command, int Timeout = 1000)
        {
            try
            {
                var request = WebRequest.CreateHttp($"http://{Properties.Settings.Default.Server}/{Command}");
                request.Method = "GET";
                request.Timeout = Timeout;
                request.CookieContainer = CookieContainer;

                var response = (HttpWebResponse)await request.GetResponseAsync();
                var content = "";

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            content = reader.ReadToEnd();
                        }
                    }
                }

                return new ApiResponse
                {
                    StatusCode = response.StatusCode,
                    Content = content
                };
            }
            catch
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = ""
                };
            }
        }
    }
}