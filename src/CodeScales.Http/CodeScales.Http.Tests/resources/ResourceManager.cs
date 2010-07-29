using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace CodeScales.Http.Tests.resources
{
    public class ResourceManager
    {
        private const string RESOURCE_FOLDER = "resources";

        public static FileInfo GetResourceFileInfo(string fileName)
        {
            string filePath = Assembly.GetAssembly(typeof(ResourceManager)).Location;
            filePath = filePath.Substring(0, filePath.LastIndexOf('\\')) + "\\";
            byte[] fileBytes = GetResourceBinary(fileName);
            File.WriteAllBytes(filePath + fileName, fileBytes);
            return new FileInfo(filePath + fileName);
        }

        public static byte[] GetResourceBinary(string fileName)
        {
            Stream stream = Assembly.GetAssembly(typeof(ResourceManager)).GetManifestResourceStream("CodeScales.Http.Tests.resources." + fileName);
            BinaryReader reader = new BinaryReader(stream);
            return reader.ReadBytes((int)reader.BaseStream.Length);
        }
    }
}
