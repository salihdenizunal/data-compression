using System.Collections.Generic;

namespace DataCompression
{
    class CharacterFrequency
    {
        public Dictionary<char, int> CharacterFrequencyPairs = new Dictionary<char, int>();

        // Constructor of the class which parses the string into characters and their frequencies.
        public CharacterFrequency(string stringToparse)
        {
            parseString(stringToparse);
        }

        private void parseString(string stringToparse)
        {
            // Loop thougrough the length of given string to count the frequencies.
            for (int i = 0; i < stringToparse.Length; i++)
            {
                // Get the character from the string.
                char character = stringToparse[i];

                // Check if the character has already been seen before.
                if (CharacterFrequencyPairs.ContainsKey(character))
                {
                    // If so, increase the frequency by 1.
                    CharacterFrequencyPairs[character] += 1;
                }
                else
                {
                    // Else, add the character-frequency pair.
                    CharacterFrequencyPairs.Add(character, 1);
                }
            }
        }
    }
}

