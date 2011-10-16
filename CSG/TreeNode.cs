using System.Collections.Generic;
using System.Linq;

namespace Csg
{
    public abstract class TreeNode
    {
        public IEnumerable<Sphere> GetAllSpheres()
        {
            var treeSphere = this as TreeSphere;

            if (treeSphere != null)
            {
                yield return treeSphere.S;
            }
            else
            {
                var treeOperation = this as TreeOperation;

                foreach(var sphere in treeOperation.Left.GetAllSpheres().Concat(treeOperation.Right.GetAllSpheres())) {
                    yield return sphere;
                }
            }
        }

        public abstract List<Interval> TraverseTree(float x, float y);

        public abstract bool FindRect(out float x0, out float y0, out float x1, out float y1);
    }
}
