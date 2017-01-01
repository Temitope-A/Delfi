using Xunit;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.Attributes;
using Delfi.QueryProvider.Tree;
using Delfi.EntityFramework;
using Delfi.QueryProvider.StandardNamespaces;

namespace Delfi.Test
{
    public class EndToEndTests
    {
        protected IGraphContext Context = new GraphContext();

        [Fact]
        public void Read()
        {             
            int i = 0;

            foreach (TreeNode<object> component in Context.Read<RdfProperty>())
            {
                i++;
            }

            Assert.Equal(2758, i);
        }

        [Fact]
        public void Write()
        {
            var statement = new Statement(new Rdf("test3"), new Rdf("type"), new Rdf("Property"));
            var graph = Context.Read<RdfProperty>();

            int i = 0;

            foreach (TreeNode<object> component in graph )
            {
                i++;
            }

            var countBeforeAdd = i;

            Context.Append(statement);
            Context.SaveChanges();

            i = 0;

            foreach (TreeNode<object> component in graph)
            {
                i++;
            }

            var countAfterAdd = i;

            Context.Remove(statement);
            Context.SaveChanges();

            foreach (TreeNode<object> component in graph)
            {
                i++;
            }

            var countAfterDelete = i;

            Assert.Equal(countBeforeAdd, countAfterDelete);

            Assert.Equal(countBeforeAdd + 1, countAfterAdd);
        }
    }

    [EntityBind("rdf","Property")]
    public class RdfProperty : Resource
    {
        public RdfProperty(string iri):base(iri) { }

        [PropertyBind("rdfs", "domain")]
        public RdfsClass Domain { get; set; }

        [Required]
        [PropertyBind("rdfs", "range")]
        public RdfsClass Range { get; set; }

        [PropertyBind("foaf", "age")]
        public int? Age { get; set; }
    }

    [EntityBind("rdfs", "Class")]
    public class RdfsClass : Resource
    {
        public RdfsClass(string iri) : base(iri) { }

        [PropertyBind("rdfs", "subClassOf")]
        public Resource SuperClass { get; set; }
    }
}
