namespace Csg
{
    class TreeSphere : TreeNode
    {
        private Sphere _s;
        
        public Sphere S
        {
            get { return _s; }
        }

        public TreeSphere(Sphere s)
        {
            _s = s;
        }
    }
}
