using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Csg
{
    public class TextLightsParser : ILightsParser
    {
        public Light[] ParseLights(string fileName)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();

            int lightsNumber = int.Parse(line);
            line = sr.ReadLine();
            Light.M = float.Parse(line);

            Light[] lights = new Light[lightsNumber];

            for (int i = 0; i < lightsNumber; i++)
            {
                line = sr.ReadLine();
                string[] splitLine = line.Split(' ');
                switch (splitLine[0][0])
                {
                    case '0':
                        lights[i] = new DirectLight(
                            new float[] { float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]) },
                            new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) });
                        break;
                    case '1':
                        lights[i] = new PointLight(
                            new float[] { float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]) },
                            new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) });
                        break;
                    case '2':
                        lights[i] = new ReflectorLight(
                            new float[] { float.Parse(splitLine[1]), float.Parse(splitLine[2]), float.Parse(splitLine[3]) },
                            new float[] { float.Parse(splitLine[4]), float.Parse(splitLine[5]), float.Parse(splitLine[6]) },
                            float.Parse(splitLine[7]),
                            new int[] { int.Parse(splitLine[8]), int.Parse(splitLine[9]), int.Parse(splitLine[10]) }
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
