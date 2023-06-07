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