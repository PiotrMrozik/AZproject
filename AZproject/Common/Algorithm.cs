using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Algorithm
    {
        public Dictionary<string, int> namesInd;
        public bool ErrorInFormula = false;

        private bool[] used;
        private List<HashSet<int>> G;
        private List<HashSet<int>> GT;
        private int index;
        private int[] order;
        private int[] sccindex;
        private bool[] assign;
        private int n;

        public bool Perform2SAT(string formula, out bool[] Assign)
        {
            Assign = null;
            CreateGraphs(formula);
            if (n == -1)
            {
                ErrorInFormula = true;
                return false;
            }
            else
                ErrorInFormula = false;

            used = new bool[2 * n];
            order = new int[2 * n];
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
                if (sccindex[v] == -1)
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
            assign.CopyTo(Assign, 0);
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

        public void CreateGraphs(string formula)
        {
            n = 0;
            G = new List<HashSet<int>>();
            GT = new List<HashSet<int>>();
            namesInd = new Dictionary<string, int>();
            string[] split = formula.Split('&');
            foreach (string bracket in split)
            {
                string temp = bracket.Trim();
                if (temp.Length < 3 || temp[0] != '(' || temp[temp.Length - 1] != ')')
                {
                    n = -1;
                    return;
                }
                if (temp.Contains('|'))
                {
                    temp = temp.Remove(temp.Length - 1, 1).Remove(0, 1).Trim();
                    string[] tempsplit = temp.Split('|');
                    if (tempsplit.Length != 2)
                    {
                        n = -1;
                        return;
                    }
                    tempsplit[0] = tempsplit[0].Trim();
                    tempsplit[1] = tempsplit[1].Trim();
                    bool[] negated = new bool[2];
                    if (tempsplit[0][0] == '~')
                    {
                        negated[0] = true;
                        tempsplit[0] = tempsplit[0].Remove(0, 1);
                    }
                    if (tempsplit[1][0] == '~')
                    {
                        negated[1] = true;
                        tempsplit[1] = tempsplit[1].Remove(0, 1);
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(tempsplit[0], "^[a-zA-Z][a-zA-Z1-9]*$") || !System.Text.RegularExpressions.Regex.IsMatch(tempsplit[1], "^[a-zA-Z][a-zA-Z1-9]*$"))
                    {
                        n = -1;
                        return;
                    }
                    AddVariable(tempsplit[0]);
                    AddVariable(tempsplit[1]);
                    AddEdges(tempsplit[0], tempsplit[1], negated[0], negated[1]);
                }
                else
                {
                    temp = temp.Remove(temp.Length - 1, 1).Remove(0, 1).Trim();
                    bool negated = false;
                    if (temp[0] == '~')
                    {
                        negated = true;
                        temp = temp.Remove(0, 1);
                    }
                    if (!System.Text.RegularExpressions.Regex.IsMatch(temp, "^[a-zA-Z][a-zA-Z1-9]*$"))
                    {
                        n = -1;
                        return;
                    }
                    AddVariable(temp);
                    if (negated)
                    {
                        if (!G[namesInd[temp] * 2].Contains(namesInd[temp] * 2 + 1))
                        {
                            G[namesInd[temp] * 2].Add(namesInd[temp] * 2 + 1);
                            GT[namesInd[temp] * 2 + 1].Add(namesInd[temp] * 2);
                        }
                    }
                    else if (!G[namesInd[temp] * 2 + 1].Contains(namesInd[temp] * 2))
                    {
                        G[namesInd[temp] * 2 + 1].Add(namesInd[temp] * 2);
                        GT[namesInd[temp] * 2].Add(namesInd[temp] * 2 + 1);
                    }
                }
            }
        }

        private bool AddVariable(string v)
        {
            if (!namesInd.ContainsKey(v))
            {
                namesInd.Add(v, n);
                G.Add(new HashSet<int>());
                G.Add(new HashSet<int>());
                GT.Add(new HashSet<int>());
                GT.Add(new HashSet<int>());
                n++;
                return true;
            }
            return false;
        }

        private void AddEdges(string a, string b, bool aneg, bool bneg)
        {
            int an = aneg ? 1 : 0;
            int bn = bneg ? 1 : 0;
            if (!G[namesInd[a] * 2 + 1 - an].Contains(namesInd[b] * 2 + bn))
            {
                G[namesInd[a] * 2 + 1 - an].Add(namesInd[b] * 2 + bn);
                GT[namesInd[b] * 2 + bn].Add(namesInd[a] * 2 + 1 - an);
            }
            if (!G[namesInd[b] * 2 + 1 - bn].Contains(namesInd[a] * 2 + an))
            {
                G[namesInd[b] * 2 + 1 - bn].Add(namesInd[a] * 2 + an);
                GT[namesInd[a] * 2 + an].Add(namesInd[b] * 2 + 1 - bn);
            }
        }
    }
}
