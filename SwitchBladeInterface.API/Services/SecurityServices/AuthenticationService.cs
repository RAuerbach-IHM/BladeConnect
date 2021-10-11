using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.SecurityServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITokensRepository _tokensRepository;
        public AuthenticationService(IAccountsRepository accountsRepository, ITokensRepository tokensRepository)
        {
            _accountsRepository = accountsRepository;
            _tokensRepository = tokensRepository;
        }
        public async Task<Token> VerifyCredentials(string username, string pw)
        {
            try
            {
                //Create Default Token
                var errorToken = new Token
                {
                    id = -1
                };

                //Get account
                var account = await _accountsRepository.GetAccount(username);

                if (account == null)
                {
                    return errorToken;
                }

                //Verify Password
                if (account.password_required == 1)
                {
                    //Verify Password
                    
                     byte[] hashBytes = account.password;
                     PasswordHashService hash = new PasswordHashService(hashBytes);
                     if (!hash.Verify(pw))
                     {
                         return errorToken;
                     }
                     

/*
                    byte[] hashBytes = account.password;
                    byte[] hashBytesReceived = Convert.FromBase64String(pw);

                    if (hashBytesReceived != account.password)
                    {
                        return errorToken;
                    }

    */
                    //string hash = Convert.ToBase64String(hashBytes);
                    //byte[] hashBytes = Convert.FromBase64String(hash);
                }

                //If Token exists, delete Token
                _ = await _tokensRepository.DeleteToken(username);

                //Save new Token
                Token newToken = new Token
                {
                    id = DateTime.Now.Ticks,
                    role = account.role,
                    user_name = account.user_name,
                    first_name = account.first_name,
                    last_name = account.last_name,
                    expiration = DateTime.Now.AddHours(12).Ticks,
                    account_id = account.id
                };

                await _tokensRepository.SaveToken(newToken);
                
                return newToken;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Bad Token Request: " + ex);

                Token error = new Token
                {
                    id = -1
                };
                return error;
            }
        }

        public async Task<bool> Logout(string username)
        {
            try
            {
                
                //Get account
                var account = await _accountsRepository.GetAccount(username);

                if (account == null)
                {
                    return false;
                }

              
                //If Token exists, delete Token
                int result = await _tokensRepository.DeleteToken(username);

                if(result > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bad Logout Request: " + ex);

                return false;
            }
        }

        public byte[] HashPassword( string password)
        {
            //Verify Password
            PasswordHashService hash = new PasswordHashService("password");
            byte[] hashBytes = hash.ToArray();
            return hashBytes;
        }
    }
}
