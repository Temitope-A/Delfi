using Xunit;
using Delfi.QueryProvider;
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
            IQueryableGraph graph = Context.Read<RdfProperty>();

            int i = 0;

            foreach (TreeNode<object> component in graph)
            {
                i++;
            }

            Assert.Equal(2758, i);
        }

        [Fact]
        public void Write()
        {
            Context.Append(new Statement(new Rdf("test2"), new Rdf("type"), new Rdf("Property")));
            Context.SaveChanges();

            int i = 0;

            foreach (TreeNode<object> component in Context.Read<RdfsClass>())
            {
                i++;
            }

            Assert.Equal(2759, i);
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
