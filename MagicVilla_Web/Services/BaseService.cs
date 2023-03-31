using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;   
        }
        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client=httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message=new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri=new Uri(apiRequest.Url);
                if (apiRequest.Data != null) // apiRequest.Data = ??
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        // message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data));
                        message.Method = HttpMethod.Post;
                        break;

                    case SD.ApiType.PUT:
                        // message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data));
                        message.Method = HttpMethod.Put;
                        break;

                    case SD.ApiType.DELETE:
                        // message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data));
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        // message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data));
                        message.Method = HttpMethod.Get;
                        break;
                }
                HttpResponseMessage apiResponse = null;
                apiResponse=await client.SendAsync(message);
                var apiContent=await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch(Exception e)
            {
                var dto = new APIResponse
                {
                    ErrorMessages=new List<string> { Convert.ToString(e.Message)},
                    IsSuccess=false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIResponse=JsonConvert.DeserializeObject<T>(res);
                return APIResponse;
            }

        }
    }
}
