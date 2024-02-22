using AcerProProject1.Models;
using AcerProProject1.Models.Dto;

namespace AcerProProject1.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto);
    }
}
