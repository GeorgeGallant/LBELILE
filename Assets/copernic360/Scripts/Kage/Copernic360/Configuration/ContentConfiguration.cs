using Kage.Utils;
using System.IO;
using System.Runtime.Serialization;

namespace Kage.Copernic360.Configuration
{
    // Base class for configuration data for a single
    // piece of content (image or video). The class
    // is abstract to clarify that instantiating this
    // base case is meaningless.
    [DataContract]
    public abstract class ContentConfiguration
    {
        // Attempts to load a content's configuration data from the given file.
        // If that fails for any reason, returns null instead.
        private protected static T LoadFromJsonFile<T>(string filePath)
            where T : ContentConfiguration
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return IO.ReadAsJson<T>(stream);
            }
        }

        // Saves this configuration to the given file.
        internal void SaveToJsonFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                IO.WriteAsJson(this, stream);
            }
        }
    }
}