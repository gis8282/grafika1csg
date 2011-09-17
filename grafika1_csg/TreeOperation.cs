using System.Collections.Generic;

namespace Csg
{
    public class TreeOperation : TreeNode
    {
        public OperationType OperationType { get; set; }
        public TreeOperation() { }
        public TreeOperation(OperationType operationType)
        {
            OperationType = operationType;
        }

        public override System.Collections.Generic.List<Interval> TraverseTree(float x, float y)
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
            return null;
        }
    }
}
