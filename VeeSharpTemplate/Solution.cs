#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#endregion

namespace VeeSharpTemplate
{
    public class Solution
    {
        private readonly List<string> _fileNames;
        private string _projectPath;

        public Solution(string mPath)
        {
            Path = mPath;
            Directory = System.IO.Path.GetDirectoryName(mPath);
            FileName = System.IO.Path.GetFileNameWithoutExtension(mPath);

            Files = new List<File>();
            _fileNames = new List<string>();
        }

        public string Directory { get; private set; }
        public string FileName { get; private set; }
        public string Path { get; private set; }
        public List<File> Files { get; private set; }
        public string ProjectFileName { get; private set; }

        public void AddNewFile(File mFile)
        {
            Files.Add(mFile);
            _fileNames.Add(mFile.FileName.RemoveBlank());
        }
        public void SetProject(string mProjectFileName)
        {
            ProjectFileName = mProjectFileName;
            _projectPath = System.IO.Path.GetDirectoryName(mProjectFileName);
        }

        public void LoadFromFile()
        {
            var xmlReader = new XmlTextReader(System.IO.File.OpenText(Path));

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType != XmlNodeType.Element) continue;
                if (xmlReader.Name == Utils.XmlFile) _fileNames.Add(xmlReader.ReadElementContentAsString().RemoveBlank());
                if (xmlReader.Name == Utils.XmlProjectFileName) ProjectFileName = xmlReader.ReadElementContentAsString();
                if (xmlReader.Name == Utils.XmlProjectPath) _projectPath = xmlReader.ReadElementContentAsString();
            }

            xmlReader.Close();

            foreach (var fileName in _fileNames) Files.Add(Utils.FileInSolutionFolder(this, fileName));
        }
        public void SaveToFile()
        {
            var xmlWriter = new XmlTextWriter(System.IO.File.CreateText(Path)) {Formatting = Formatting.Indented};
            xmlWriter.WriteStartElement(Utils.XmlRoot);

            xmlWriter.WriteElementString(Utils.XmlProjectFileName, ProjectFileName);
            xmlWriter.WriteElementString(Utils.XmlProjectPath, _projectPath);
            foreach (var file in _fileNames) xmlWriter.WriteElementString(Utils.XmlFile, file);

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        public void Generate(List<string> mParsedPreviews, bool mMakeProjectBackup = true)
        {
            if (string.IsNullOrEmpty(_projectPath)) throw new Exception("Project path is invalid");
            if (string.IsNullOrEmpty(ProjectFileName)) throw new Exception("Project filename is invalid");

            var filenames = new List<string>();

            foreach (var templateFile in Files)
            {
                var parsed = Parser.ParseSymbols(templateFile.Code);
                mParsedPreviews.Add(parsed);
                var processed = Parser.Process(parsed);
                var folderPath = _projectPath + @"\" + templateFile.Folder;

                System.IO.Directory.CreateDirectory(folderPath);

                var filename = string.Format(@"{0}" + templateFile.Prefix + "{1}.cs", templateFile.Folder, templateFile.TemplateName);
                filenames.Add(filename);

                var streamWriter = System.IO.File.CreateText(_projectPath + @"\" + filename);
                streamWriter.Write(processed);
                streamWriter.Flush();
                streamWriter.Close();
            }

            if (mMakeProjectBackup)
            {
                var projectBackup = System.IO.File.CreateText(string.Format(ProjectFileName + Utils.ExtensionBackup));
                var projectStream = System.IO.File.OpenText(ProjectFileName);
                projectBackup.Write(projectStream.ReadToEnd());
                projectStream.Close();
                projectBackup.Flush();
                projectBackup.Close();
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(ProjectFileName);

            var root = xmlDocument.DocumentElement;
            if (root == null) throw new Exception("Project is invalid (no root element)");
            var project = xmlDocument.GetElementsByTagName(Utils.XmlCSProject)[0];

            foreach (var filename in filenames)
            {
                var value = filename;

                var itemGroup = xmlDocument.CreateElement(Utils.XmlCSItemGroup, root.NamespaceURI);
                var compile = xmlDocument.CreateNode(XmlNodeType.Element, Utils.XmlCSCompile, root.NamespaceURI);
                var include = xmlDocument.CreateAttribute(Utils.XmlCSInclude);

                // Look if the file is already included
                var compileElements = xmlDocument.GetElementsByTagName(Utils.XmlCSCompile);
                if (compileElements.Cast<XmlElement>().Any(x => x.GetAttribute(Utils.XmlCSInclude) == value)) continue;

                include.Value = value;

                if (compile.Attributes == null) throw new Exception("Project is invalid (compile attributes are null)");
                compile.Attributes.Append(include);
                itemGroup.AppendChild(compile);

                project.InsertAfter(itemGroup, project.LastChild);
                itemGroup.Attributes.RemoveAll();
            }

            xmlDocument.Save(ProjectFileName);
        }
    }
}