using System;
using System.IO;
using System.Collections.Generic;

namespace DataCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example 1: "LARSA EURASIA"
            var compressor = new HuffmanEncoding(new FileInfo("example1.tree"));
            var encodedText = Utility.ReadTextFromFile("example1.encoded");
            Console.WriteLine(Decode(compressor, encodedText));

            // Example 2: Secret message.
            compressor = new HuffmanEncoding(new FileInfo("example2.tree"));
            encodedText = Utility.ReadTextFromFile("example2.encoded");
            Console.WriteLine(Decode(compressor, encodedText));
        }

        static string Decode(HuffmanEncoding tree, string encodedText)
        {
            // YOUR CODE HERE

            // You can access the Huffman tree using:
            //
            // tree.Root.Left, tree.Root.Right, and tree.Root.Character

            // You can check whether a node is leaf or not using:
            //
            // node.IsLeaf()

            return "decoded text";
        }

    }
}
