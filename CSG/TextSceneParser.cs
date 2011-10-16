using System;
using System.IO;
using System.Collections.Generic;

namespace Csg
{
    public class TextSceneParser : ISceneParser
    {
        private List<TreeNode> _allSpheres = new List<TreeNode>();

        public TreeNode ParseScene(string fileName)
        {
            _allSpheres.Clear();

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string line = sr.ReadLine();

                    int sphereNumber = int.Parse(line);
                    int operationNumber = sphereNumber - 1;

                    TreeOperation[] treeOp = new TreeOperation[operationNumber];

                    for (int i = 0; i < sphereNumber; i++)
                    {
                        line = sr.ReadLine();
                        _allSpheres.Add(ParseLineWithSphere(line));
                    }

                    for (int i = 0; i < operationNumber; i++)
                    {
                        line = sr.ReadLine();
                        treeOp[i] = ParseLineWithOperation(line, treeOp);
                    }

                    return treeOp[treeOp.Length - 1];
                }
            }           
        }

        private TreeSphere ParseLineWithSphere(string line)
        {
            string[] splitLine = line.Split(' ');
            return new TreeSphere(new Sphere(new float[] { float.Parse(splitLine[0]), float.Parse(splitLine[1]), float.Parse(splitLine[2]) },
                float.Parse(splitLine[3]),
                new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) }));
        }

        private TreeOperation ParseLineWithOperation(string line, TreeOperation[] treeOp)
        {
            string[] splitLine = line.Split(' ');
            OperationType ot = OperationType.difference;

            ot = GetOperationFromChar(splitLine[0][0]);

            TreeNode left = null, right = null;

            if (int.Parse(splitLine[1]) > 0)
                left = _allSpheres[int.Parse(splitLine[1]) - 1];
            else
                left = treeOp[-int.Parse(splitLine[1]) - 1];

            if (int.Parse(splitLine[2]) > 0)
                right = _allSpheres[int.Parse(splitLine[2]) - 1];
            else
                right = treeOp[-int.Parse(splitLine[2]) - 1];

            return new TreeOperation(ot, left, right);
        }

        private OperationType GetOperationFromChar(char operationChar)
        {
            switch (operationChar)
            {
                case '*':
                    return OperationType.intersection;
                case '+':
                    return OperationType.union;
                case '-':
                    return OperationType.difference;
            }
            throw new ArgumentException("Wrong operation char");
        }
    }
}
