using System;
using System.Text;

public static class RandomStringGenerator
{
    private static readonly Random random = new Random();
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int length)
    {
        StringBuilder builder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            builder.Append(chars[random.Next(chars.Length)]);
        }
        return builder.ToString();
    }
}
