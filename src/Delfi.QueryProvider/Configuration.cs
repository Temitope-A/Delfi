using Delfi.QueryProvider.RDF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Delfi.QueryProvider
{
    /// <summary>
    /// Singleton implementation of a configuration provider
    /// </summary>
    public class Configuration
    {
        private static Configuration _instance;

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

        /// <summary>
        /// Get the configured RDF namespaces
        /// </summary>
        public List<NamespaceDeclaration> Namespaces {
            get {
                var result = new List<NamespaceDeclaration>();
                _configurationRoot.GetSection("namespaces").Bind(result);

                return result;
            }
        }
    }
}
