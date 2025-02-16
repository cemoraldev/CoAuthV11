using CoAuth.Core.DTOs;
using SharedLibrary.Dtos;

namespace CoAuth.Core.Services;

public interface IAuthenticationService
{
    Task<Response<TokenDto>> CreateTokenAsync(LoginDto? loginDto);

    Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

 
    // Kullanımdan kaldırır. Örneğin kullanıcı Logout olmak istediği gibi durumlarda vb..
    //Veya kötü niyetli bir kullanım durumunda, ilgili kullanıcının refresh token'ının null'a set etmek için kullanılabilir.
    Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);

   Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
}

