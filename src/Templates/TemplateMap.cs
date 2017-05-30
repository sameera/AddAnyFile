namespace MadsKristensen.AddAnyFile.Templates
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using M = TemplateMapping;

    /// <summary>
    /// Map of input patterns and their corresponding VS Templates. Ideally, these patterns would be configurable via the 
    /// Visual Studio Options dialog box.
    /// </summary>
    public class TemplateMap : List<TemplateMapping>
    {
        /// <summary>
        /// Loads the default mappings.
        /// </summary>
        public void LoadDefaultMappings()
        {
            this.Add(new M(@"^c\:(?<name>.*)", "CSharp", "Class"));
            this.Add(new M(@"^i\:(?<name>.*)", "CSharp", "Interface"));
            this.Add(new M(@"^(?<name>I[A-Z].*)\.cs$", "CSharp", "Interface"));
            this.Add(new M(@"^(?<name>I[A-Z].*)\.vb$", "VisualBasic", "Interface"));
            /* NOTE: With VS 2015 Community Edition, CodeFile.cs was not replacing the $rootnamespace$ tag.
             * If this happens, open the CodeFile.vstemplate file 
             * (e.g. in C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\ItemTemplates\CSharp\Code\1033\CodeFile\CodeFile.vstemplate)
             * and replace
             
              <TemplateContent>
                <ProjectItem>CodeFile.cs</ProjectItem>
              </TemplateContent>

            * with

              <TemplateContent>
                <ProjectItem ReplaceParameters="true">CodeFile.cs</ProjectItem>
              </TemplateContent>

            */
            this.Add(new M(@"^e:(?<name>.*)\.cs", "CSharp", "CodeFile"));
            // Following mappings are the broadest from a language POV. So, they need to be at the end of the mappings
            this.Add(new M(@"^(?<name>.*)\.cs$", "CSharp", "Class"));
            this.Add(new M(@"^(?<name>.*)\.js$", "JavaScript", "JScript"));
        }

        public static TemplateMap LoadFromFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    string input = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<TemplateMap>(input);
                }
                catch (Exception e)
                {
                    AddAnyFilePackage.LogToOutputPane(string.Concat(
                        "There was error loading the mappings from: ",
                        path,
                        "\r\n",
                        e.Message));
                }
            }
            return null;
        }

        public static void WriteToFile(TemplateMap map, string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string json = JsonConvert.SerializeObject(map, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (System.Exception e)
            {
                AddAnyFilePackage.LogToOutputPane(string.Concat(
                    "Could not save the mapping file to: ",
                    path,
                    "\r\n",
                    e.Message));

            }
        }

        public void MergeWith(TemplateMap lowerPriorityMappings)
        {
            this.AddRange(lowerPriorityMappings);
        }

        public TemplateMap()
        {
        }
    }
}
