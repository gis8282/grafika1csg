using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Csg
{
    abstract class Light
    {
        abstract public int[] CalculateLight();

        protected int[] _colorL;
        protected static int[] _colorM;
        protected static float[] _posS;

        public static float[] Normal { get; set; }
        public static float M { get; set; }
        public static int[] ColorM { get { return _colorM; } set { _colorM = value; } }
        public int[] cL { get { return _colorL; } set { _colorL = value; } }
        public static float[] PosS { get { return _posS; } set { _posS = value; } }

        public static float[] Normalize(float[] vec)
        {
            float length = (float)Math.Sqrt(vec[0] * vec[0] + vec[1] * vec[1] + vec[2] * vec[2]);
            return new float[] { vec[0] / length, vec[1] / length, vec[2] / length };
        }

        public static Light[] ReadFile(string fileName)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();

            int lightsNumber = int.Parse(line);
            line = sr.ReadLine();
            M = float.Parse(line);

            Light[] lights = new Light[lightsNumber];

            for (int i = 0; i < lightsNumber; i++)
            {
                line = sr.ReadLine();
                string[] splitLine = line.Split(' ');
                switch (splitLine[0][0])
                {
                    case '0':
                        lights[i] = new DirectLight(
                            new float[] {float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]) },
                            new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6])});                   
                        break;
                    case '1':
                        lights[i] = new PointLight(
                            new float[] { float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]) },
                            new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) });
                        break;
                    case '2':
                        lights[i] = new ReflectorLight(
                            new float[] { float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3])},
                            new float[] { float.Parse(splitLine[4]), float.Parse(splitLine[5]), float.Parse(splitLine[6])},
                            float.Parse(splitLine[7]),
                            new int[] { int.Parse(splitLine[8]), int.Parse(splitLine[9]), int.Parse(splitLine[10])}
                            );
                        break;
                }
            }

            sr.Close();
            file.Close();
            return lights;
        }
    }

}
