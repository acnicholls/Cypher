using System;
using System.Collections;

namespace cypher.data.classes
{
    /// <summary>
    /// list of integers corresponding to words in the message
    /// </summary>
    public class MessageWords
    {
        

        public ArrayList wordValues = new ArrayList();

        public MessageWords() { }

        public MessageWords(EncryptedMessage inMessage)
        {
            wordValues.Clear();
            string[] codes = inMessage.Content.Split(Convert.ToChar(" "));
            foreach (string s in codes)
            {
                wordValues.Add(s);
            }
        }




    }
}
