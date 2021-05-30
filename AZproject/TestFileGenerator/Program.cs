using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace TestFileGenerator
{
    class Program
    {
        static string ussageString = "Invalid arguments!\n\n" +
                    "" +
                    "Ussage:\nFirst argument (required): output file\n" +
                    "Second argument(not required): number of formulas (default 10)\n" +
                    "Third argument(not required): number of variables in each formula (default 10)\n" +
                    "Fourth argument(not required): number of clauses in each formula (default 20)\n";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(ussageString);
                return;
            }
            int n = 10;
            int v = 10;
            int c = 20;
            if (args.Length > 1)
            {
                try
                {
                    n = int.Parse(args[1]);
                }
                catch
                {
                    Console.WriteLine(ussageString);
                    return;
                }
            }
            if (args.Length > 2)
            {
                try
                {
                    v = int.Parse(args[2]);
                }
                catch
                {
                    Console.WriteLine(ussageString);
                    return;
                }
            }
            if (args.Length == 4)
            {
                try
                {
                    c = int.Parse(args[3]);
                }
                catch
                {
                    Console.WriteLine(ussageString);
                    return;
                }
            }

            string stringResult = "";
            for (int i = 0; i < n; i++)
            {
                stringResult += Generator.GenerateFormula(v, c)+";\n";
            }

            try
            {
                File.WriteAllText(args[0], stringResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
