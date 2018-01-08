using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace POCOGenerator.Helpers
{
    public static class SerializationHelper
    {
        #region XmlSerializer

        public static MemoryStream XmlSerializeToMemoryStream<T>(T serializableObject) where T : new()
        {
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, serializableObject);
            stream.Position = 0;
            return stream;
        }

        public static T XmlDeserializeFromMemoryStream<T>(MemoryStream stream) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));
            stream.Position = 0;
            return (T)serializer.Deserialize(stream);
        }

        public static void XmlSerializeToFile<T>(T serializableObject, string path, FileMode fileMode = FileMode.Create) where T : new()
        {
            using (var fs = new FileStream(path, fileMode))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fs, serializableObject);
            }
        }

        public static T XmlDeserializeFromFile<T>(string path) where T : new()
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
        }

        public static string XmlSerializeToString<T>(T serializableObject) where T : new()
        {
            using (var sw = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(sw, serializableObject);
                return sw.ToString();
            }
        }

        public static T XmlDeserializeFromString<T>(string xml) where T : new()
        {
            using (StringReader sr = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        public static StringBuilder XmlSerializeToStringBuilder<T>(T serializableObject) where T : new()
        {
            StringBuilder sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(sw, serializableObject);
                return sb;
            }
        }

        public static T XmlDeserializeFromStringBuilder<T>(StringBuilder sb) where T : new()
        {
            using (StringReader sr = new StringReader(sb.ToString()))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(sr);
            }
        }

        #endregion

        #region BinaryFormatter

        public static MemoryStream BinarySerializeToMemoryStream<T>(T serializableObject)
        {
            var stream = new MemoryStream();
            var serializer = new BinaryFormatter();
            serializer.Serialize(stream, serializableObject);
            stream.Position = 0;
            return stream;
        }

        public static T BinaryDeserializeFromMemoryStream<T>(MemoryStream stream)
        {
            stream.Position = 0;
            var serializer = new BinaryFormatter();
            return (T)serializer.Deserialize(stream);
        }

        public static void BinarySerializeToFile<T>(T serializableObject, string path, FileMode fileMode = FileMode.Create)
        {
            using (var fs = new FileStream(path, fileMode))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fs, serializableObject);
            }
        }

        public static T BinaryDeserializeFromFile<T>(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var serializer = new BinaryFormatter();
                return (T)serializer.Deserialize(fs);
            }
        }

        public static byte[] BinarySerializeToBinaryArray<T>(T serializableObject)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(stream, serializableObject);
                return stream.ToArray();
            }
        }

        public static T BinaryDeserializeFromBinaryArray<T>(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                stream.Position = 0;
                var serializer = new BinaryFormatter();
                return (T)serializer.Deserialize(stream);
            }
        }

        #endregion
    }
}
