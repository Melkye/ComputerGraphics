namespace Bmp.Common;

public readonly struct BmpFileHeader
{
    public string Signature { get; }
    public int FileSize { get; }
    public int DataOffset { get; }
    public BmpFileHeader(string signature, int fileSize, int dataOffset)
    {
        Signature = signature;
        FileSize = fileSize;
        DataOffset = dataOffset;
    }
}