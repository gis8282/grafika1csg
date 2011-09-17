using System;
using System.Collections.Generic;
using System.Text;

using System.IO;


namespace Csg
{
    class TreeNode
    {
       
        TreeNode _left, _right;
        public static TreeSphere[] treeSp;
        public TreeNode Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public TreeNode Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public TreeNode()
        {
        }

        public TreeNode(TreeNode left, TreeNode right){
            _left = left;
            _right = right;
        }

        public bool FindRect(out float x0, out float y0, out float x1, out float y1)
        {
            if (Left != null && Right != null)
            {
                float temp0_x0, temp0_y0, temp0_x1, temp0_y1;
                float temp1_x0, temp1_y0, temp1_x1, temp1_y1;
                bool bl = Left.FindRect(out temp0_x0, out temp0_y0, out temp0_x1, out temp0_y1);
                bool br = Right.FindRect(out temp1_x0, out temp1_y0, out temp1_x1, out temp1_y1);
                switch (((TreeOperation)this).OperationType)
                {
                    case OperationType.difference:
                        if ((temp0_x0 == temp1_x0 && temp0_y0 == temp1_y0 &&
                            temp0_x1 == temp1_x1 && temp0_y1 == temp1_y1) ||
                            bl == false)
                        {
                            x0 = x1 = -1;
                            y0 = y1 = -1;
                            return false;
                        }
                        else
                        {
                            x0 = temp0_x0;
                            y0 = temp0_y0;
                            x1 = temp0_x1;
                            y1 = temp0_y1;
                        }
                        return true;
                    case OperationType.union:
                        if (bl == false && br == false)
                        {
                            x0 = x1 = -1;
                            y0 = y1 = -1;
                            return false;
                        }
                        else if (bl == false)
                        {
                            x0 = temp1_x0;
                            y0 = temp1_y0;
                            x1 = temp1_x1;
                            y1 = temp1_y1;
                        }
                        else if (br == false)
                        {
                            x0 = temp0_x0;
                            y0 = temp0_y0;
                            x1 = temp0_x1;
                            y1 = temp0_y1;
                        }
                        else
                        {
                            x0 = Math.Min(temp0_x0, temp1_x0);
                            y0 = Math.Min(temp0_y0, temp1_y0);
                            x1 = Math.Max(temp0_x1, temp1_x1);
                            y1 = Math.Max(temp0_y1, temp1_y1);
                        }
                        return true;
                    default://case TreeOperation.OType.intersection:
                        if (bl == false || br == false)
                        {
                            x0 = x1 = -1;
                            y0 = y1 = -1;
                            return false;
                        }
                        else
                        {
                            x0 = Math.Max(temp0_x0, temp1_x0);
                            y0 = Math.Max(temp0_y0, temp1_y0);
                            x1 = Math.Min(temp0_x1, temp1_x1);
                            y1 = Math.Min(temp0_y1, temp1_y1);
                        }
                        return true;
                }
            }
            else// if (this is TreeSphere)
            {
                x0 = ((this as TreeSphere).S.CurrentPosition[0] - (this as TreeSphere).S.Radius);
                y0 = ((this as TreeSphere).S.CurrentPosition[1] - (this as TreeSphere).S.Radius);

                x1 = (((this as TreeSphere).S.CurrentPosition[0] + (this as TreeSphere).S.Radius));
                y1 = ((this as TreeSphere).S.CurrentPosition[1] + (this as TreeSphere).S.Radius);
                return true;
            }
        }

        public List<Interval> TraverseTree(float x, float y)
        {
            if (Left != null || Right != null)
            {
                List<Interval> left = Left.TraverseTree(x, y);
                List<Interval> right = Right.TraverseTree(x, y);
                    switch (((TreeOperation)this).OperationType)
                    {
                        case OperationType.difference:   
                            return Interval.Difference(left, right);
                        case OperationType.union:        
                            return Interval.Union(left, right);
                        case OperationType.intersection: 
                            return Interval.Intersection(left, right);
                    }
            }
            else if(this is TreeSphere)
            {
                Sphere s = ((TreeSphere)this).S;
                float[] c = new float[] { s.CurrentPosition[0], s.CurrentPosition[1], s.CurrentPosition[2]};

                if (Math.Pow(x - c[0], 2) + Math.Pow(y - c[1], 2) < s.Radius * s.Radius)
                {
                    float z1 = (float)(c[2] - Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                    float z2 = (float)(c[2] + Math.Sqrt(s.Radius * s.Radius - Math.Pow(x - c[0], 2f) - Math.Pow(y - c[1], 2f)));
                    List<Interval> ret_val = new List<Interval>();
                    Interval interval = new Interval(z1, z2);
                    interval.ColourA = s.Color;
                    interval.ColourB = s.Color;
                    interval.NA = new float[] { (x - c[0]) / s.Radius, (y - c[1]) / s.Radius, (z1 - c[2]) / s.Radius };
                    interval.NB = new float[] { (x - c[0]) / -s.Radius, (y - c[1]) / -s.Radius, (z2 - c[2]) / -s.Radius };
                    
                    ret_val.Add(interval);
                    return ret_val;
                }
            }

            return null;
        }

        public static TreeOperation ReadFile(string fileName){
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            StreamReader sr = new StreamReader(file);
            string line = sr.ReadLine();

            int sphereNumber = int.Parse(line);

            TreeOperation[] treeOp = new TreeOperation[sphereNumber - 1];
            treeSp = new TreeSphere[sphereNumber];

            for (int i = 0; i < sphereNumber; i++)
            {
                line = sr.ReadLine();
                string[] splitLine = line.Split(' ');
                treeSp[i] = new TreeSphere(new Sphere(new float[] { float.Parse(splitLine[0]), float.Parse(splitLine[1]), float.Parse(splitLine[2]) },
                    float.Parse(splitLine[3]),
                    new int[] { int.Parse(splitLine[4]), int.Parse(splitLine[5]), int.Parse(splitLine[6]) }));
            }

            for (int i = 1; i <= sphereNumber - 1; i++)
                treeOp[i - 1] = new TreeOperation();

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

                //treeOp[i-1] = new TreeOperation(ot);
                treeOp[i - 1].OperationType = ot; 
                //treeOp[i - 1].Left = (int.Parse(splitLine[1]) > 0) ? treeSp[int.Parse(splitLine[1]) - 1] : treeOp[-int.Parse(splitLine[1]) - 1];
                if (int.Parse(splitLine[1]) > 0)
                    treeOp[i-1].Left = treeSp[int.Parse(splitLine[1]) - 1];
                else
                    treeOp[i-1].Left = treeOp[-int.Parse(splitLine[1]) - 1];

                if(int.Parse(splitLine[2]) > 0)
                    treeOp[i-1].Right = treeSp[int.Parse(splitLine[2]) - 1];
                else
                    treeOp[i-1].Right = treeOp[-int.Parse(splitLine[2]) - 1];
            }

            sr.Close();
            file.Close();
            return treeOp[0];
        }
    }
}
