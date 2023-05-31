
namespace Reader.Gif;

public class LzwCompressor
{
    public byte[] Decompress(byte[] compressedData, int lzwMinCodeSize)
    {
        int byteSize = 8;
        int clearCode = (int)Math.Pow(2, lzwMinCodeSize);
        int EOICode = clearCode + 1;
        List<byte> decompressedData = new();
        List<int> listOfCodes = new(); //for debugging

        int firstCodeSize = lzwMinCodeSize + 1;

        // Initialize code table
        Dictionary<int, string> codeTable = new();
        for (int i = 0; i <= EOICode; i++)
        {
            codeTable.Add(i, i.ToString());
        }

        int currentCodeSize = firstCodeSize;
        string K = "";
        int prevCode = -1;
        bool firstCodeAdded = false;
        int freeCode = EOICode + 1;

        int compressedDataLength = compressedData.Length;
        for (int i = 0; i < compressedDataLength * byteSize;)
        {
            // inclusive (or actually exclusive?) index
            int bitFrom = i;
            int wholeByteFrom = bitFrom / byteSize; //automatically obtain the result of floor division
            int byteShiftFrom = bitFrom % byteSize;

            // exclusive index
            int bitTo = i + currentCodeSize;
            int wholeByteTo = bitTo / byteSize; //automatically obtain the result of floor division
            int byteShiftTo = bitTo % byteSize;

            i += currentCodeSize;

            if (wholeByteTo >= compressedDataLength)
                continue;

            int newCode = 0;

            int offset = 0;
            int allBytes = 0;
            for (int j = wholeByteFrom; j <= wholeByteTo; j++)
            {
                allBytes += compressedData[j] << offset;
                offset += byteSize;
            }

            //clear redundant bits to the left
            int mask = ((1 << byteShiftTo + byteSize * (wholeByteTo - wholeByteFrom)) - 1) - ((1 << byteShiftFrom) - 1);
            allBytes &= mask;

            //move needed bits to the right, clear right bits
            allBytes = allBytes >> byteShiftFrom;

            newCode = allBytes;
            listOfCodes.Add(newCode);

            // when code size in code stream is 12, reinitialize the code table
            if (newCode == clearCode && wholeByteFrom != 0)
            {
                codeTable = new();
                for (int j = 0; j <= EOICode; j++)
                {
                    codeTable.Add(j, j.ToString());
                }

                currentCodeSize = firstCodeSize;
                K = "";
                prevCode = -1;
                freeCode = EOICode + 1;

                continue;
            }
            if (newCode == clearCode)
            {
                continue;
            }

            if (newCode == EOICode)
            {
                break;
            }

            if (prevCode == -1)
            {
                // let CODE be the first code in the code stream
                // (already is)

                // output {CODE} to index stream
                if (codeTable.TryGetValue(newCode, out var val))
                {
                    var values = val.Split(",");
                    foreach (string v in values)
                    {
                        decompressedData.Add(byte.Parse(v));
                    }
                }
                prevCode = newCode;
            }
            else
            {
                codeTable.TryGetValue(prevCode, out var prevValue);
                // is CODE in the code table?
                if (codeTable.TryGetValue(newCode, out var value))
                {
                    // output {CODE} to index stream
                    var values = value.Split(",");
                    foreach (string v in values)
                    {
                        decompressedData.Add(byte.Parse(v));
                    }

                    // let K be the first index in {CODE}
                    K = values[0];

                    // add {CODE-1}+K to the code table
                    codeTable.Add(freeCode, prevValue + ',' + K);
                    freeCode += 1;
                }
                else
                {
                    // let K be the first index of {CODE-1}
                    var prevValues = prevValue.Split(",");
                    K = prevValues[0];

                    // output {CODE-1}+K to index stream
                    foreach (string v in prevValues)
                    {
                        decompressedData.Add(byte.Parse(v));
                    }
                    decompressedData.Add(byte.Parse(K));

                    // add {CODE-1}+K to code table
                    codeTable.Add(freeCode, prevValue + ',' + K);
                    freeCode += 1;
                }
            }

            // if freeCode close to limit, make code size bigger by 1 bit
            if (freeCode == (Math.Pow(2, currentCodeSize)) && currentCodeSize < 12)
            {
                currentCodeSize += 1;
            }
            prevCode = newCode;
        }

        return decompressedData.ToArray();
    }
}