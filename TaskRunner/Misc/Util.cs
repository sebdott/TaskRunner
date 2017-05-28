using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaskRunner.Events;

namespace TaskRunner.Misc
{
    public static class Util
    {
        public static T LoadXML<T>(OpenFileDialog openFileDialog) where T : class
        {
            var myStream = openFileDialog.OpenFile();

            return new XmlSerializer(typeof(T)).Deserialize(myStream) as T;
        }

        public static void Save<T>(string path, T saveObject)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EventList));

            using (StreamWriter writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, saveObject);

            }
        }

        public static void Copy(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));

            foreach (var directory in Directory.GetDirectories(sourceDir))
                Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        public static bool IsStringNullorWhitespaceReturnFalse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            return false;
        }
    }
}
