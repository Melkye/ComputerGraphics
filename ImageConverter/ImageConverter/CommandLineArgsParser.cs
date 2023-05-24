using System.Text;

namespace ImageConverter;

internal class CommandLineArgsParser
{
    /// <summary>
    /// reads it ignores all args that are not supported
    /// </summary>
    /// <exception cref="ArgumentException"> thrown if error occurs when parsing</exception>
    public (string source, string goalFormat, string destination) ParseArgs(string[] args)
    {
        List<string> supportedReadFormats = new() { ".ppm", ".bmp", ".gif" };
        List<string> supportedWriteFormats = new() { ".ppm", ".bmp" };

        int errorsOccured = 0;
        StringBuilder exceptionMessage = new();

        string source = "";
        string goalFormat = "";
        string destination = "";

        bool sourceParameterExists = false;
        bool goalFormatParameterExists = false;
        bool destinationParametеrExists = false;

        // TODO: do smth with ToLower
        foreach (string arg in args)
        {
            if (arg.StartsWith("--source="))
            {
                source = arg["--source=".Length..].ToLower();
                sourceParameterExists = true;
            }
            else if (arg.StartsWith("--goal-format="))
            {
                goalFormat = arg["--goal-format=".Length..].ToLower();
                goalFormatParameterExists = true;
            }
            else if (arg.StartsWith("--destination="))
            {
                destination = arg["--destination=".Length..].ToLower();
                destinationParametеrExists = true;
            }
        }

        if (destination == "")
        {
            destination = source[..^4];
        }

        string destinationFileName = Path.GetFileNameWithoutExtension(source) + 
            "_FROM_" + Path.GetExtension(source)[1..].ToUpper() + "." + goalFormat;

        destination += destinationFileName;


        if (!sourceParameterExists)
        {
            errorsOccured += 1;
            exceptionMessage.Append(errorsOccured + " you forgot --source \n");
        }
        if (!goalFormatParameterExists)
        {
            errorsOccured += 1;
            exceptionMessage.Append(errorsOccured + " you forgot --goal-format \n");
        }
        if (!destinationParametеrExists)
        {
            errorsOccured += 1;
            exceptionMessage.Append(errorsOccured + " you forgot --destination \n");
        }

        if (!string.IsNullOrEmpty(source))
        {
            if (!File.Exists(source))
            {
                errorsOccured += 1;
                exceptionMessage.Append(errorsOccured + " source file doesn't exist \n");
            }

            string extensionToRead = Path.GetExtension(source);
            if (!supportedReadFormats.Contains(extensionToRead))
            {
                string supportedReadExtensions = string.Join(' ', supportedReadFormats);

                errorsOccured += 1;
                exceptionMessage.Append(errorsOccured + $" you try to read {extensionToRead} but only {supportedReadExtensions} are supported\n");
            }
        }

        if (!string.IsNullOrEmpty(goalFormat)
            && !supportedWriteFormats.Contains("." + goalFormat))
        {
            string extensionToWrite = "." + goalFormat;
            if (!supportedReadFormats.Contains(extensionToWrite))
            {
                string supportedWriteExtensions = string.Join(' ', supportedWriteFormats);

                errorsOccured += 1;
                exceptionMessage.Append(errorsOccured + $" you try to convert to {extensionToWrite} but only {supportedWriteExtensions} are supported\n");
            }
        }

        if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(goalFormat))
        {
            string extensionToRead = Path.GetExtension(source);
            string extensionToWrite = "." + goalFormat;

            if (extensionToRead == extensionToWrite)
            {
                errorsOccured += 1;
                exceptionMessage.Append(errorsOccured + $" you try to convert to the same format: {extensionToRead} to {extensionToWrite} \n");
            }
        }

        if (errorsOccured > 0)
        {
            throw new ArgumentException(exceptionMessage.ToString());
        }

        return (source, goalFormat, destination);
    }
}