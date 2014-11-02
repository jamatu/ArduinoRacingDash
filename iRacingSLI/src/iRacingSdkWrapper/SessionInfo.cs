using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using iRSDKSharp;
using YamlDotNet.RepresentationModel;

namespace iRacingSdkWrapper
{
    /// <summary>
    /// Represents a single update of the Session Info YAML and includes parsing capabilities.
    /// </summary>
    public class SessionInfo
    {
        public SessionInfo(string yaml, double updateTime)
        {
            _updateTime = updateTime;

            _yaml = yaml;

            this.ParseYaml();
        }

        #region Properties

        private readonly double _updateTime;
        /// <summary>
        /// The time of this update.
        /// </summary>
        public double UpdateTime { get { return _updateTime; } }

        private readonly string _yaml;
        /// <summary>
        /// The raw YAML string representing the session info.
        /// </summary>
        public string Yaml { get { return _yaml; } }

        private YamlStream _yamlStream;
        public YamlStream YamlStream { get { return _yamlStream; } }

        private YamlMappingNode _yamlRoot;
        public YamlMappingNode YamlRoot { get { return _yamlRoot; } }

        #endregion

        #region Methods
        
        private void ParseYaml()
        {
            using (var reader = new StringReader(this.Yaml))
            {
                _yamlStream = new YamlStream();
                _yamlStream.Load(reader);
                _yamlRoot = (YamlMappingNode)_yamlStream.Documents[0].RootNode;
            }
        }

        public YamlQuery this[string key]
        {
            get
            {
                return YamlQuery.Mapping(_yamlRoot, key);
            }
        }

        /// <summary>
        /// Gets a value from the session info YAML, or null if there is an error.
        /// </summary>
        /// <param name="query">The YAML query path to the value.</param>
        public string TryGetValue(string query)
        {
            try
            {
                return YamlParser.Parse(_yaml, query);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value from the session info YAML. Returns true if successfull, false if there is an error.
        /// </summary>
        /// <param name="query">The YAML query path to the value.</param>
        /// <param name="value">When this method returns, contains the requested value if the query was valid, or null if the query was invalid.</param>
        public bool TryGetValue(string query, out string value)
        {
            try
            {
                value = YamlParser.Parse(_yaml, query);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }

        /// <summary>
        /// Gets a value from the session info YAML. May throw an exception. Use <see cref="TryGetValue"/> for safer operation.
        /// </summary>
        /// <param name="query">The YAML query path to the value.</param>
        public string GetValue(string query)
        {
            return YamlParser.Parse(_yaml, query);
        }

        #endregion
    }
}
