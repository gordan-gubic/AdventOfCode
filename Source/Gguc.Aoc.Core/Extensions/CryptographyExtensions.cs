namespace Gguc.Aoc.Core.Extensions;

using System.Security.Cryptography;

public static class CryptographyExtensions
{
    private static readonly MD5 Md5HashAlgorithm = System.Security.Cryptography.MD5.Create();

    public static string GenerateMD5x(this string input)
    {
        return string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input)).Select(s => s.ToString("x2")));
    }

    public static string GenerateMD5(this string input)
    {
        var hashBytes = Md5HashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(input));
        return Convert.ToHexString(hashBytes);
    }
}
