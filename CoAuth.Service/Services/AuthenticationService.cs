using CoAuth.Core.Configuration;
using CoAuth.Core.DTOs;
using CoAuth.Core.Entities;
using CoAuth.Core.Repositories;
using CoAuth.Core.Services;
using CoAuth.Core.UnifOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;

namespace CoAuth.Service.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly List<Client> _clients;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserApp> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<UserRefreshToken> _userRefreshTokenService;


    public AuthenticationService(IOptions<List<Client>> optionsClient, ITokenService tokenService,
        UserManager<UserApp> userManager, IUnitOfWork unitOfWork, IRepository<UserRefreshToken> userRefreshTokenService)
    {
        _clients = optionsClient.Value;
        _tokenService = tokenService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _userRefreshTokenService = userRefreshTokenService;
    }

    public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto? loginDto)
    {
        if (loginDto is null) throw new ArgumentNullException(nameof(loginDto));

        var user = await _userManager.FindByEmailAsync(loginDto.Email);

        if (user is null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

        if ((await _userManager.CheckPasswordAsync(user, loginDto.Password)) is false)
            return Response<TokenDto>.Fail("Email or Password is wrong", 400, true);

        var token = _tokenService.CreateToken(user);

        var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

        if (userRefreshToken is null)
        {
            await _userRefreshTokenService.AddAsync(new UserRefreshToken
                { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
        }
        else
        {
            userRefreshToken.Code = token.RefreshToken;
            userRefreshToken.Expiration = token.RefreshTokenExpiration;
        }

        await _unitOfWork.CommitAsync();

        return Response<TokenDto>.Success(token, 200);
    }

    public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
    {
        var existRefreshToken =
            await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken is null) return Response<TokenDto>.Fail("Refresh token not found", 404, true);
        var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);
        if (user is null) return Response<TokenDto>.Fail("User Id not found", 404, true);

        var tokenDto = _tokenService.CreateToken(user);

        existRefreshToken.Code = tokenDto.RefreshToken;
        existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

        await _unitOfWork.CommitAsync();

        return Response<TokenDto>.Success(tokenDto, 200);
    }

    public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
    {
        var existRefreshToken =
            await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
        if (existRefreshToken is null) return Response<NoDataDto>.Fail("Refresh token not found", 404, true);
        
        _userRefreshTokenService.Remove(existRefreshToken);

        await _unitOfWork.CommitAsync();

        return Response<NoDataDto>.Success(200);
    }

    public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
    {
        var client =
            _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);
        if (client is null)
        {
            return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
        }

        var token = _tokenService.CreateTokenByClient(client);

        return Response<ClientTokenDto>.Success(token, 200);
    }
}