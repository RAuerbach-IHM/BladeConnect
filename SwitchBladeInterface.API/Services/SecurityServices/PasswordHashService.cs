using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SwitchBladeInterface.API.Services.SecurityServices
{
    public class PasswordHashService
    {
        //USAGE
        /*
        //Store a password hash:
        PasswordHash hash = new PasswordHash("password");
        byte[] hashBytes = hash.ToArray();
 
        //Check password against a stored hash
        byte[] hashBytes = //read from store.
        PasswordHash hash = new PasswordHash(hashBytes);
        if(!hash.Verify("newly entered password"))
        throw new System.UnauthorizedAccessException();
         */

        const int SaltSize = 16, HashSize = 20, HashIter = 10000;
        readonly byte[] _salt, _hash;
        public PasswordHashService(string password)
        {
            new RNGCryptoServiceProvider().GetBytes(_salt = new byte[SaltSize]);
            _hash = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
        }
        public PasswordHashService(byte[] hashBytes)
        {
            Array.Copy(hashBytes, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hashBytes, SaltSize, _hash = new byte[HashSize], 0, HashSize);
        }
        public PasswordHashService(byte[] salt, byte[] hash)
        {
            Array.Copy(salt, 0, _salt = new byte[SaltSize], 0, SaltSize);
            Array.Copy(hash, 0, _hash = new byte[HashSize], 0, HashSize);
        }
        public byte[] ToArray()
        {
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(_salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(_hash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
        public byte[] Salt { get { return (byte[])_salt.Clone(); } }
        public byte[] Hash { get { return (byte[])_hash.Clone(); } }
        public bool Verify(string password)
        {
            byte[] test = new Rfc2898DeriveBytes(password, _salt, HashIter).GetBytes(HashSize);
            for (int i = 0; i < HashSize; i++)
                if (test[i] != _hash[i])
                    return false;
            return true;
        }
    }
}