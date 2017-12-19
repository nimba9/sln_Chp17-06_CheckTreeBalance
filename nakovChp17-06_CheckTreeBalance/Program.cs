using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheckTreeBalance
{
    public class BiTrNode
    {
        private int value;
        private BiTrNode leftChild;
        private BiTrNode rightChild;
        public bool hasParent;
        
        public BiTrNode(int value, BiTrNode leftChild, BiTrNode rightChild)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot insert null value!");
            }

            this.value = value;
            this.LeftChild = leftChild;
        }



        public BiTrNode(int value): this (value, null, null)
		{
        }

        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
            }
        }

        public BiTrNode LeftChild
        {
            get
            {
                return this.leftChild;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                if (value.hasParent)
                {
                    throw new ArgumentException("The node already has a parent!");
                }

                value.hasParent = true;
                this.leftChild = value;
            }
        }

        public BiTrNode RightChild
        {
            get
            {
                return this.rightChild;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                if (value.hasParent)
                {
                    throw new ArgumentException("The node already has a parent!");
                }

                value.hasParent = true;
                this.rightChild = value;
            }
        }
    }

    public class BiTree
    {
        private BiTrNode root;
        public BiTree(int value, BiTree leftChild, BiTree rightChild)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot insert null value!");
            }

            BiTrNode leftChildNode = leftChild != null ? leftChild.root : null;
            BiTrNode rightChildNode = rightChild != null ? rightChild.root : null;
            this.root = new BiTrNode(value, leftChildNode, rightChildNode);
        }

        public BiTree(int value): this (value, null, null)
        {
        }

        public BiTree(BiTrNode value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Cannot insert null value!");
            }

            this.root = value;
        }

        public BiTrNode Root
        {
            get { return this.root; }
        }

        private int CountNodesDFS(BiTrNode root)
        {
            int nodesCount = 1;
            Stack<BiTrNode> presentNodes = new Stack<BiTrNode>();
            presentNodes.Push(root);

            while (presentNodes.Count > 0)
            {
                BiTrNode currentNode = presentNodes.Pop();
                if (currentNode.LeftChild != null)
                {
                    presentNodes.Push(currentNode.LeftChild);
                    nodesCount++;
                }

                if (currentNode.RightChild != null)
                {
                    presentNodes.Push(currentNode.RightChild);
                    nodesCount++;
                }
            }

            return nodesCount;
        }

        public bool IsIdeallyBalanced()
        {
            return IsIdeallyBalanced(this.root);
        }

        private bool IsIdeallyBalanced(BiTrNode root)
        {
            long leftTrNodeCount = 0;
            long rightTrNodeCount = 0;

            if (root == null)
            {
                return true;
            }

            if (root.LeftChild != null)
            {
                leftTrNodeCount = CountNodesDFS(root.LeftChild);
            }

            if (root.RightChild != null)
            {
                rightTrNodeCount = CountNodesDFS(root.RightChild);
            }

            if (Math.Abs(leftTrNodeCount - rightTrNodeCount) <= 1
               && IsIdeallyBalanced(root.LeftChild) == true && IsIdeallyBalanced(root.RightChild) == true)
           {
                return true;
           }

            return false;
        }
    }

    public class CheckBalance
    {
        private static List<string> GetChildren(string successors)
        {
            List<string> children = new List<string>();
            int openBrackets = 0;
            StringBuilder currChildTree = new StringBuilder();

            for (int i = 0; i < successors.Length; i++)
            {
                if (openBrackets == 0 && successors[i] != '(')
                {
                    StringBuilder currLeaf = new StringBuilder();
                    while (i < successors.Length && (Char.IsDigit(successors[i]) || successors[i] == 'x'))
                    {
                        currLeaf.Append(successors[i]);
                        i++;
                    }
                    children.Add(currLeaf.ToString());
                    continue;
                }

                if (successors[i] ==')')
                {
                    openBrackets--;
                }

                else if (successors[i] == '(')
                {
                    openBrackets++;
                }

                if (openBrackets == 0)
                {
                    currChildTree.Append(')');
                    children.Add(currChildTree.ToString());
                    i++;
                }

                else
                { currChildTree.Append(successors[i]); }
            }
            return children;
        }

        public static BiTrNode ParseTree(string tree)
        {
            if (tree.Contains("->") == false)
            {
                int currValue = int.Parse(tree);
                return new BiTrNode(currValue);
            }

            string cleanTree = tree.Substring(1, tree.Length - 2);
            string[] currentNodes = cleanTree.Split(new string[] { "->" }, 2, StringSplitOptions.RemoveEmptyEntries);
            BiTrNode presentNode = new BiTrNode(int.Parse(currentNodes[0]));

            if (currentNodes[1].Contains("->") == true)
            {
                List<string> children = GetChildren(currentNodes[1]);

                if (children[0] != "x")
                {
                    presentNode.LeftChild = ParseTree(children[0]);
                }

                if (children[1] != "x")
                {
                    presentNode.RightChild = ParseTree(children[1]);
                }
            }

            else
            {
                string[] leafs = currentNodes[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (leafs[0] != "x")
                {
                    int leafValue = int.Parse(leafs[0]);
                    presentNode.LeftChild = new BiTrNode(leafValue);
                }

                if (leafs[1] != "x")
                {
                    int leafValue = int.Parse(leafs[0]);
                    presentNode.RightChild = new BiTrNode(leafValue);
                }
            }

            return presentNode;
        }

        public static void Main(string[] args)
        {
            string rawTree = Console.ReadLine();
            BiTrNode root = ParseTree(rawTree);
            BiTree tree = new BiTree(root);
            Console.WriteLine(tree.IsIdeallyBalanced());
        }
    }

    

    



}

