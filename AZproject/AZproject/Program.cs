using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace AZproject
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Invalid arguments!\n\nUssage:\nFirst argument: input file\nSecond argument: output file");
                return;
            }
            string text;
            try
            {
                text = File.ReadAllText(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }
            if(text.Length==0)
            {
                Console.WriteLine("File is empty\n");
                return;
            }
            string[] formulas = text.Split(';');

            Algorithm a = new Algorithm();
            bool[] result;

            string stringResult = "";

            foreach (string formula in formulas)
                if (a.Perform2SAT(formula, out result))
                {
                    stringResult += "TRUE: ";
                    foreach (var item in a.namesInd)
                    {
                        stringResult += $"({item.Key}:{result[item.Value]}), ";
                    }
                    stringResult = stringResult.Remove(stringResult.Length - 2, 2);
                    stringResult += ";\n";
                }
                else
                {
                    stringResult += "FALSE;\n";
                }

            try
            {
                File.WriteAllText(args[1], stringResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }

        }
    }
}
