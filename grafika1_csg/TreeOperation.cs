namespace Csg
{
    class TreeOperation : TreeNode
    {
        public OperationType OperationType { get; set; }
        public TreeOperation() { }
        public TreeOperation(OperationType operationType)
        {
            OperationType = operationType;
        }
    }
}
