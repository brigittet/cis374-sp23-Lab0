using System;

namespace Lab0
{
    public class BinarySearchTree<T> : IBinarySearchTree<T>
    {

        private BinarySearchTreeNode<T> Root { get; set; }

        public BinarySearchTree()
        {
            Root = null;
            Count = 0;
        }

        public bool IsEmpty => Root == null;

        public int Count { get; private set; }

        // TODO
        public int Height => IsEmpty ? 0 : HeightRecursive(Root);

        private int HeightRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return -1;
            }

            //if (node.Left == null && node.Right == null)
            //{
            //    return 0;
            //}

            int leftHeight = HeightRecursive(node.Left);
            int rightHeight = HeightRecursive(node.Right);

            return 1 + Math.Max(leftHeight, rightHeight);
        }

        // TODO
        public int? MinKey => MinKeyRecursive(Root);

        private int? MinKeyRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else if (node.Left == null)
            {
                return node.Key;
            }
            else
            {
                return MinKeyRecursive(node.Left);
            }

        }

        // TODO
        public int? MaxKey => MaxKeyRecursive(Root);

        private int? MaxKeyRecursive(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }
            else if (node.Right == null)
            {
                return node.Key;
            }
            else
            {
                return MinKeyRecursive(node.Right);
            }
        }


        // TODO
        public double MedianKey => InOrderKeys.Count % 2 == 0 ? ((double)(InOrderKeys[InOrderKeys.Count / 2] + InOrderKeys[InOrderKeys.Count / 2 - 1]) / 2) : InOrderKeys[InOrderKeys.Count / 2];


        public BinarySearchTreeNode<T> GetNode(int key)
        {
            return GetNodeRecursive(Root, key);
        }

        private BinarySearchTreeNode<T>? GetNodeRecursive(BinarySearchTreeNode<T> node, int key)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Key == key)
            {
                return node;
            }
            else if (key < node.Key)
            {
                return GetNodeRecursive(node.Left, key);
            }
            else
            {
                return GetNodeRecursive(node.Right, key);
            }
        }


        // TODO
        public void Add(int key, T value)
        {
            if (Root == null)
            {
                Root = new BinarySearchTreeNode<T>(key, value);
                Count++;
            }
            else
            {
                AddRecursive(key, value, Root);
            }
        }
        // TODO
        private void AddRecursive(int key, T value, BinarySearchTreeNode<T> parent)
        {
            // duplicate found
            // do not add to BST
            if (key == parent.Key)
            {
                return;
            }

            if (key < parent.Key)
            {
                if (parent.Left == null)
                {
                    var newNode = new BinarySearchTreeNode<T>(key, value); ;
                    parent.Left = newNode;
                    newNode.Parent = parent;
                    Count++;

                }
                else
                {
                    AddRecursive(key, value, parent.Left);
                }
            }
            else
            {
                if (parent.Right == null)
                {
                    var newNode = new BinarySearchTreeNode<T>(key, value);
                    parent.Right = newNode;
                    newNode.Parent = parent;
                    Count++;
                }
                else
                {
                    AddRecursive(key, value, parent.Right);
                }
            }
        }

        // TODO
        public void Clear()
        {
            Root = null;
        }

        public bool Contains(int key)
        {
            if (GetNode(key) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // TODO
        public BinarySearchTreeNode<T> Next(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Right == null)
            {

                if (node.Parent != null)
                {
                    var parentNode = node.Parent;
                    if (node == parentNode.Left)
                    {
                        return parentNode;
                    }
                    else
                    {
                        while ((parentNode != parentNode.Parent.Left) && (parentNode != Root))
                        {
                            parentNode = parentNode.Parent;
                        }
                        if (parentNode == parentNode.Parent.Left)
                        {
                            return parentNode.Parent;
                        }
                    }

                }
                return null;
            }

            var currentNode = node.Right;

            while (currentNode.Left != null)
            {
                currentNode = currentNode.Left;
            }

            return currentNode;


        }

        // TODO
        public BinarySearchTreeNode<T> Prev(BinarySearchTreeNode<T> node)
        {
            if (node == null)
            {
                return null;
            }

            if (node.Left == null)
            {
                if (node.Parent != null)
                {
                    var parentNode = node.Parent;
                    if (node == parentNode.Right)
                    {
                        return parentNode;
                    }
                }
                return null;
            }

            var currentNode = node.Left;

            while (currentNode.Right != null)
            {
                currentNode = currentNode.Right;
            }

            return currentNode;
        }

        // TODO
        public List<BinarySearchTreeNode<T>> RangeSearch(int min, int max)
        {
            var rangeList = new List<BinarySearchTreeNode<T>>();
            foreach (var key in InOrderKeys)
            {
                if (key >= min && key <= max)
                {
                    rangeList.Add(GetNode(key));
                }
                if (key > max)
                {
                    break;
                }
            }
            return rangeList;
        }

        public void Remove(int key)
        {
            var node = GetNode(key);

            if (node == null)
            {
                return;
            }

            Count--;

            if (node != Root)
            {
                var parent = node.Parent;

                // 1) leaf node
                if (node.Left == null && node.Right == null)
                {
                    if (parent.Left == node)
                    {
                        parent.Left = null;
                        node.Parent = null;
                    }
                    else if (parent.Right == node)
                    {
                        parent.Right = null;
                        node.Parent = null;
                    }

                    return;
                }

                // 2) parent with 1 child
                if (node.Left == null && node.Right != null)
                {
                    // only has a right child
                    var child = node.Right;
                    if (parent.Left == node)
                    {
                        parent.Left = child;
                        child.Parent = parent;
                    }
                    else if (parent.Right == node)
                    {
                        parent.Right = child;
                        child.Parent = parent;
                    }

                    return;
                }

                if (node.Left != null && node.Right == null)
                {
                    // only has a left child
                    var child = node.Left;
                    if (parent.Left == node)
                    {
                        parent.Left = child;
                        child.Parent = parent;

                        node.Parent = null;
                        node.Left = null;
                    }
                    else if (parent.Right == node)
                    {
                        parent.Right = child;
                        child.Parent = parent;

                        node.Parent = null;
                        node.Right = null;
                    }

                    return;
                }
            }

            // 3) parent with 2 children
            // Find the node to remove
            // Find the next node (successor)
            // Swap Key and Data from successor to node
            // Remove the successor (a leaf node) (like case 1)

            if (Next(node) == null)
            {
                Clear();
            }
            var currentNode = Next(node);

            var dupeKey = node.Key;
            var dupeVal = node.Value;
            node.Key = currentNode.Key;
            node.Value = currentNode.Value;
            currentNode.Key = dupeKey;
            currentNode.Value = dupeVal;

            Remove(currentNode.Key);
            return;

        }

        // TODO
        public T Search(int key)
        {
            if (Contains(key))
            {
                return GetNode(key).Value;
            }
            else
            {
                return default(T);
            }
        }

        // TODO
        public void Update(int key, T value)
        {
            var newNode = GetNode(key);
            newNode.Value = value;
        }


        // TODO
        public List<int> InOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                InOrderKeysRecursive(Root, keys);

                return keys;

            }
        }

        private void InOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            // left
            // root
            // right

            if (node == null)
            {
                return;
            }

            InOrderKeysRecursive(node.Left, keys);
            keys.Add(node.Key);
            InOrderKeysRecursive(node.Right, keys);

        }

        // TODO
        public List<int> PreOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                PreOrderKeysRecursive(Root, keys);

                return keys;
            }
        }

        private void PreOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            if (node == null)
            {
                return;
            }

            keys.Add(node.Key);
            PreOrderKeysRecursive(node.Left, keys);
            PreOrderKeysRecursive(node.Right, keys);
        }

        // TODO
        public List<int> PostOrderKeys
        {
            get
            {
                List<int> keys = new List<int>();
                PostOrderKeysRecursive(Root, keys);
                return keys;
            }
        }

        Tuple<int, T> IBinarySearchTree<T>.Min
        {
            get
            {
                if (IsEmpty)
                {
                    return null;
                }
                var minNode = MinNode(Root);
                return Tuple.Create(minNode.Key, minNode.Value);
            }
        }

        Tuple<int, T> IBinarySearchTree<T>.Max
        {
            get
            {
                if (IsEmpty)
                {
                    return null;
                }
                var maxNode = MaxNode(Root);
                return Tuple.Create(maxNode.Key, maxNode.Value);
            }
        }

        private void PostOrderKeysRecursive(BinarySearchTreeNode<T> node, List<int> keys)
        {
            if (node == null)
            {
                return;
            }

            PostOrderKeysRecursive(node.Left, keys);
            PostOrderKeysRecursive(node.Right, keys);
            keys.Add(node.Key);
        }

        public BinarySearchTreeNode<T> MinNode(BinarySearchTreeNode<T> node)
        {
            return MinNodeRecursive(node);
        }

        private BinarySearchTreeNode<T> MinNodeRecursive(BinarySearchTreeNode<T> node)
        {
            if (node.Left == null)
            {
                return node;
            }

            return MinNodeRecursive(node.Left);
        }

        public BinarySearchTreeNode<T> MaxNode(BinarySearchTreeNode<T> node)
        {
            return MaxNodeRecursive(node);
        }

        private BinarySearchTreeNode<T> MaxNodeRecursive(BinarySearchTreeNode<T> node)
        {
            if (node.Right == null)
            {
                return node;
            }

            return MaxNodeRecursive(node.Right);
        }


    }
}

