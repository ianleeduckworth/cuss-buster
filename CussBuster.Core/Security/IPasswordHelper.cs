namespace CussBuster.Core.Security
{
	public interface IPasswordHelper
	{
		bool CompareSecurePasswords(byte[] enteredPassword, byte[] storedPassword);
		byte[] GenerateSecurePassword(byte[] password);
	}
}