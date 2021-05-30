using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Generator
    {
        private static Random rand;

        /*Formula used to generate CNF formulas.
          /param[i] number of different symbols in formula
          /param[i] number of clauses in equation
          /return string with CNF formula
        */
        public static string GenerateFormula(int numberOfSymbols, int clauses)
        {
            char[] symbols=new char[numberOfSymbols];
            string result = "";
            rand = new Random(Guid.NewGuid().GetHashCode());
            //generate dictionary
            for (int i=0; i< numberOfSymbols; ++i)
            {
                symbols[i]=(char)(97+i);
            }

          
            //generate formula
            for (int i = 0; i< clauses; ++i)
            {
                result += '(';

                //randomize first negation
                if (rand.Next(2)==0)
                {
                    result += '~';
                }

                //randomize first symbol
                result += symbols[rand.Next(numberOfSymbols)];               

                result += '|';

                //randomize second negation
                if (rand.Next(2) == 0)
                {
                    result += '~';
                }

                //randomize second symbol
                result += symbols[rand.Next(numberOfSymbols)];

                result += ")&";
            }
            return result.Remove(result.Length - 1);
        }

    }
}

