using System.Collections.Generic;
using System;

namespace Csg
{
    public class TreeOperation : TreeNode
    {
        public OperationType OperationType { get; set; }


        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeOperation(OperationType operationType, TreeNode left, TreeNode right)
        {
            if (left == null || right == null)
            {
                throw new ArgumentException("Both left and right operands must not be null.");
            }

            OperationType = operationType;
            Left = left;
            Right = right;
        }

        public override System.Collections.Generic.List<Interval> TraverseTree(float x, float y)
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
                default:
                    return Interval.Intersection(left, right); //just to make compiler happy...
            }
        }

        public override bool FindRect(out float x0, out float y0, out float x1, out float y1)
        {
            //Assert (Left != null && Right != null)
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
    }
}
