using CoAuth.Core.DTOs;
using CoAuth.Core.Entities;
using CoAuth.Core.Services;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;

namespace CoAuth.Service.Services;

public class UserService:IUserService
{

    private readonly UserManager<UserApp> _userManager;

    public UserService(UserManager<UserApp> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new UserApp
        {
            Email = createUserDto.Email,
            UserName = createUserDto.UserName,
        };

        var result = await _userManager.CreateAsync(user, createUserDto.Password);

        if (result.Succeeded is false)
        {
            var errors = result.Errors.Select(x => x.Description).ToList();
            return Response<UserAppDto>.Fail(new ErrorDto(), 404, true);
        }
        
        return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
    }

    public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        return user is null ? Response<UserAppDto>.Fail("Username not found",404,true) : Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user),200);
    }
}