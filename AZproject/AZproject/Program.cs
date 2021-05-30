﻿using System;
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
            //if (args.Length != 2)
            //{
            //    Console.WriteLine("Invalid arguments!\n\nUssage:\nFirst argument: input file\nSecond argument: output file");
            //    return;
            //}
            //string text;
            //try
            //{
            //    text = File.ReadAllText(args[0]);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error: " + ex.Message);
            //    return;
            //}
            //if (text.Length == 0)
            //{
            //    Console.WriteLine("File is empty\n");
            //    return;
            //}
            //string[] formulas = text.Split(';');

            //Algorithm a = new Algorithm();
            //bool[] result;

            //string stringResult = "";

            //foreach (string formula in formulas)
            //    if (string.IsNullOrEmpty(formula.Trim()))
            //        continue;
            //    else if (a.Perform2SAT(formula, out result))
            //    {
            //        stringResult += "TRUE: ";
            //        foreach (var item in a.namesInd)
            //        {
            //            stringResult += $"({item.Key}:{result[item.Value].ToString().ToUpper()}), ";
            //        }
            //        stringResult = stringResult.Remove(stringResult.Length - 2, 2);
            //        stringResult += ";\n";
            //    }
            //    else
            //    {
            //        if (a.ErrorInFormula)
            //            stringResult += "Invalid formula;\n";
            //        else
            //            stringResult += "FALSE;\n";
            //    }

            //try
            //{
            //    File.WriteAllText(args[1], stringResult);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error: " + ex.Message);
            //    return;
            //}

            string test = "test";
            //for (int i = 1; i < 100; i++)
            //{
            //    string stringResult = "";
            //    for (int j = 0; j < 100; j++)
            //    {
            //        stringResult += Generator.GenerateFormula(100, 100 + i * 10) + ";\n";
            //    }
            //    File.WriteAllText(test + i + ".txt", stringResult);
            //}

            for (int i = 1; i < 100; i++)
            {
                string text = File.ReadAllText(test + i + ".txt");
                string[] formulas = text.Split(';');
                double sum = 0;
                int num = 0;
                foreach (var formula in formulas)
                {
                    Algorithm a = new Algorithm();
                    bool[] result;
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    a.Perform2SAT(formula, out result);
                    sw.Stop();
                    sum += sw.Elapsed.TotalSeconds;
                    num++;
                }
                File.AppendAllText("wyniki.txt", $"{100 + i * 10};{sum / num};\n");
                Console.WriteLine(i);
            }
        }
    }
}
