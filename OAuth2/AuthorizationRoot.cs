using System;
using System.Collections.Generic;
using System.Reflection;
using HL.OAuth2.Client;
using HL.OAuth2.Configuration;
using HL.OAuth2.Infrastructure;
using System.Linq;

namespace HL.OAuth2
{
    public class AuthorizationRoot
    {
        private readonly IRequestFactory _requestFactory;
        private readonly OAuth2ConfigurationSection _configurationSection;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRoot" /> class.
        /// </summary>
        /// <remarks>
        /// Since this is boundary class, we decided to create 
        /// parameterless constructor where default implementations of dependencies are used.
        /// So, despite we encourage you to employ IoC pattern, 
        /// you are still able to just create instance of manager manually and then use it in your project.
        /// </remarks>
        public AuthorizationRoot() : 
            this(new ConfigurationManager(), "oauth2", new RequestFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRoot" /> class.
        /// </summary>
        /// <param name="configurationManager">The configuration manager.</param>
        /// <param name="configurationSectionName">Name of the configuration section.</param>
        /// <param name="requestFactory">The request factory.</param>
        public AuthorizationRoot(
            IConfigurationManager configurationManager, 
            string configurationSectionName, 
            IRequestFactory requestFactory)
        {
            _requestFactory = requestFactory;
            _configurationSection = configurationManager
                .GetConfigSection<OAuth2ConfigurationSection>(configurationSectionName);
        }
        
        /// <summary>
        /// Returns collection of clients which were configured 
        /// using application configuration file and are enabled.
        /// </summary>
        public virtual IEnumerable<IClient> Clients
        {
            get
            {
                var types = this.GetClientTypes().ToList();
                Func<ClientConfiguration, Type> getType = 
                    configuration => types.FirstOrDefault(x => x.Name == configuration.ClientTypeName);

                return
                    _configurationSection.Services.AsEnumerable()
                                        .Where(configuration => configuration.IsEnabled)
                                        .Select(configuration => new { configuration, type = getType(configuration) })
                                        .Where(o => o.type != null)
                                        .Select(o => (IClient)Activator.CreateInstance(o.type, _requestFactory, o.configuration));                
            }
        }

        /// <summary>
        /// Returns collection of client types to consider
        /// </summary>        
        protected virtual IEnumerable<Type> GetClientTypes()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var  typeList = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    var types = assembly.GetTypes();
                    var iclientTypes = types.Where(p => typeof(IClient).IsAssignableFrom(p));
                    typeList.AddRange(iclientTypes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error loading types from assembly {assembly.FullName}");
                }
            }
            return typeList;
        }
    }
}