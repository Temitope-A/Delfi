using Delfi.QueryProvider.RDF;
using Sparql.Algebra;
using Sparql.Algebra.Maps;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Collections.Generic;

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
        /// Public constructor with constant initializer
        /// </summary>
        public GraphExpression(Resource root)
        {
            Root = root;
            Map = new ConstantMap(new LabelledTreeNode<object, Term>(root));
        }

        /// <summary>
        /// Public constructor with query model initializer
        /// </summary>
        public GraphExpression(LabelledTreeNode<object, Term> queryModel)
        {
            Root = (Term)queryModel.Value;
            Map = new BgpMap(queryModel);
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
            var result = CreateJoinedMap(property, expression);
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
            var result = CreateJoinedMap(property, expression);
            return new GraphExpression(new JoinMap(Map, result.Map, result.AddressPairList));
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
            return new GraphExpression(new SelectMap(Map, result.Map, result.AddressPairList));
        }

        /// <summary>
        /// Select
        /// </summary>
        /// <param name="property"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public GraphExpression Select(Resource property, GraphExpression expression)
        {
            var result = CreateJoinedMap(property, expression);
            return new GraphExpression(new SelectMap(Map, result.Map, result.AddressPairList));
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

        // Helper method to compute expression joins
        private JoinedMapResult CreateJoinedMap(Resource property, Term @object)
        {
            @object = @object ?? new Variable();
            var joinSegment = new LabelledTreeNode<object, Term>(Root).AddChild(property, @object);
            IMap joinedMap = new BgpMap(joinSegment);

            var comparisonSites = new List<JoinAddressPair> { JoinAddressPair.RootComparison };

            return new JoinedMapResult { Map = joinedMap, AddressPairList = comparisonSites};
        }

        // Helper method to compute expression joins
        private JoinedMapResult CreateJoinedMap(Resource property, GraphExpression expression)
        {
            var joinSegment = new LabelledTreeNode<object, Term>(Root).AddChild(property, expression.Root);
            var joineMapComparisonSites = new List<JoinAddressPair> { new JoinAddressPair { TreeAddress1 = new List<Term> { property }, TreeAddress2 = new List<Term>() } };
            IMap joinedMap = new JoinMap(new BgpMap(joinSegment), expression.Map,  joineMapComparisonSites);

            var comparisonSites = new List<JoinAddressPair> { JoinAddressPair.RootComparison };

            return new JoinedMapResult { Map = joinedMap, AddressPairList = comparisonSites };
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
