using Delfi.QueryProvider.Attributes;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.StandardNamespaces;
using Delfi.QueryProvider.Tree;
using Sparql.Algebra.Maps;
using System.Collections.Generic;
using System.Reflection;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Represents a graph expression to be evaluated by a GraphProvider
    /// </summary>
    public class GraphExpression
    {
        public IMap Map { get; private set; }

        public TreeNode<Term> Header { get; private set; }

        public static GraphExpression Empty { get { return new GraphExpression(); } }

        #region Life Cycle
        /// <summary>
        /// Public constructor
        /// </summary>
        /// <param name="root">root term of the new expression</param>
        public GraphExpression(Term root)
        {
            Header = new TreeNode<Term>(root);
            Map = new EmptyMap();

            var variable = root as Variable;

            if (variable != null)
            {              
                Map = AddVariable(variable, Header, Map);
            }          
        }

        /// <summary>
        /// Constructor for internal usage
        /// </summary>
        private GraphExpression()
        {
            Map = new EmptyMap();
        }

        /// <summary>
        /// Constructor for internal usage
        /// </summary>
        /// <param name="map"></param>
        /// <param name="header"></param>
        private GraphExpression(IMap map, TreeNode<Term> header)
        {
            Map = map;
            Header = header;
        }
        #endregion

        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public GraphExpression LeftJoin(Resource property, Term @object)
        {
            var result = CreateJoinedMap(property, @object);
            return new GraphExpression(new LeftJoinMap(Map, result.Map), result.Node);
        }
                
        /// <summary>
        /// Join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public GraphExpression Join(Resource property, Term @object)
        {
            var result = CreateJoinedMap(property, @object);
            return new GraphExpression(new JoinMap(Map, result.Map), result.Node);
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="property"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public GraphExpression Select(Resource property, Term @object)
        {
            var result = CreateJoinedMap(property, @object);
            return new GraphExpression(new JoinMap(Map, result.Map), new TreeNode<Term>(@object));
        }

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="graphExpression"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperation"></exception>
        public GraphExpression Union(GraphExpression graphExpression)
        {
            return new GraphExpression(new UnionMap(Map, graphExpression.Map), Header.Copy());
        }

        /// <summary>
        /// Slice
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GraphExpression Slice(int? offset, int? size)
        {
            return new GraphExpression(new SliceMap(Map, offset, size), Header.Copy());
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="statementList"></param>
        /// <returns></returns>
        public GraphExpression Join(IEnumerable<Statement> statementList)
        {
            var header = Header.Copy();
            var map = new JoinMap(Map, new BGPMap(statementList));

            return new GraphExpression(map, header);
        }

        #region Private Methods

        // Adds a variable to the graph expression and adds statements for related properties using reflection
        private IMap AddVariable(Variable variable, TreeNode<Term> node, IMap map)
        {
            if (variable.ResourceType != null)
            {
                var statement = new Statement(node.Data, new Rdf("type"), variable.ResourceType);
                map = new JoinMap(map, new BGPMap(statement));
            }         

            foreach (var prop in variable.Type.GetRuntimeProperties())
            {
                PropertyBindAttribute attr;
                if ((attr = prop.GetCustomAttribute<PropertyBindAttribute>()) != null)
                {
                    var varData = new Variable(prop.PropertyType, variable.Id + "·" + prop.Name);
                    var varNode = new TreeNode<Term>(varData);

                    if (prop.GetCustomAttribute<RequiredAttribute>() != null)
                    {
                        map = new JoinMap(map, AddVariable(varData, varNode, new BGPMap(new Statement(node.Data, attr.Property, varData))));
                    }
                    else
                    {
                        map = new LeftJoinMap(map, AddVariable(varData, varNode, new BGPMap(new Statement(node.Data, attr.Property, varData))));
                    }

                    var newChild = new TreeNode<Term>(attr.Property).AddChild(varNode);
                    node.AddChild(newChild);
                }
            }

            return map;
        }

        // Helper method to compute expression joins
        private JoinedMapResult CreateJoinedMap(Resource property, Term @object)
        {
            @object = @object ?? new Variable();
            var objectNode = new TreeNode<Term>(@object);
            var propertyNode = new TreeNode<Term>(property).AddChild(objectNode);

            var header = Header.Copy().AddChild(propertyNode);
            IMap joinedMap = new BGPMap(new Statement(Header.Data, property, @object));

            var variable = @object as Variable;
            if (variable != null)
            {
                joinedMap = AddVariable(variable, objectNode, joinedMap);
            }

            return new JoinedMapResult { Map = joinedMap, Node = header };
        }

        #endregion

        /// Result class for internal usage
        private class JoinedMapResult
        {
            public IMap Map { get; set; }

            public TreeNode<Term> Node { get; set; }
        }
    }
}
