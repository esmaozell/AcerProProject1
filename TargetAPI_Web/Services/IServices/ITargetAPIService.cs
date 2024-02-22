using TargetAPI_Web.Models.Dto;

namespace TargetAPI_Web.Services.IServices
{
    public interface ITargetAPIService
    {
        Task<T> GetAllAsync<T>(string token);
        Task<T> GetAsync<T>(int id, string token);
        Task<T> CreateAsync<T>(TargetAPICreateDto dto, string token);
        Task<T> UpdateAsync<T>(TargetAPIUpdateDto dto, string token);
        Task<T> DeleteAsync<T>(int id, string token);
    }
}
