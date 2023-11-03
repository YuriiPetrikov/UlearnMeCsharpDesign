using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private BinaryTree<T> left;
        public BinaryTree<T> Left
        {
            get
            {
                if (left == null)
                {
                    left = new BinaryTree<T>();
                }
                return left;
            }
            set { left = value; }
        }
        private BinaryTree<T> right;
        public BinaryTree<T> Right
        {
            get
            {
                if (right == null)
                {
                    right = new BinaryTree<T>();
                }
                return right;
            }
            set { right = value; }
        }
        private BinaryTree<T> parent;
        public BinaryTree<T> Parent { get; set; }

        public T Value { get; set; }

        private bool hasValue;
        public bool HasValue { get; set; }

        public BinaryTree()
        {
            Parent = null;
            hasValue = false;
        }


        public void Add(T value)
        {
            if (Parent == null)
            {
                Parent = new BinaryTree<T>();
                Value = value;
                HasValue = true;
            }
            else
            {
                if (value.CompareTo(Value) == 1)
                {
                    Insert(value, Right, this);
                }
                else
                {
                    Insert(value, Left, this);
                }
            }
        }

        private void Insert(T value, BinaryTree<T> currentNode, BinaryTree<T> parent)
        {
            if (currentNode.Parent == null)
            {
                currentNode.Value = value;
                currentNode.Parent = parent;
                currentNode.HasValue = true;
                return; 
            }

            if (value.CompareTo(currentNode.Value) == 1)
            {
                Insert(value, currentNode.Right, currentNode);
            }
            else 
            {
                Insert(value, currentNode.Left, currentNode);
            }
		}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
		}

        public IEnumerator<T> GetEnumerator()
        {
            return new BinaryTreeEnumerator<T>(this);
        }
    }

    public static class BinaryTree
    {
        public static BinaryTree<int> Create(params int[] items)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>();
            
            for (int i = 0; i < items.Length; i++)
                binaryTree.Add(items[i]);
            
            return binaryTree;
        }
    }

    class BinaryTreeEnumerator<T> : IEnumerator<T> where T : IComparable<T>
    {
        private BinaryTree<T> OriginalTree { get; set; }
        private BinaryTree<T> CurrentNode { get; set; }
        object IEnumerator.Current => Current;
        public T Current => CurrentNode.Value;

        public BinaryTreeEnumerator(BinaryTree<T> node)
        {
            OriginalTree = node;
            CurrentNode = null;
        }

        public bool MoveNext()
        {
            if(OriginalTree != null)
                if (!OriginalTree.HasValue)
                    return false;
           
            if (CurrentNode == null)
                CurrentNode = FindMostLeft(OriginalTree);
            else
            {
                if (CurrentNode.Right.HasValue) 
                    CurrentNode = FindMostLeft(CurrentNode.Right);
                else
                {
                    T CurrentValue = CurrentNode.Value;

                    while (CurrentNode != null)
                    {
                        CurrentNode = CurrentNode.Parent;
                        if (CurrentNode != null)
                        {
                            int Compare = Current.CompareTo(CurrentValue);
                            if (Compare < 0) continue;
                        }
                        break;
                    }
                }
            }
            return CurrentNode != null;
        }

        public void Reset() { CurrentNode = new BinaryTree<T>(); }
        public void Dispose() { }
        BinaryTree<T> FindMostLeft(BinaryTree<T> start)
        {
            BinaryTree<T> node = start;
            while (true)
            {
                if (node.Left.HasValue) 
                {
                    node = node.Left;
                    continue;
                }
                break;
            }
            return node;
        }
    }
}
