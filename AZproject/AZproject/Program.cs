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
            var du = a.Perform2SAT("(a|b)&(~b|~b)&(c|b)&(c|c)", out dudu);
        }
    }
}
