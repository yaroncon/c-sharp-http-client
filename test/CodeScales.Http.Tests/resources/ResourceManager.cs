/* Copyright (c) 2010 CodeScales.com
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

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
