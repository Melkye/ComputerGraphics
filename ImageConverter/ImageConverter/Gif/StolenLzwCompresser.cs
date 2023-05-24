using System.Text;

namespace ImageConverter.Gif;

//https://github.com/CherpakAndrii/ComputerGraphics/blob/master/Reader.GIF/LZW.cs

public class StolenLzwCompresser
{
    public byte[] Decompress(IEnumerable<byte> compressedData, int codeSize)
    {
        int clearCode = (int)Math.Pow(2, codeSize);
        int endOfInformation = clearCode + 1;

        List<byte> decompressedData = new();
        var dictionary = GetInitializedDecompressorDictionary(clearCode + 1);
        int digitCapacity = codeSize + 1, index = clearCode + 2, maxIndex = clearCode * 2;
        string codeWord = string.Empty, previousCodeWord = string.Empty;

        foreach (var compressedByte in compressedData)
        {
            for (int i = 0; i <=7; i++)
            {
                var bit = (compressedByte >> i) & 1;
                codeWord += bit;

                if (codeWord.Length != digitCapacity)
                    continue;

                int codeWordInt = IntFromStr(codeWord);

                if (codeWordInt == clearCode)
                {
                    dictionary = GetInitializedDecompressorDictionary((int)Math.Pow(2, codeSize) + 1);
                    index = clearCode + 2;
                    maxIndex = clearCode * 2;
                    digitCapacity = codeSize + 1;
                    previousCodeWord = string.Empty;
                }
                else if (codeWordInt == endOfInformation)
                {
                    return decompressedData.ToArray();
                }
                else
                {
                    if (string.IsNullOrEmpty(previousCodeWord))
                    {
                        decompressedData.Add((byte)codeWordInt);
                        previousCodeWord = Convert.ToChar(codeWordInt).ToString();
                    }
                    else
                    {
                        string entry;

                        if (dictionary.TryGetValue(codeWordInt, out var value))
                        {
                            entry = value;
                        }
                        else
                        {
                            entry = previousCodeWord + previousCodeWord[0];
                        }

                        decompressedData.AddRange(Encoding.UTF32.GetBytes(entry).Where((_, ind) => ind % 4 == 0));
                        dictionary.Add(index++, previousCodeWord + entry[0]);
                        previousCodeWord = entry;

                        if (index == maxIndex && digitCapacity < 12)
                        {
                            maxIndex *= 2;
                            digitCapacity++;
                        }


                    }
                }

                codeWord = string.Empty;
            }
        }

        return decompressedData.ToArray();
    }

    private static Dictionary<int, string> GetInitializedDecompressorDictionary(int maxSize)
    {
        Dictionary<int, string> dictionary = new();
        for (int i = 0; i <= maxSize - 2; i++)
        {
            dictionary.Add(i, Convert.ToChar(i).ToString());
        }

        return dictionary;
    }

    public static int IntFromStr(string str)
    {
        var value = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '1')
                value += (int)Math.Pow(2, i);
        }
        return value;
    }
}