using TargetAPI_Utility;
using TargetAPI_Web.Models;
using TargetAPI_Web.Models.Dto;
using TargetAPI_Web.Services.IServices;

namespace TargetAPI_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory clientFactory;
        private string targetAPIUrl;
        public AuthService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            this.clientFactory = clientFactory;
            targetAPIUrl = configuration.GetValue<string>("ServicesUrls:TargetAPI");
        }

        Task<T> IAuthService.LoginAsync<T>(LoginRequestDto obj)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = targetAPIUrl + "/api/UsersAuth/login"
            });
        }

        Task<T> IAuthService.RegisterAsync<T>(RegisterationRequestDto obj)
        {
            var a = targetAPIUrl + "/api/UsersAuth/register";
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = obj,
                Url = targetAPIUrl + "/api/UsersAuth/register"
            });
        }
    }
}
