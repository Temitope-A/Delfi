using Delfi.EntityFramework;
using Delfi.EntityFramework.Attributes;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.StandardNamespaces;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using Xunit;

namespace GraphRepository.Test.EntityFramework
{
    public class EndToEndTest
    {
        [Fact]
        public void ConnectedQuery()
        {
            IGraphContext context = new GraphContext();

            var query = context.Select<RdfProperty>().Require<RdfClass>(new Rdfs("range"));

            int i = 0;
            foreach (var item in query)
            {
                i++;
            }

            Assert.Equal(76, i);
        }

        [Fact]
        public void EmbeddedQuery()
        {
            IGraphContext context = new GraphContext();

            var query = context.Select<RdfPropertyWithRange>();

            int i = 0;
            foreach (var item in query)
            {
                i++;
            }

            Assert.Equal(181, i);
        }

        [Fact]
        public void InsertionAndDeletion()
        {
            IGraphContext context = new GraphContext();
            var query = context.Select<RdfPropertyWithRange>();

            int i = 0;
            foreach (var item in query)
            {
                i++;
            }

            Assert.Equal(181, i);

            var prop = new RdfPropertyWithRange("http://example.org/exampleprop");
            prop.Range = new RdfClass("http://example.org/exampleclass");

            context.Add(prop);

            i = 0;
            foreach (var item in query)
            {
                i++;
            }

            Assert.Equal(183, i);

            var graph = new LabelledTreeNode<object, Term>(prop);
            graph.AddChild(new Rdfs("range"), new RdfClass("http://example.org/exampleclass"));

            context.Remove(graph);

            i = 0;
            foreach (var item in query)
            {
                i++;
            }

            Assert.Equal(181, i);
        }


    }

    [EntityBind("rdf", "Property")]
    public class RdfProperty:Resource
    {
        public RdfProperty(string iri) : base(iri) { }
    }

    [EntityBind("rdfs", "Class")]
    public class RdfClass : Resource
    {
        public RdfClass(string iri) : base(iri) { }
    }

    [EntityBind("rdf", "Property")]
    public class RdfPropertyWithRange : Resource
    {
        public RdfPropertyWithRange(string iri) : base(iri) { }

        [PropertyBind("rdfs", "range")]
        public RdfClass Range { get; set; }
    }
}