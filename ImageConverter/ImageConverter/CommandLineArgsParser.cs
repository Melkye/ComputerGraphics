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
        int errorsOccured = 0;
        StringBuilder exceptionMessage = new();

        string source = "";
        string goalFormat = "";
        string destination = "";

        bool sourceParameterExists = false;
        bool goalFormatParameterExists = false;
        bool destinationParametеrExists = false;

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
            destination = Path.GetFileNameWithoutExtension(source);
        }
        else
        {
            string fileName = source[source.LastIndexOf("\\")..source.LastIndexOf(".")];
            destination += fileName;
        }

        destination += "." + goalFormat;

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
        }

        if (errorsOccured > 0)
        {
            throw new ArgumentException(exceptionMessage.ToString());
        }

        return (source, goalFormat, destination);
    }
}