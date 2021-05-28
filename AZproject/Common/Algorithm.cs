using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Algorithm
    {
        private bool[] used;
        private List<List<int>> G;
        private List<List<int>> GT;
        private int index;
        private int[] order;
        private int[] sccindex;
        private bool[] assign;

        public bool Perform2SAT(string formula, out bool[] Assign)
        {
            Assign = null;
            int n = CreateGraphs(formula);
            if (n == -1)
                return false;
            used = new bool[2 * n];
            index = 0;
            for (int i = 0; i < 2 * n; i++)
                if (!used[i])
                    DFSOrder(i);

            sccindex = new int[2 * n];
            for (int i = 0; i < 2 * n; i++)
                sccindex[i] = -1;

            int j = 0;
            for (int i = 0; i < 2 * n; i++)
            {
                int v = order[2 * n - i - 1];
                if(sccindex[v]==-1)
                {
                    TopoOrderSCC(v, j);
                    j++;
                }
            }

            assign = new bool[n];
            for (int i = 0; i < 2 * n; i += 2)
            { 
                if (sccindex[i] == sccindex[i + 1])
                    return false;
                assign[i / 2] = sccindex[i] > sccindex[i + 1];
            }
            Assign = new bool[n];
            assign.CopyTo(Assign,0);
            return true;
        }

        private void DFSOrder(int v)
        {
            used[v] = true;
            foreach (int u in G[v])
                if (!used[u])
                    DFSOrder(u);
            order[index] = v;
            index++;
        }

        private void TopoOrderSCC(int v, int ind)
        {
            sccindex[v] = ind;
            foreach (int u in GT[v])
                if (sccindex[u] == -1)
                    TopoOrderSCC(u, ind);
        }

        private int CreateGraphs(string formula)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(formula, ""))
                return -1;

            return 10;
        }
    }
}
