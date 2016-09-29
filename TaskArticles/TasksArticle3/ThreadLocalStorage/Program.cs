using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadLocalStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            int matches = 0;
            object syncLock = new object();


            string[] words
                = new string[] { "the","other","day","I","was",
                "speaking","to","a","man","about",
                "this","and","that","cat","and","he","told","me",
                "the","only","other","person","to","ask","him",
                "about","the","crazy","cat","was","the","cats","owner"};

            string searchWord = "cat";

            //Add up all words that match the searchWord
            Parallel.ForEach(
                //source
                words,  
                //local init
                () => 0, 
                //body
                (string item, ParallelLoopState loopState, int tlsValue) => 
                {
                    if (item.ToLower().Equals(searchWord))
                    {
                        tlsValue++;
                    }
                    return tlsValue;
                },
                //local finally
                tlsValue =>
                {
                    lock (syncLock)
                    {
                        matches += tlsValue;
                    }
                });

            Console.WriteLine("Matches for searchword '{0}' : {1}\r\n", searchWord, matches);
            Console.WriteLine("Where the original word list was : \r\n\r\n{0}",
                words.Aggregate((x, y) => x.ToString() + " " + y.ToString()));
            Console.ReadLine();
        }
    }
}
