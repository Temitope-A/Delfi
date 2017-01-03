using Delfi.QueryProvider;
using Delfi.QueryProvider.Evaluators;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.StandardNamespaces;
using Sparql.Algebra.GraphSources;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System.Diagnostics;
using Xunit;

namespace GraphRepository.Test.QueryProvider
{
    public class EndToEndTest
    {
        [Fact]
        public void PropertyQuery()
        {
            var graphSource = new GraphSource("http://localhost:7200/repositories/Pets");
            var graphProvider = new GraphProvider<SparqlBgpEvaluator>(graphSource);

            var propertyGraph = (new LabelledTreeNode<object, Term>(new Variable())).AddChild(new Rdf("type"), new Rdf("Property"));
            var query = new QueryableGraph(graphProvider, new GraphExpression(propertyGraph));

            int count = 0;
            foreach (var item in query) {
                count++;
            }
            Assert.Equal(79, count);
        }

        [Fact]
        public void ClassQuery()
        {
            var graphSource = new GraphSource("http://localhost:7200/repositories/Pets");
            var graphProvider = new GraphProvider<SparqlBgpEvaluator>(graphSource);

            var classGraph = (new LabelledTreeNode<object, Term>(new Variable())).AddChild(new Rdfs("subClassOf"), new Variable());
            var query = new QueryableGraph(graphProvider, new GraphExpression(classGraph));

            int count = 0;
            foreach (var item in query) {
                count++;
            }
            Assert.Equal(291, count);
        }

        [Fact]
        public void JoinTest()
        {
            var graphSource = new GraphSource("http://localhost:7200/repositories/Pets");
            var graphProvider = new GraphProvider<SparqlBgpEvaluator>(graphSource);

            var propertyGraph = (new LabelledTreeNode<object, Term>(new Variable()))
                .AddChild(new Rdf("type"), new Rdf("Property"));
            var propertyQuery = new QueryableGraph(graphProvider, new GraphExpression(propertyGraph));

            var classGraph = (new LabelledTreeNode<object, Term>(new Variable()))
                .AddChild(new Rdfs("subClassOf"), new Variable());
            var classQuery = new QueryableGraph(graphProvider, new GraphExpression(classGraph));

            var query = propertyQuery.Require(new Rdfs("range"), classQuery);

            int count = 0;
            foreach (var item in query)
            {
                count++;
                Debug.WriteLine(item);
            }
            Assert.Equal(76, count);
        }

        [Fact]
        public void LeftJoinTest()
        {
            var graphSource = new GraphSource("http://localhost:7200/repositories/Pets");
            var graphProvider = new GraphProvider<SparqlBgpEvaluator>(graphSource);

            var propertyGraph = (new LabelledTreeNode<object, Term>(new Variable()))
                .AddChild(new Rdf("type"), new Rdf("Property"));
            var propertyQuery = new QueryableGraph(graphProvider, new GraphExpression(propertyGraph));

            var classGraph = (new LabelledTreeNode<object, Term>(new Variable()))
                .AddChild(new Rdfs("subClassOf"), new Variable());
            var classQuery = new QueryableGraph(graphProvider, new GraphExpression(classGraph));

            var query = propertyQuery.Expand(new Rdfs("range"), classQuery);

            int count = 0;
            foreach (var item in query)
            {
                count++;
                Debug.WriteLine(item);
            }
            Assert.Equal(79, count);
        }
    }
}
