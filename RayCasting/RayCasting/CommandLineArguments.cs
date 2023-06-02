using System.Text;

namespace RayCasting
{
    internal class CommandLineArguments
    {
        public CommandLineArguments()
        { }

        public (string, string) ParserArgs(string[] args)
        {
            string source = "";
            string destination = "";
            bool sourceParameterExists = false;
            bool destinationParametеrExists = false;

            int errorsOccured = 0;
            StringBuilder exceptionMessage = new();

            foreach (string arg in args)
            {
                if (arg.StartsWith("--source="))
                {
                    source = arg["--source=".Length..].ToLower();
                    sourceParameterExists = true;
                }
                else if (arg.StartsWith("--destination="))
                {
                    destination = arg["--destination=".Length..].ToLower();
                    destinationParametеrExists = true;
                }
            }

            if (!sourceParameterExists)
            {
                errorsOccured += 1;
                exceptionMessage.Append(errorsOccured + " you forgot --source \n");
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
                if (Path.GetExtension(source) != ".obj")
                {
                    errorsOccured += 1;
                    exceptionMessage.Append(errorsOccured + " source file doesn't .obj extension \n");
                }
            }

            if (errorsOccured > 0)
            {
                throw new ArgumentException(exceptionMessage.ToString());
            }

            return (source, destination);
        }
    }
}

//using System.IO;
//using System.Text;

//namespace ImageConverter;

//// TODO change source extension retrieval

//internal class CommandLineArgsParser
//{
//    /// <summary>
//    /// reads it ignores all args that are not supported
//    /// </summary>
//    /// <exception cref="ArgumentException"> thrown if error occurs when parsing</exception>
//    public (string source, string goalFormat, string destination) ParseArgs(string[] args)
//    {
//        List<string> supportedReadFormats = new() { "ppm", "bmp", "gif" };
//        List<string> supportedWriteFormats = new() { "ppm", "bmp" };

//        int errorsOccured = 0;
//        StringBuilder exceptionMessage = new();

//        string source = "";
//        string goalFormat = "";
//        string destination = "";

//        bool sourceParameterExists = false;
//        bool goalFormatParameterExists = false;
//        bool destinationParametеrExists = false;

//        // TODO: do smth with ToLower
//        foreach (string arg in args)
//        {
//            if (arg.StartsWith("--source="))
//            {
//                source = arg["--source=".Length..].ToLower();
//                sourceParameterExists = true;
//            }
//            else if (arg.StartsWith("--goal-format="))
//            {
//                goalFormat = arg["--goal-format=".Length..].ToLower();
//                goalFormatParameterExists = true;
//            }
//            else if (arg.StartsWith("--destination="))
//            {
//                destination = arg["--destination=".Length..].ToLower();
//                destinationParametеrExists = true;
//            }
//        }

//        if (destination == "")
//        {
//            destination = Path.GetFileNameWithoutExtension(source);
//        }
//        else
//        {
//            string fileName = source[source.LastIndexOf("\\")..source.LastIndexOf(".")];
//            destination += fileName;
//        }

//        destination += "." + goalFormat;

//        //destination += "\\" + destinationFileName;


//        if (!sourceParameterExists)
//        {
//            errorsOccured += 1;
//            exceptionMessage.Append(errorsOccured + " you forgot --source \n");
//        }
//        if (!goalFormatParameterExists)
//        {
//            errorsOccured += 1;
//            exceptionMessage.Append(errorsOccured + " you forgot --goal-format \n");
//        }
//        if (!destinationParametеrExists)
//        {
//            errorsOccured += 1;
//            exceptionMessage.Append(errorsOccured + " you forgot --destination \n");
//        }

//        if (!string.IsNullOrEmpty(source))
//        {
//            if (!File.Exists(source))
//            {
//                errorsOccured += 1;
//                exceptionMessage.Append(errorsOccured + " source file doesn't exist \n");
//            }

//            string sourceFormat = new FormatReader().GetFileFormat(source);
//            // Path.GetExtension(source);
//            if (!supportedReadFormats.Contains(sourceFormat))
//            {
//                string supportedReadExtensions = string.Join(' ', supportedReadFormats);

//                errorsOccured += 1;
//                exceptionMessage.Append(errorsOccured + $" you try to read {sourceFormat} but only {supportedReadExtensions} are supported\n");
//            }
//        }

//        if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(goalFormat))
//        {
//            string sourceFormat = new FormatReader().GetFileFormat(source);

//            if (sourceFormat == goalFormat)
//            {
//                errorsOccured += 1;
//                exceptionMessage.Append(errorsOccured + $" you try to convert to the same format: {sourceFormat} to {goalFormat} \n");
//            }
//        }

//        if (errorsOccured > 0)
//        {
//            throw new ArgumentException(exceptionMessage.ToString());
//        }

//        return (source, goalFormat, destination);
//    }


//}