using TargetAPI_Utility;
using TargetAPI_Web.Models;
using TargetAPI_Web.Models.Dto;
using TargetAPI_Web.Services.IServices;

namespace TargetAPI_Web.Services
{
    public class TargetAPIService : BaseService, ITargetAPIService
    {
        private readonly IHttpClientFactory clientFactory;
        private string targetAPIUrl;
        public TargetAPIService(IHttpClientFactory clientFactory,IConfiguration configuration): base(clientFactory) 
        {
            this.clientFactory = clientFactory;
            targetAPIUrl = configuration.GetValue<string>("ServicesUrls:TargetAPI");
        }

        public Task<T> CreateAsync<T>(TargetAPICreateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = targetAPIUrl + "/api/TargetAPI"
			});
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = targetAPIUrl + "/api/TargetAPI/"+id
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = targetAPIUrl + "/api/TargetAPI/GetTargetAPIs/"
			});
        }
        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = targetAPIUrl + "/api/TargetAPI/" + id,
            });
        }

        public Task<T> UpdateAsync<T>(TargetAPIUpdateDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = targetAPIUrl + "/api/TargetAPI/"+dto.Id
            });
        }
    }
}
