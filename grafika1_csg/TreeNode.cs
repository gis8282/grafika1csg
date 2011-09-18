using System.Collections.Generic;

namespace Csg
{
    public abstract class TreeNode
    {
        public static TreeSphere[] AllTreeSpheres = new TreeSphere[0];

        public abstract List<Interval> TraverseTree(float x, float y);

        public abstract bool FindRect(out float x0, out float y0, out float x1, out float y1);
    }
}
