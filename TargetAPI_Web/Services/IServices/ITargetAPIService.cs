using TargetAPI_Web.Models.Dto;

namespace TargetAPI_Web.Services.IServices
{
    public interface ITargetAPIService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> CreateAsync<T>(TargetAPICreateDto dto);
        Task<T> UpdateAsync<T>(TargetAPIUpdateDto dto);
        Task<T> DeleteAsync<T>(int id);
    }
}
