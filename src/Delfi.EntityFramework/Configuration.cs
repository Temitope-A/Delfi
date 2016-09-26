using Microsoft.Extensions.Configuration;
using Sparql.Algebra.GraphSources;
using System;

namespace Delfi.EntityFramework
{
    public class Configuration
    {
        #region static members
        private static Configuration _instance;

        /// <summary>
        /// Gets the singleton
        /// </summary>
        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
                return _instance;
            }
        }
        #endregion

        #region instance members
        private IConfigurationRoot _configurationRoot;

        /// <summary>
        /// Private constrctor for internal usage
        /// </summary>
        private Configuration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json");
            _configurationRoot = builder.Build();
        }

        public IGraphSource QueryEndpoint {
            get
            {
                var iri = _configurationRoot.GetSection("endpoint:query").Value;
                return new GraphSource(iri);
            }
        }

        public IGraphSource UpdateEndpoint {
            get
            {
                var iri = _configurationRoot.GetSection("endpoint:update").Value;
                return new GraphSource(iri);
            }
        }
        #endregion
    }
}
