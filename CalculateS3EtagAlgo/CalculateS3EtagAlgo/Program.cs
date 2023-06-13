// See https://aka.ms/new-console-template for more information
using CommandLine;
using System.Globalization;
using System.Security.Cryptography;

namespace CalculateS3EtagAlgo
{
    public class Options
    {
        [Option('f', "filepath", Required = true, HelpText = "Set file path to read the file.")]
        public string FilePath { get; set; }
        [Option('c', "chunksize", Required = true, HelpText = "Set chunk size in MB.")]
        public int ChunkSizeInMB { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsedAsync<Options>(async o =>
            {
                var file = o.FilePath;
                byte[] fileBytes = File.ReadAllBytes(file);
                var multipartETag = CalculateMultipartEtag(fileBytes, o.ChunkSizeInMB);
                Console.WriteLine(multipartETag);
            });
        }

        static string CalculateMultipartEtag(byte[] array, int chunkCount)
        {
            var multipartSplitCount = 0;
            var chunkSize = 1024 * 1024 * chunkCount;
            var splitCount = array.Length / chunkSize;
            var mod = array.Length - chunkSize * splitCount;
            IEnumerable<byte> concatHash = new byte[] { };

            using (HashAlgorithm md5 = MD5.Create())
            {
                for (var i = 0; i < splitCount; i++)
                {
                    var offset = i == 0 ? 0 : chunkSize * i;
                    var chunk = GetSegment(array, offset, chunkSize);
                    byte[] hash = chunk.ToArray().GetHash(md5);
                    concatHash = concatHash.Concat(hash);
                    Console.WriteLine(chunk.ToArray().GetHash(md5).ToHexString());

                    multipartSplitCount++;
                }
                if (mod != 0)
                {
                    var chunk = GetSegment(array, chunkSize * splitCount, mod);
                    byte[] hash = chunk.ToArray().GetHash(md5);
                    concatHash = concatHash.Concat(hash);
                    Console.WriteLine(chunk.ToArray().GetHash(md5).ToHexString());
                    multipartSplitCount++;
                }
                var multipartHash = concatHash.ToArray().GetHash(md5).ToHexString();
                return multipartHash + "-" + multipartSplitCount;
            }
        }

        static ArraySegment<T> GetSegment<T>(T[] array, int offset, int? count = null)
        {
            if (count == null) { count = array.Length - offset; }
            return new ArraySegment<T>(array, offset, count.Value);
        }
    }
}
