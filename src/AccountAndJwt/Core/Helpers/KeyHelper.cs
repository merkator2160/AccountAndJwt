﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AccountAndJwt.Api.Core.Helpers
{
	public static class KeyHelper
	{
		public static String CreateToken(Int32 length)
		{
			const String charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();

			return new String(Enumerable.Repeat(charPool, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
		public static String CreateRandomBase64String(Int32 length)
		{
			var randomBytes = new Byte[length];
			new RNGCryptoServiceProvider().GetBytes(randomBytes);

			return Convert.ToBase64String(randomBytes);
		}
		public static String CreatePasswordHash(String password, String salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: Encoding.UTF8.GetBytes(salt),
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
		}
	}
}