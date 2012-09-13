namespace VeeSharpTemplate
{
    public static class Utils
    {
        public const string AssemblyName = "VeeSharpTemplate";

        public const string XmlRoot = "VeeSharpTemplate";
        public const string XmlPrefix = "Prefix";
        public const string XmlTemplateName = "TemplateName";
        public const string XmlCode = "Code";
        public const string XmlFile = "File";
        public const string XmlFolder = "Folder";
        public const string XmlProjectFileName = "ProjectFileName";
        public const string XmlProjectPath = "ProjectPath";

        public const string XmlCSProject = "Project";
        public const string XmlCSItemGroup = "ItemGroup";
        public const string XmlCSCompile = "Compile";
        public const string XmlCSInclude = "Include";

        public const string Symbol = "$$";
        public const string MethodWriteLine = "WriteLine";
        public const string ScriptSuffix = "}}}";
        public const string ScriptName = "GeneratedScript";
        public const string ScriptPrefix = "using System; namespace VeeSharpTemplate{ public class " + ScriptName + ":Script{ public override void Run(){";

        public const string DefaultTemplateName = "unnamed";
        public const string DefaultTemplatePrefix = "vtg_";
        public const string DefaultTemplateCode = "// write c# code here";
        public const string DefaultProjectFolder = @"vte\";

        public const string ExtensionBackup = ".vstbak";
        public const string ExtensionSolution = ".vstsln";
        public const string ExtensionFile = ".vst";

        public static string RemoveBlank(this string mString) { return mString.Replace("\n", "").Replace("\r", "").Replace("\t", ""); }
        public static File FileFromPath(string mPath)
        {
            var templateFile = new File(mPath);
            templateFile.LoadFromFile();
            return templateFile;
        }
    }
}