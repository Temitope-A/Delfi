﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Delfi.QueryProvider.RDF;

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
        public List<NamespaceDefinition> Namespaces {
            get {
                var result = new List<NamespaceDefinition>();
                _configurationRoot.GetSection("namespaces").Bind(result);

                return result;
            }
        }
    }
}
