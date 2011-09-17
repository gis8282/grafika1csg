using System;
using System.Collections.Generic;
using System.Text;

using System.IO;


namespace Csg
{
    public abstract class TreeNode
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

        public abstract List<Interval> TraverseTree(float x, float y);
    }
}
