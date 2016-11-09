using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TimeStopper
{
    public static class XmlSerializers
    {
        public static void SerializeToXmlFile(this object objectToSerialize, string serializationPath)
        {
            using (var serializationFile = new FileStream(serializationPath, FileMode.Create))
            {
                var serializer = new XmlSerializer(objectToSerialize.GetType());

                serializer.Serialize(serializationFile, objectToSerialize);
            }
        }

        public static T DeserializeFromXmlFile<T>(string xmlFilePath) where T : class
        {
            using (var xmlFileStream = new FileStream(xmlFilePath, FileMode.Open))
            {
                var deserializer = new XmlSerializer(typeof(T));

                var deserializedIntervallRecorderInstance = deserializer.Deserialize(xmlFileStream) as T;

                return deserializedIntervallRecorderInstance;
            }
        }
    }
}
