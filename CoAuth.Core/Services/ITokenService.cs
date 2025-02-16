using CoAuth.Core.Configuration;
using CoAuth.Core.DTOs;
using CoAuth.Core.Entities;

namespace CoAuth.Core.Services;

public interface ITokenService
{
    TokenDto CreateToken(UserApp userApp);

    ClientTokenDto CreateTokenByClient(Client client);
}