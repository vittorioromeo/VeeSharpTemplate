#region
using System.IO;
using System.Xml;

#endregion

namespace VeeSharpTemplate
{
    public class File
    {
        private readonly Solution _solution;

        public File(Solution mSolution, string mFileName)
        {
            _solution = mSolution;
            FileName = mFileName;

            TemplateName = mFileName;
            Code = Utils.DefaultTemplateCode;
            Prefix = Utils.DefaultTemplatePrefix;
            Folder = Utils.DefaultSolutionFolderPrefix + mSolution.FileName + @"\";
        }

        public string Code { get; set; }
        public string Prefix { get; set; }
        public string TemplateName { get; set; }
        public string FileName { get; private set; }
        public string Folder { get; set; }

        public void SaveToFile()
        {
            var path = CreateAndGetPath();
            var xmlWriter = new XmlTextWriter(System.IO.File.CreateText(path)) {Formatting = Formatting.Indented};
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
            var path = CreateAndGetPath();
            var xmlReader = new XmlTextReader(System.IO.File.OpenText(path));
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
        private string CreateAndGetPath()
        {
            var directory = string.Format(@"{0}\{1}{2}\", _solution.Directory, Utils.DefaultSolutionSourceFolderPrefix, _solution.FileName);
            Directory.CreateDirectory(directory);
            return directory + FileName + Utils.ExtensionFile;
        }
    }
}