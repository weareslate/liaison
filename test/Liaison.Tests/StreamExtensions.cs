using System.IO;
using Newtonsoft.Json.Linq;

namespace Liaison.Tests
{
    internal static class StreamExtensions
    {
        public static JObject GetJson(
            this Stream stream )
        {
            using ( var bodyReader = new StreamReader( stream ) )
            {
                bodyReader.BaseStream.Seek( 0, SeekOrigin.Begin );
                using ( var baseReader = new StreamReader( bodyReader.BaseStream ) )
                {
                    var responseJson = baseReader.ReadToEnd();
                    return JObject.Parse( responseJson );
                }
            }
        }
    }
}