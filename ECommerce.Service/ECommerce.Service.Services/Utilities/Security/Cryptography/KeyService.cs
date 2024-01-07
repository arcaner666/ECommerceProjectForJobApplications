using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Service.Services.Utilities.Security.Cryptography;

public static class KeyService
{
    public static char[] alphanumericalCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public static char[] uppercaseLettersAndNumbers =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

    public static char[] secureUppercaseLettersAndNumbers =
        "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

    public static char[] testCharacters =
        "123456789".ToCharArray();

    public static string GenerateUniqueKey(int size, char[] charArray)
    {
        byte[] data = new byte[4 * size];
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(data);
        StringBuilder result = new StringBuilder(size);
        for (int i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % charArray.Length;

            result.Append(charArray[idx]);
        }

        return result.ToString();
    }
}

