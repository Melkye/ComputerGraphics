using ImageConverter.Bmp;
using ImageConverter.Ppm;

namespace ImageConverter
{
    internal class Input
    {
        public Input()
        {

        }

        public void InputCheck(string[] args)
        {
            string source = "";
            string destination = "";
            string goal_format = "";
            bool isDestinationParametеrExist = false;
            bool isSourceParameterExist = false;
            bool isGoalFormatParameterExist = false;

            if (args.Length > 6)
            {
                throw new Exception("Too many parameters");
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
                throw new Exception("you forget --destination");
            }
            if (!isSourceParameterExist)
            {
                throw new Exception("you forget --source");
            }
            if (!isGoalFormatParameterExist)
            {
                throw new Exception("you forget --goal-format");
            }
            if (!File.Exists(source))
            {
                throw new Exception("source is not exists");
            }
            if (Path.GetExtension(destination) != ".bmp" && Path.GetExtension(destination) != ".ppm")
            {
                throw new Exception("destination is wrong");
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
            if (string.IsNullOrEmpty(destination) || string.IsNullOrEmpty(source) || string.IsNullOrEmpty(goal_format))
            {
                throw new Exception("destination, source and goal-format must not be null or empty");
            }

            Console.WriteLine(destination + " " + source + " " + goal_format);

            Choise(destination, source, goal_format);

        }

        private void Choise(string destination, string source, string goal_format)
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