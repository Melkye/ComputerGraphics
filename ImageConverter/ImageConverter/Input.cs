using ImageConverter.Bmp;
using ImageConverter.Ppm;

namespace ImageConverter
{
    internal class Input
    {
        public Input(string[] args) 
        {
            string[] parameters = new string[3];

            parameters = InputCheck(args);
            ChoiseConverter(parameters[0], parameters[1], parameters[2]);
        }

        private string[] InputCheck(string[] args)
        {
            string source = "";
            string destination = "";
            string goal_format = "";
            bool isDestinationParametеrExist = false;
            bool isSourceParameterExist = false;
            bool isGoalFormatParameterExist = false;
            string[] parameters = new string[3];

            if (args.Length != 6)
            {
                throw new Exception("Wrong number of parameters (6 needed)");
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--destination")
                {
                    destination = args[i + 1];
                    isDestinationParametеrExist = true;
                }
                if (args[i] == "--source")
                {
                    source = args[i + 1];
                    isSourceParameterExist = true;
                }
                if (args[i] == "--goal-format")
                {
                    goal_format = args[i + 1];
                    isGoalFormatParameterExist = true;
                }
            }

            if (!isDestinationParametеrExist)
            {
                throw new ArgumentException("you forgot --destination");
            }
            if (!isSourceParameterExist)
            {
                throw new ArgumentException("you forgot --source");
            }
            if (!isGoalFormatParameterExist)
            {
                throw new ArgumentException("you forgot --goal-format");
            }
            if (!File.Exists(source))
            {
                throw new Exception("source is not exists");
            }
            if (Path.GetExtension(destination) != ".bmp" && Path.GetExtension(destination) != ".ppm")
            {
                throw new Exception("file extension of destination must be gif, bmp or ppm.");
            }
            if (goal_format != ".bmp" && goal_format != ".ppm")
            {
                throw new Exception("goal-format must be .ppm or .bmp");
            }
            if (Path.GetExtension(destination) == Path.GetExtension(source))
            {
                throw new Exception("destination file extension cannot be the same as source file extension");
            }
            if (Path.GetExtension(destination) != goal_format)
            {
                throw new Exception("destination file extension must be the same as goal-format");
            }

            parameters[0] = destination;
            parameters[1] = source;
            parameters[2] = goal_format;

            return parameters;
        }

        private void ChoiseConverter(string destination, string source, string goal_format)
        {
            ImageConverter ic;

            if (goal_format == ".bmp")
            {
                ic = new(new PpmImageReader(), new BmpImageWriter());
            }
            else
            {
                ic = new(new BmpImageReader(), new PpmImageWriter());
            }

            ic.Convert(source, goal_format, destination);
        }
    }
}