using AcerProProject1.Data;
using AcerProProject1.Models;
using AcerProProject1.Models.Dto;
using AcerProProject1.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcerProProject1.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private string secretKey;
        public UserRepository(ApplicationDbContext context,IConfiguration configuration,
            UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
              var user = _context.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower() &&
            u.Password == loginRequestDto.Password);
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
               };
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user
            };

            return loginResponseDto;


        }

        public async Task<LocalUser> Register(RegisterationRequestDto registerationRequestDto)
        {
            LocalUser user = new()
            {
                UserName = registerationRequestDto.UserName,
                Password = registerationRequestDto.Password,
                Name = registerationRequestDto.Name,
                Role = registerationRequestDto.Role
            };

            _context.LocalUsers.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
