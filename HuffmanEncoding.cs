using System.IO;
using System.Collections.Generic;
using Priority_Queue;

namespace DataCompression
{
    class Node
    {
        public char Character;
        public Node Left, Right;

        // Constructor of the Node class. 
        public Node(char character, Node left, Node right)
        {
            Character = character;
            Left = left;
            Right = right;
        }

        // If left and right nodes are null, which means the node doesn't have a children,
        // then it is a leaf node (It is at the bottom most part of the tree).
        public bool IsLeaf()
        {
            if (Left == null && Right == null)
                return true;
            return false;
        }
    }

    class HuffmanEncoding
    {
        public Node Root;

        // Constructor of the HuffmanTree class. 
        public HuffmanEncoding(string stringToBuildTree)
        {
            // Build the tree from the given string.
            Root = buildTree(stringToBuildTree);
        }

        // Constructor of HuffmanTree class from a file.
        public HuffmanEncoding(System.IO.FileInfo treeFile)
        {
            Root = readTree(treeFile.FullName);
        }

        private Node buildTree(string stringToBuildTree)
        {
            // Get the characters and their frequencies.
            CharacterFrequency parsed = new CharacterFrequency(stringToBuildTree);

            // Create a priorty queue consist of Node frequency tuples.
            SimplePriorityQueue<(Node,int)> priorityQueue = new SimplePriorityQueue<(Node,int)>();

            // For each pair in the CharacterFrequencyPairs, add them into the priority queue 
            // with respect to their frequencies.
            foreach(KeyValuePair<char,int> characterFrequencyPair in parsed.CharacterFrequencyPairs)
            {
                char character = characterFrequencyPair.Key;    // Key is character.
                int frequency = characterFrequencyPair.Value;   // Value is frequency.
                
                // If the frequency is larger than 0, insert it to priority queue as a Node-Frequency tuple.
                if (frequency > 0)
                {
                    priorityQueue.Enqueue((new Node(character, null, null),frequency), frequency);
                }
            }

            // Until there is only 1 element left in the priority queue, get the ones with highest priority (lowest frequency).
            while (priorityQueue.Count > 1)
            {
                var x = priorityQueue.Dequeue();
                var y = priorityQueue.Dequeue();

                // Make them left and right children of a parent node.
                Node parent = new Node('\0', x.Item1, y.Item1);

                // Enqueue the parent node and the addition of the frequencies of its children as a tuple.
                priorityQueue.Enqueue((parent, x.Item2 + y.Item2), x.Item2 + y.Item2);
            }

            // Last item will be the root node of the tree.
            var rootNode = priorityQueue.Dequeue().Item1;
            return rootNode;
        }

        public void WriteTree(string filename)
        {
            using (var file = File.Open(filename, FileMode.Create))
            {
                var writer = new BinaryWriter(file);
                writeTree(Root, writer);
                writer.Close();
            }
        }

        private void writeTree(Node node, BinaryWriter file)
        {
            if (node.IsLeaf())
            {
                file.Write(true);
                file.Write(node.Character);
                return;
            }

            file.Write(false);

            writeTree(node.Left, file);
            writeTree(node.Right, file);
        }

        private Node readTree(string fileName, BinaryReader reader = null)
        {
            BinaryReader file;
            if (reader == null)
            {
                file = new BinaryReader(File.Open(fileName, FileMode.Open));
            }
            else
            {
                file = reader;
            }

            if (file.ReadBoolean())
            {
                char c = file.ReadChar();
                // Create a leaf node.
                return new Node(c, null, null);
            }
            Node x = readTree(fileName, file);
            Node y = readTree(fileName, file);
            return new Node('\0', x, y);
        }

        public string Encode(string decodedText)
        {
            return Encode(decodedText, BuildTable(Root));
        }

        public static Dictionary<char, string> BuildTable(Node node, string code = "", Dictionary<char, string> charCodePairs = null)
        {
            if (charCodePairs == null)
            {
                charCodePairs = new Dictionary<char, string>();
            }

            if (node.IsLeaf())
            {
                charCodePairs.Add(node.Character, code);
                return charCodePairs;
            }

            BuildTable(node.Left, code + "0", charCodePairs);
            BuildTable(node.Right, code + "1", charCodePairs);
            return charCodePairs;
        }

        static string Encode(string decodedText, Dictionary<char, string> charCodePairs)
        {
            // Initialize the return string.
            string encoded = "";

            for (int i = 0; i < decodedText.Length; i++)
            {
                // Get the character from the given string.
                var character = decodedText[i];

                // Find its code from charCodePairs and add it to the encoded string.
                encoded += charCodePairs[character];
            }
            // Return the cumulatively added encoded string.
            return encoded;
        }

        private string Decode(string encodedText)
        {
            string decoded = "";
            Node n = Root;
            for (int i = 0; i < encodedText.Length; i++)
            {
                char bit = encodedText[i];
                if (bit == '0')
                    n = n.Left;
                else if (bit == '1')
                    n = n.Right;
                else
                    throw new System.Exception("Invalid encoded text");
                if (n.IsLeaf())
                {
                    // We got a complete Huffman code.
                    decoded += n.Character;

                    // Reset to the root for the next character.
                    n = Root;
                }
            }
            return decoded;
        }
    }
}
