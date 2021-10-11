using Microsoft.EntityFrameworkCore;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Entities;
using SwitchBladeInterface.API.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Repositories
{
    public class TokensRepository : ITokensRepository
    {
        private readonly SwitchBladeInterfaceContext _context;

        public TokensRepository(SwitchBladeInterfaceContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Token> GetToken(String username)
        {
            try
            {
                return await _context.Tokens.FirstOrDefaultAsync(t => t.user_name.Trim() == username.Trim());
            } catch (Exception ex)
            {
                return new Token();
            }
        }

        public async Task<bool> VerifyToken(long id)
        {
            try
            {
                var token = await _context.Tokens.FirstOrDefaultAsync(t => t.id == id);

                if(token == null && id == 200) //Create public token 200 because it was not found
                {
                    Token newToken = new Token
                    {
                        account_id = 200
                    };

                    await SaveToken(newToken);
                    return true;
                }
                else if(token == null)
                {
                    return false;
                }

                if (token.id > 0 && token.expiration > DateTime.Now.Ticks)
                {
                    //Valid token found
                    return true;
                } 
                return false;
            } catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<Token> GetToken(long id)
        {
            try
            {
                return await _context.Tokens.FirstOrDefaultAsync(t => t.id == id);
            } catch (Exception ex)
            {
                return new Token();
            }
        }

        public async Task<int> SaveToken(Token token) {
            if(token.account_id == 200)
            {
                token.id = 200;
                token.expiration = DateTime.Now.AddYears(100).Ticks;
            }
            
            try
            {
                //Save new Token
                await _context.AddAsync(token);
                return await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteToken(String username)
        {
            try
            {
                //Check if token exists for username
                var token = await GetToken(username);

                if (token == null)
                {
                    return 0;
                }
                if(token.id == 200)  //Never delete default token
                {
                    return 0;
                }

                _context.Remove(token);

            } catch(Exception ex)
            {
                return 0;
            }
            return await _context.SaveChangesAsync();
        }
    }
}
