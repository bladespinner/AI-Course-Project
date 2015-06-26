using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MihailGospodinov.WordPrediction.Learning
{
    /// <summary>
    /// Wrapper around a stream providing enumeration of words
    /// </summary>
    public class WordStream : IDisposable , IEnumerable<string>
    {
        private Stream stream;
        public WordStream(Stream stream)
        {
            this.stream = stream;
        }

        public IEnumerable<string> Words()
        {
            int symbol = 0;
            string word = String.Empty;
            do
            {
                symbol = stream.ReadByte();
                char character = (char)symbol;
                if (char.IsLetterOrDigit(character) || character == '-')
                {
                    word += character;
                }
                else
                {
                    if (!String.IsNullOrWhiteSpace(word))
                    {
                        yield return word.ToLower();
                    }
                    word = String.Empty;
                }
            }
            while (symbol > 0);
            yield return String.Empty;
        }

        public void Dispose()
        {
            stream.Dispose();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Words().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
