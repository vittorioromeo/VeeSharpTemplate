#region
using System.Xml;

#endregion

namespace VeeSharpTemplate
{
    public class File
    {
        public File(string mPath)
        {
            Path = mPath;

            TemplateName = Utils.DefaultTemplateName;
            Code = Utils.DefaultTemplateCode;
            Prefix = Utils.DefaultTemplatePrefix;
            Folder = Utils.DefaultProjectFolder;
        }

        public string Code { get; set; }
        public string Prefix { get; set; }
        public string TemplateName { get; set; }
        public string Path { get; private set; }
        public string Folder { get; set; }

        public void SaveToFile()
        {
            var xmlWriter = new XmlTextWriter(System.IO.File.CreateText(Path)) {Formatting = Formatting.Indented};
            xmlWriter.WriteStartElement(Utils.XmlRoot);

            xmlWriter.WriteElementString(Utils.XmlPrefix, Prefix);
            xmlWriter.WriteElementString(Utils.XmlTemplateName, TemplateName);
            xmlWriter.WriteElementString(Utils.XmlCode, Code);
            xmlWriter.WriteElementString(Utils.XmlFolder, Folder);

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }
        public void LoadFromFile()
        {
            var xmlReader = new XmlTextReader(System.IO.File.OpenText(Path));

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType != XmlNodeType.Element) continue;

                if (xmlReader.Name == Utils.XmlPrefix) Prefix = xmlReader.ReadElementContentAsString();
                if (xmlReader.Name == Utils.XmlTemplateName) TemplateName = xmlReader.ReadElementContentAsString();
                if (xmlReader.Name == Utils.XmlCode) Code = xmlReader.ReadElementContentAsString();
                if (xmlReader.Name == Utils.XmlFolder) Folder = xmlReader.ReadElementContentAsString();
            }

            xmlReader.Close();
        }
    }
}