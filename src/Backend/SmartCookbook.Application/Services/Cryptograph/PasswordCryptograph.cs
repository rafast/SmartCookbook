using System.Security.Cryptography;
using System.Text;

namespace SmartCookbook.Application.Cryptograph;

public class PasswordCryptograph
{
    private readonly string _saltPassword;

    public PasswordCryptograph(string saltPassword)
    {
        _saltPassword = saltPassword;
    }

    public string Cryptograph(string password)
    {
        var passwordWithSalt = $"{password}{_saltPassword}";
        var bytes = Encoding.UTF8.GetBytes(passwordWithSalt);
        var sha512 = SHA512.Create();
        byte[] hashBytes = sha512.ComputeHash(bytes);
        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            var hex = b.ToString("x2");
            sb.Append(hex);
        }
        return sb.ToString();
    }
}
