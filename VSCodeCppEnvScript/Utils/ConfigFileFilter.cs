using System;
using System.IO;
using System.Collections.Generic;

namespace VSCodeCppEnvScript.Utils
{
    public class ConfigFileFilter
    {
        public Dictionary<string, string> FilterDict { get; set; }

        public ConfigFileFilter(Dictionary<string, string> dict)
        {
            FilterDict = dict
                ?? throw new ArgumentNullException(nameof(dict));
        }

        public void FilterFiles(params string[] path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            foreach (var file in path)
            {
                if (file is null) throw new ArgumentNullException(nameof(file));

                var content = File.ReadAllText(file);

                foreach (var key in FilterDict.Keys)
                {
                    if (FilterDict.TryGetValue(key, out string dest))
                    {
                        content = content.Replace(key, dest);
                    }
                }

                File.WriteAllText(file, content);
            }
        }

        public bool TryFilterFiles(params string[] path)
        {
            var result = true;
            if (path is null) throw new ArgumentNullException(nameof(path));
            foreach (var file in path)
            {
                if (file is null)
                {
                    result = false;
                    continue;
                }

                var content = File.ReadAllText(file);

                foreach (var key in FilterDict.Keys)
                {
                    if (FilterDict.TryGetValue(key, out string dest))
                    {
                        content = content.Replace(key, dest);
                    }
                }

                File.WriteAllText(file, content);
            }

            return result;
        }
    }
}
