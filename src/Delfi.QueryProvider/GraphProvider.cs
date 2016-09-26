using Delfi.QueryProvider.Attributes;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.Tree;
using Sparql.Algebra.Evaluators;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.Maps;
using Sparql.Algebra.Rows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Delfi.QueryProvider
{
    public class GraphProvider<T>:IGraphProvider where T:IEvaluator
    {
        /// <summary>
        /// Map evaluator
        /// </summary>
        public IGraphSource DefaultSource { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="evaluator"></param>
        public GraphProvider(IGraphSource source)
        {
            DefaultSource = source;
        }

        /// <summary>
        /// Executes the query codified in graphExpression
        /// </summary>
        /// <param name="graphExpression"></param>
        /// <returns></returns>
        public IEnumerable<TreeNode<object>> Execute(GraphExpression graphExpression, IGraphSource source = null)
        {
            var headers = FlattenGraph(graphExpression.Header);
            var resultSet = new ProjectMap(graphExpression.Map.Reduce(), headers);

            foreach (var row in resultSet.Evaluate<T>( source ?? DefaultSource).Skip(1))
            {
                yield return MakeGraph((ResultRow)row, graphExpression.Header);
            }
        }

        // Project the content of the result row onto the header graph
        private TreeNode<object> MakeGraph(ResultRow row, TreeNode<Term> header)
        {
            TreeNodeVisitor<Term, object> visitor = node => ResolveNode(row.SolutionMapping, node);

            Func<TreeNode<Term>, bool> stopCondition = t => t.Children.Any(c => c.Data is Variable && ((Variable)c.Data).IsResourceInternal);

            return header.Traverse(visitor, stopCondition);
        }

        // Flatten the header graph into an array of variables to be projected atthe end of the map evaluation
        private string[] FlattenGraph(TreeNode<Term> header)
        {
            var result = new List<string>();

            TreeNodeVisitor<Term, bool> visitor = node => IncludeNodeIfVariable(result, node);

            header.Traverse(visitor);

            return result.ToArray();
        }

        private bool IncludeNodeIfVariable(List<string> list, Term node)
        {
            if (node.GetType() == typeof(Variable))
            {
                list.Add(node.Id);
                return true;
            }

            return false;
        }

        private object ResolveNode(Dictionary<string, object> solutionMapping, Term nodeData)
        {
            var variable = nodeData as Variable;

            if (variable == null)
            {
                return (Resource)nodeData;
            }
            else
            {
                return CreateResourceInstance(variable.Type, solutionMapping, nodeData.Id);
            }
        }

        private object CreateResourceInstance(Type type, Dictionary<string, object> solutionMapping, string propertyPath)
        {
            var match = solutionMapping[propertyPath];

            if (match == null)
            {
                return null;
            }

            if (type != null)
            {
                if (type.GetTypeInfo().IsValueType)
                {
                    return match;
                }

                var result = Activator.CreateInstance(type, new[] { match.ToString() });

                foreach (var prop in result.GetType().GetRuntimeProperties())
                {
                    PropertyBindAttribute attr;
                    if ((attr = prop.GetCustomAttribute<PropertyBindAttribute>()) != null)
                    {  
                        var propertyValue = CreateResourceInstance(prop.PropertyType, solutionMapping, propertyPath + "·" + prop.Name);
                        prop.SetValue(result, propertyValue);
                    }
                }

                return result;
            }

            return new Resource(match.ToString());
        }
    }
}
