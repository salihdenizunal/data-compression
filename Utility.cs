using System;
using System.Collections.Generic;
using System.Text;

namespace DataCompression
{
    class Utility
    {
        public static void PrepareExamples(string exampleText, string filename)
        {
            var compressor = new HuffmanEncoding(exampleText);
            compressor.WriteTree(filename + ".tree");
            using (var file = System.IO.File.OpenWrite(filename + ".encoded"))
            {
                var writer = new System.IO.StreamWriter(file);
                writer.Write(compressor.Encode(exampleText));
                writer.Close();
            }
        }

        public static string ReadTextFromFile(string filename)
        {
            using (var file = System.IO.File.OpenRead(filename))
            {
                var reader = new System.IO.StreamReader(file);
                return reader.ReadToEnd();
            }
        }

        public static void PrintTable(Dictionary<char, string> table)
        {
            foreach (KeyValuePair<char, string> pair in table)
            {
                Console.WriteLine("Chacter = {0}, Code = {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("\n");
        }

        public static string GetBinaryString(string stringToMakeBinary)
        {
            // This function is just to show how UTF8 encoding would look like.
            string binaryString = "";

            // Convert string to byte
            byte[] byteArray = Encoding.ASCII.GetBytes(stringToMakeBinary);

            for (int i = 0; i < byteArray.Length; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    binaryString += (byteArray[i] & 0x80) > 0 ? "1" : "0";
                    byteArray[i] <<= 1;
                }
                binaryString += " ";
            }


            return binaryString;
        }

    }
}
