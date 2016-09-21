using System;
using System.Collections.Generic;
using System.Linq;

namespace Delfi.QueryProvider.Tree
{
    /// <summary>
    /// Tree visitor
    /// </summary>
    /// <typeparam name="TX"></typeparam>
    /// <typeparam name="TY"></typeparam>
    /// <param name="txData"></param>
    /// <returns></returns>
    public delegate TY TreeNodeVisitor<in TX, out TY>(TX txData);

    /// <summary>
    /// Tree structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNode<T>
    {
        public T Data { get; }
        public List<TreeNode<T>> Children { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data"></param>
        public TreeNode(T data)
        {
            Data = data;
            Children = new List<TreeNode<T>>();
        }

        /// <summary>
        /// Add new child given node data
        /// </summary>
        /// <param name="childData"></param>
        /// <returns></returns>
        public TreeNode<T> AddChild(T childData)
        {
            Children.Add(new TreeNode<T>(childData));
            return this;
        }

        /// <summary>
        /// Add new child given a node
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public TreeNode<T> AddChild(TreeNode<T> child)
        {
            Children.Add(child);
            return this;
        }

        /// <summary>
        /// Returns granchildren of the current node whose parent is a child with the specified data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<T>> Descend(T data)
        {
            return Children.Where(c=>c.Data.Equals(data)).SelectMany(child => child.Children);
        }

        /// <summary>
        /// Returns granchildren of the current node whose parent is a child with the specified data type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TreeNode<T>> Descend<TE>()
        {
            return Children.Where(c => c.Data.GetType() == typeof(TE)).SelectMany(child => child.Children);
        }

        /// <summary>
        /// Copy the current tree node and descendents into new instance
        /// </summary>
        /// <returns></returns>
        public TreeNode<T> Copy()
        {
            var node = new TreeNode<T>(Data);

            foreach (var child in Children)
            {
                node.Children.Add(child.Copy());
            }

            return node;
        }

        /// <summary>
        /// Traverse the tree and perform operations recursively
        /// </summary>
        /// <typeparam name="TY"></typeparam>
        /// <param name="nodeVisitor"></param>
        /// <returns>
        /// A new tree node that is, at every node, the result of the visit.
        /// The stopper decides whether the exploration of a branch should be passed
        /// </returns>
        public TreeNode<TY> Traverse<TY>(TreeNodeVisitor<T,TY> nodeVisitor, Func<TreeNode<T>,bool> stopCondition = null)
        {
            var node = new TreeNode<TY>(nodeVisitor(Data));
            if (stopCondition == null)
            {
                stopCondition = d => false;
            }
            foreach (var child in Children)
            {
                if (!stopCondition(child))
                {
                    node.AddChild(child.Traverse(nodeVisitor, stopCondition));
                }
            }

            return node;
        }
    }
}
