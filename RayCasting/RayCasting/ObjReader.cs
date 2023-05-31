namespace RayCasting
{
    internal class ObjReader
    {
        public ObjReader() { Reader(); }

        private void Reader()
        {
            double[,] Vertices; //= new double[,] { };
            int countV = 0, countF = 0;
            if (File.Exists("C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\RayCasting\\RayCasting\\cow.obj"))   
            {
                string[] lines = File.ReadAllLines("C:\\Users\\arikt\\Documents\\Programming\\ComputerGraphics\\RayCasting\\RayCasting\\cow.obj");
                foreach (var line in lines)
                {// array point3d
                    if (line.StartsWith("v "))
                    {
                        //Console.WriteLine(line);
                        countV++;
                    }
                    if (line.StartsWith("f "))
                    {
                        //Console.WriteLine(line);
                        countF++;
                    }
                }
                Console.WriteLine(countV + " " + countF);
            }
        }
    }
}
