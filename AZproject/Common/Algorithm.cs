using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class Algorithm
    {
        private bool[] used;
        public List<List<int>> G;
        public List<List<int>> GT;
        private int index;
        private int[] order;
        private int[] sccindex;
        private bool[] assign;
        public Dictionary<string, int> namesInd;
        public int n;

        public bool Perform2SAT(string formula, out bool[] Assign)
        {
            Assign = null;
            CreateGraphs(formula);
            if (n == -1)
                return false;
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
            G = new List<List<int>>();
            GT = new List<List<int>>();
            namesInd = new Dictionary<string, int>();
            string[] split = formula.Split('&');
            foreach (string bracket in split)
            {
                string temp = bracket.Trim();
                if (temp[0] != '(' || temp[temp.Length - 1] != ')')
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
                    AddVariable(tempsplit[0]);
                    AddVariable(tempsplit[1]);

                    if (negated[0] && negated[1])
                    {
                        if (!G[namesInd[tempsplit[0]] * 2].Contains(namesInd[tempsplit[1]] * 2 + 1))
                        {
                            G[namesInd[tempsplit[0]] * 2].Add(namesInd[tempsplit[1]] * 2 + 1);
                            GT[namesInd[tempsplit[1]] * 2 + 1].Add(namesInd[tempsplit[0]] * 2);
                        }
                        if (!G[namesInd[tempsplit[1]] * 2].Contains(namesInd[tempsplit[0]] * 2 + 1))
                        {
                            G[namesInd[tempsplit[1]] * 2].Add(namesInd[tempsplit[0]] * 2 + 1);
                            GT[namesInd[tempsplit[0]] * 2 + 1].Add(namesInd[tempsplit[1]] * 2);
                        }
                    }
                    else if (negated[0])
                    {
                        if (!G[namesInd[tempsplit[0]] * 2].Contains(namesInd[tempsplit[1]] * 2))
                        {
                            G[namesInd[tempsplit[0]] * 2].Add(namesInd[tempsplit[1]] * 2);
                            GT[namesInd[tempsplit[1]] * 2].Add(namesInd[tempsplit[0]] * 2);
                        }
                        if (!G[namesInd[tempsplit[1]] * 2 + 1].Contains(namesInd[tempsplit[0]] * 2 + 1))
                        {
                            G[namesInd[tempsplit[1]] * 2 + 1].Add(namesInd[tempsplit[0]] * 2 + 1);
                            GT[namesInd[tempsplit[0]] * 2 + 1].Add(namesInd[tempsplit[1]] * 2 + 1);
                        }
                    }
                    else if (negated[1])
                    {
                        if (!G[namesInd[tempsplit[0]] * 2 + 1].Contains(namesInd[tempsplit[1]] * 2 + 1))
                        {
                            G[namesInd[tempsplit[0]] * 2 + 1].Add(namesInd[tempsplit[1]] * 2 + 1);
                            GT[namesInd[tempsplit[1]] * 2 + 1].Add(namesInd[tempsplit[0]] * 2 + 1);
                        }
                        if (!G[namesInd[tempsplit[1]] * 2].Contains(namesInd[tempsplit[0]] * 2))
                        {
                            G[namesInd[tempsplit[1]] * 2].Add(namesInd[tempsplit[0]] * 2);
                            GT[namesInd[tempsplit[0]] * 2].Add(namesInd[tempsplit[1]] * 2);
                        }
                    }
                    else
                    {
                        if (!G[namesInd[tempsplit[0]] * 2 + 1].Contains(namesInd[tempsplit[1]] * 2))
                        {
                            G[namesInd[tempsplit[0]] * 2 + 1].Add(namesInd[tempsplit[1]] * 2);
                            GT[namesInd[tempsplit[1]] * 2].Add(namesInd[tempsplit[0]] * 2 + 1);
                        }
                        if(!G[namesInd[tempsplit[1]] * 2 + 1].Contains(namesInd[tempsplit[0]] * 2))
                        { 
                            G[namesInd[tempsplit[1]] * 2 + 1].Add(namesInd[tempsplit[0]] * 2);
                            GT[namesInd[tempsplit[0]] * 2].Add(namesInd[tempsplit[1]] * 2 + 1);
                        }
                    }
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
                G.Add(new List<int>());
                G.Add(new List<int>());
                GT.Add(new List<int>());
                GT.Add(new List<int>());
                n++;
                return true;
            }
            return false;
        }
    }
}
