using System.IO;

namespace Pharma263.Application.Extensions
{
    public static class StreamHelperExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            // If the stream is already a MemoryStream, just return its buffer.
            if (stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            // Otherwise, copy the stream to a new MemoryStream and return the buffer.
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
