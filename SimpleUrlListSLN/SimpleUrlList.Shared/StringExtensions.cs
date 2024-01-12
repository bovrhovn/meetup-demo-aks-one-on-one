using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SimpleUrlList.Shared;

public static class StringExtensions
{
    /// <summary>
    /// truncating string with number of chars by endchar
    /// </summary>
    /// <param name="original">original string</param>
    /// <param name="numberOfChars">number of char to be shown</param>
    /// <param name="endChar">char on the end</param>
    /// <returns>truncated string or string wiht endchar</returns>
    public static string Truncate(this string original, int numberOfChars = 10, string endChar = "...")
    {
        if (original.Length > numberOfChars)
            original = original[..numberOfChars];
        original += endChar;
        return original;
    }

    /// <summary>
    /// gets unique value for a string in hexadecimal string
    /// </summary>
    /// <param name="input">string you want to change</param>
    /// <returns>changed string in hexadecimal value or empty string, if error occur</returns>
    /// <remarks>
    ///     based on https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hashalgorithm.computehash
    /// </remarks>
    public static string GetUniqueValue(this string input)
    {
        try
        {
            var shaHash = SHA256.Create();
            var data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            foreach (var currentByte in data)
            {
                sBuilder.Append(currentByte.ToString("x2"));
            }

            return sBuilder.ToString();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }

        return string.Empty;
    }
}