using System.Text;

public static class Cipher
{
    public static string EncryptDecrypt(string input, string key)
    {
        //For Debugging Remove later
        return input;

        StringBuilder result = new StringBuilder();

        for (int i = 0; i<input.Length; i++)
        {
            result.Append((char)(input[i] ^ key[i % key.Length]));
        }

        return result.ToString();
    }
}
