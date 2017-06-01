using MadsKristensen.AddAnyFile.Templates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadsKristensen.AddAnyFile
{
    class ExtensionConfig
    {
        [JsonProperty("mappings")]
        public TemplateMap Mappings { get; set; }

        [JsonProperty("defaultExtension")]
        public string DefaultExtension { get; set; }

        public static ExtensionConfig LoadFromFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string input = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<ExtensionConfig>(input);
                }
                catch (Exception e)
                {
                    AddAnyFilePackage.LogToOutputPane(string.Concat(
                        "There was error loading the configuration from: ",
                        path,
                        "\r\n",
                        e.Message));
                }
            }
            return new ExtensionConfig();
        }
    }
}
