using Xunit;
using Delfi.QueryProvider;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.Evaluators;
using Delfi.QueryProvider.Attributes;
using Delfi.QueryProvider.Tree;


namespace GraphRepository.Test
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class EndToEndTests
    {
        private IGraphProvider _provider = new GraphProvider<SparqlBGPEvaluator>();

        [Fact]
        public void HappyPath()
        {             
            IQueryableGraph graph = new QueryableGraph(_provider, new GraphExpression(new Variable(typeof(RdfProperty))));

            int i = 0;

            foreach (TreeNode<object> component in graph)
            {
                i++;
            }

            Assert.Equal(2758, i);
        }

        //[Fact]
        //public void Limit()
        //{
        //    SparqlBGPEvaluator sparqlEvaluator = new SparqlBGPEvaluator("http://localhost:7200/repositories/Pets");
        //    IGraphProvider provider = new GraphProvider(sparqlEvaluator);
        //    IQueryableGraph query = new QueryableGraph(provider, new GraphExpression(new Variable(typeof(RdfProperty))));
        //    var limitedQuery = query.Limit(10);

        //    var result = query;
        //    var _10Result = limitedQuery;

        //    Assert.True(result.Count() > 10);
        //    Assert.Equal(10, _10Result.Count());
        //}

        //[Fact]
        //public void Require()
        //{
        //    SparqlBGPEvaluator sparqlEvaluator = new SparqlBGPEvaluator("http://localhost:7200/repositories/Pets");
        //    IGraphProvider provider = new GraphProvider(sparqlEvaluator);
        //    IQueryableGraph query = new QueryableGraph(provider, new GraphExpression(new Variable(typeof(RdfProperty)))).Require(new Rdfs("domain"));

        //    var result = query;

        //    Assert.True(result.Count() > 1);
        //}

        //[Fact]
        //public void Expand()
        //{
        //    SparqlBGPEvaluator sparqlEvaluator = new SparqlBGPEvaluator("http://localhost:7200/repositories/Pets");
        //    IGraphProvider provider = new GraphProvider(sparqlEvaluator);
        //    IQueryableGraph query = new QueryableGraph(provider, new GraphExpression(new Variable(typeof(RdfProperty))));
        //    var requireQuery = query.Require(new Rdfs("nonesuch"));
        //    var expandQuery = query.Expand(new Rdfs("nonesuch"));

        //    var result = query;
        //    var requireResult = requireQuery.GetResults().ToList();
        //    var expandResult = expandQuery.GetResults().ToList();

        //    Assert.Equal(result.Count(), expandResult.Count());
        //    Assert.NotEqual(result.Count(), requireResult.Count());
        //}
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
