using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using UnityEngine;

namespace Kage.Utils
{
    internal static class IO
    {
        // Serialise the given object as a JSON file.
        public static void WriteAsJson(object obj, Stream stream)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                serializer.WriteObject(stream, obj);
            }
            catch (SerializationException e)
            {
                Debug.LogError($"Failed to serialize object. Reason: {e.Message}");
            }
        }

        // Tries to deserialise an object from a JSON file.
        // If that fails for any reason, returns null instead.
        public static T ReadAsJson<T>(Stream stream)
            where T : class
        {
            try
            {
                try
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return serializer.ReadObject(stream) as T;
                }
                catch (SerializationException e)
                {
                    Debug.LogError($"Failed to deserialize object. Reason: {e.Message}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception while trying to load object from JSON file:\n{e.Message}");
                return null;
            }
        }
    }
}
