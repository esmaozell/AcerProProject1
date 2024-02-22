using TargetAPI_Web.Models.Dto;

namespace TargetAPI_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto obj);
        Task<T> RegisterAsync<T>(RegisterationRequestDto obj);
    }
}
