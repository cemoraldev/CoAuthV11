using CoAuth.Core.DTOs;
using SharedLibrary.Dtos;

namespace CoAuth.Core.Services;

public interface IUserService
{
    Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);

    Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
}