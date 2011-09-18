using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

namespace Csg
{
    public class TextSceneParser : ISceneParser
    {
        public TreeOperation ReadFile(string fileName)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();

            int sphereNumber = int.Parse(line);

            TreeOperation[] treeOp = new TreeOperation[sphereNumber - 1];
            TreeNode.AllTreeSpheres = new TreeSphere[sphereNumber];

            for (int i = 0; i < sphereNumber; i++)
            {
                line = sr.ReadLine();
                string[] splitLine = line.Split(' ');
                TreeNode.AllTreeSpheres[i] = new TreeSphere(new Sphere(new float[] { float.Parse(splitLine[0]), float.Parse(splitLine[1]), float.Parse(splitLine[2]) },
                    float.Parse(splitLine[3]),
                    new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) }));
            }

            for (int i = 1; i <= sphereNumber - 1; i++)
            {
                line = sr.ReadLine();
                string[] splitLine = line.Split(' ');
                OperationType ot = OperationType.difference;
                if (splitLine[0][0] == '*')
                    ot = OperationType.intersection;
                if (splitLine[0][0] == '+')
                    ot = OperationType.union;
                if (splitLine[0][0] == '-')
                    ot = OperationType.difference;

                TreeNode left = null, right = null;

                if (int.Parse(splitLine[1]) > 0)
                    left = TreeNode.AllTreeSpheres[int.Parse(splitLine[1]) - 1];
                else
                    left = treeOp[-int.Parse(splitLine[1]) - 1];

                if (int.Parse(splitLine[2]) > 0)
                    right = TreeNode.AllTreeSpheres[int.Parse(splitLine[2]) - 1];
                else
                    right = treeOp[-int.Parse(splitLine[2]) - 1];

                treeOp[i - 1] = new TreeOperation(ot, left, right);

            }

            sr.Close();
            file.Close();
            return treeOp[treeOp.Length-1];
        }
    }
}
