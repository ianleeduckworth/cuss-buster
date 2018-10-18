using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CussBuster.Core.Security
{
	public class PasswordHelper : IPasswordHelper
    {
		private const int _iterations = 10000;
		private const int _length = 256;
		private byte[] _salt = new Guid("91525047-9d6e-4072-a741-5f8d420b1583").ToByteArray();

		public byte[] GenerateSecurePassword(byte[] password)
		{
			var hashedPassword = GenerateHash(password, _salt, _iterations, _length);
			return hashedPassword;
		}

		public bool CompareSecurePasswords(byte[] enteredPassword, byte[] storedPassword)
		{
			var secureEnteredPassword = GenerateSecurePassword(enteredPassword);
			return secureEnteredPassword.SequenceEqual(storedPassword);
		}

		private byte[] GenerateHash(byte[] password, byte[] salt, int iterations, int length)
		{
			using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
			{
				return deriveBytes.GetBytes(length);
			}
		}
	}
}
