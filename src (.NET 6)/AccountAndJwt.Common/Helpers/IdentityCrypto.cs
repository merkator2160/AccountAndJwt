using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AccountAndJwt.Common.Helpers
{
    /// <summary>
    /// Exported from: Microsoft.AspNet.Identity.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
    /// </summary>
    public static class IdentityCrypto
    {
        private const Int32 _iterationCount = 1000;
        private const Int32 _subKeyLength = 32;
        private const Int32 _saltSize = 16;


        // IdentityCrypto /////////////////////////////////////////////////////////////////////////
        public static String HashPassword(String password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            Byte[] salt;
            Byte[] bytes;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, _saltSize, _iterationCount))
            {
                salt = rfc2898DeriveBytes.Salt;
                bytes = rfc2898DeriveBytes.GetBytes(_subKeyLength);
            }

            var inArray = new Byte[_subKeyLength + _saltSize + 1];
            Buffer.BlockCopy(salt, 0, inArray, 1, _saltSize);
            Buffer.BlockCopy(bytes, 0, inArray, _saltSize + 1, _subKeyLength);

            return Convert.ToBase64String(inArray);
        }
        public static Boolean VerifyHashedPassword(String hashedPassword, String password)
        {
            if (hashedPassword == null)
                return false;

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var numArray = Convert.FromBase64String(hashedPassword);
            if (numArray.Length != _subKeyLength + _saltSize + 1 || numArray[0] != 0)
                return false;

            var salt = new Byte[_saltSize];
            Buffer.BlockCopy(numArray, 1, salt, 0, _saltSize);

            var a = new Byte[_subKeyLength];
            Buffer.BlockCopy(numArray, _saltSize + 1, a, 0, _subKeyLength);

            Byte[] bytes;
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, _iterationCount))
                bytes = rfc2898DeriveBytes.GetBytes(_subKeyLength);

            return IdentityCrypto.ByteArraysEqual(a, bytes);
        }


        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static Boolean ByteArraysEqual(Byte[] a, Byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null || a.Length != b.Length)
                return false;

            var flag = true;
            for (var index = 0; index < a.Length; ++index)
                flag &= a[index] == b[index];

            return flag;
        }
    }
}