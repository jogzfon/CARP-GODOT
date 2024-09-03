using System.Dynamic;

public class PremadeCode
{
    public string KeyWord { get; private set; }
    public string PreCodes { get; private set; }

    public PremadeCode(string keyWord, string preCodes)
    {
        KeyWord = keyWord;
        PreCodes = preCodes;
    }
}
