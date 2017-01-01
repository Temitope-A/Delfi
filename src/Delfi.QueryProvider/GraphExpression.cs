using Delfi.QueryProvider.Attributes;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra;
using Sparql.Algebra.Maps;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Represents a graph expression to be evaluated by a GraphProvider
    /// </summary>
    public class GraphExpression
    {
        /// <summary>
        /// Root of this graph expression
        /// </summary>
        public Term Root { get; private set; }

        /// <summary>
        /// Sparql algebra map object
        /// </summary>
        public IMap Map { get; private set; }

        /// <summary>
        /// Empty instance of a graph expression
        /// </summary>
        public static GraphExpression Empty { get { return new GraphExpression(); } }

        #region Life Cycle
        /// <summary>
        /// Public constructor
        /// </summary>
        public GraphExpression(Term root)
        {
            Root = root;
            var variable = root as Variable;

            if (variable != null)
            {              
                Map = FillObjectGraph(variable, Map);
            }
            else
            {
                Map = new ConstantMap(new LabelledTreeNode<object, Term>(root));
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
        private GraphExpression(IMap map)
        {
            Map = map;
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
            return new GraphExpression(new LeftJoinMap(Map, result.Map, result.AddressPairList));
        }

        /// <summary>
        /// Left Join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GraphExpression LeftJoin(Resource property, GraphExpression expression)
        {
            var result = CreateJoinedMap(property, @object);
            return new GraphExpression(new LeftJoinMap(Map, result.Map, result.AddressPairList));
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
            return new GraphExpression(new JoinMap(Map, result.Map, result.AddressPairList));
        }

        /// <summary>
        /// Join
        /// </summary>
        /// <param name="property"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GraphExpression Join(Resource property, GraphExpression expression)
        {
            var result = CreateJoinedMap(property, @object);
            return new GraphExpression(new JoinMap(Map, result.Map, result.AddressPairList));
        }

        /// <summary>
        /// Union
        /// </summary>
        /// <param name="graphExpression"></param>
        /// <returns></returns>
        public GraphExpression Union(GraphExpression graphExpression)
        {
            return new GraphExpression(new UnionMap(Map, graphExpression.Map));
        }

        /// <summary>
        /// Slice
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public GraphExpression Slice(int? offset, int? size)
        {
            return new GraphExpression(new SliceMap(Map, offset, size));
        }

        #region Private Methods

        private IMap FillObjectGraph(Variable variable, IMap map)
        {
            if (variable.ResourceType != null)
            {
                
            }

            var bindableProperties = type.GetRuntimeProperties().Where(p => p.GetCustomAttribute<PropertyBindAttribute>() != null);

            //List<PropertyInfo> requiredProperties = new List<PropertyInfo>();
            //List<PropertyInfo> optionalProperties = new List<PropertyInfo>();

            foreach (var prop in bindableProperties)
            {
                var predicate = prop.GetCustomAttribute<PropertyBindAttribute>().Property;
                var @object = new Variable(prop.PropertyType, node.Data.Id + "·" + prop.Name);

                var comparisonSites = new List<JoinAddressPair>
                {
                    new JoinAddressPair {TreeAddress1 = new List<Term>(), TreeAddress2 = new List<Term>()}
                };

                if (prop.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    map = new LeftJoinMap(map, new BgpMap(new Statement(node.Data, predicate, @object).ToGraph(), 0, 1), comparisonSites);
                }
                else
                {
                    map = new JoinMap(map, new BgpMap(new Statement(node.Data, predicate, @object).ToGraph(), 0, 1), comparisonSites);
                }
            }

            return map;
        }

        

        // Helper method to compute expression joins
        private JoinedMapResult CreateJoinedMap(Resource property, Term @object)
        {
            @object = @object ?? new Variable();
            var joinSegment = new LabelledTreeNode<object, Term>(Root).AddChild(property, @object);
            IMap joinedMap = new BgpMap(joinSegment);

            var variable = @object as Variable;
            if (variable != null)
            {
                joinedMap = FillObjectGraph(variable, joinedMap);
            }

            var comparisonSites = new List<JoinAddressPair>
            {
                new JoinAddressPair {TreeAddress1 = new List<Term>(), TreeAddress2 = new List<Term>()}
            };
            return new JoinedMapResult { Map = joinedMap, AddressPairList = comparisonSites};
        }

        #endregion

        /// Result class for internal usage
        private class JoinedMapResult
        {
            public IMap Map { get; set; }

            public List<JoinAddressPair> AddressPairList { get; set; }
        }
    }
}
