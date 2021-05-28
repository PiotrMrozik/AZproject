using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace AZproject
{
    class Program
    {
        static void Main(string[] args)
        {
            Algorithm a = new Algorithm();
            bool[] dudu;
            var du = a.Perform2SAT("(a|b)&(b)&(c|b)&(a|~c)&(~a|~c)&(~a|c)&(c|c)", out dudu); // TODO: niepoprawne zachowanie dla (a|a) poprawić
            int b = 1 + 2;
        }
    }
}
