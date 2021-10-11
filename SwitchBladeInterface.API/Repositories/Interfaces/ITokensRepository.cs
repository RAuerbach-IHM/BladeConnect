using SwitchBladeInterface.API.Entities;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories.Interfaces
{
    public interface ITokensRepository
    {
        Task<Token> GetToken(String username);  //Not Used?
        Task<Token> GetToken(long id);

        Task<bool> VerifyToken(long id);

        Task<int> SaveToken(Token token);

        Task<int> DeleteToken(String username);
    }
}
