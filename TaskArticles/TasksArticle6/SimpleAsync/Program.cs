using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Start();
        }

        private async void Start()
        {
            Console.WriteLine("*** BEFORE CALL ***");
            String chungles = await GetString();
            Console.WriteLine("*** AFTER CALL ***");
            Console.WriteLine("result = " + chungles);
            Console.ReadLine();
        }


        private async Task<string> GetString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hello world");
            return sb.ToString();
        }
    }
}
