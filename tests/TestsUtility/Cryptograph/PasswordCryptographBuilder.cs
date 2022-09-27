using SmartCookbook.Application.Cryptograph;

namespace TestsUtility.Cryptograph;

public class PasswordCryptographBuilder
{
    public static PasswordCryptograph Instance()
    {
        return new PasswordCryptograph("saltABCD123");
    }
}
