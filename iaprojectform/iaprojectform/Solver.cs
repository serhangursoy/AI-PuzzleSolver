using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iaprojectform
{
    class Solver
    {
        public int[] startposRow;
        public int[] startposCol;
        public int[] lengthforRow;
        public int[] lengthforCol;
        public List<string> Row;
        public List<string> Col;
        public char[,] resultTable;
        public int max_res;
        public void printp(char[,] p)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                    Console.Write(p[i, j]+" ");
                Console.WriteLine();
            }
            Console.WriteLine("******************");
        }

        public void rec(int n, char[,] p, int point)
        {
            if (point < max_res / 2)
                return;
            //printp(p);
            if(n == 10)
            {
                int count = 25;
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        if (p[i, j] == '1' || p[i, j] == 0)
                            count--;
                if(count > max_res)
                {
                    max_res = count;
                    for (int i = 0; i < 5; i++)
                        for (int j = 0; j < 5; j++)
                            if (p[i, j] == '0')
                                resultTable[i, j] = '*';
                            else
                                resultTable[i, j] = p[i, j];
                    printp(p);
                }
                //printp(p);
                return;
            }
            int x = n / 2;
            Parser temp = new Parser();
            char[] backup = new char[5];
            if (n%2 == 0) // satira koyma
            {
                int blank = Row[x].Split(' ').Length;
                if(blank == 1)
                {
                    List<string> t = temp.GetSynonym(Row[x]);
                    for(int i=0;i<t.Count;i++)
                    {
                        if(t[i].Length == lengthforRow[x])
                        {
                            int match = 0;
                            int m2 = 0;
                            for(int j=0;j<lengthforRow[x];j++)
                            {
                                if (p[x, j + startposRow[x]] == t[i][j] || p[x, j + startposRow[x]] == '0')
                                    match++;
                                if (p[x, j + startposRow[x]] == '0')
                                    m2++;
                            }
                            if(match == lengthforRow[x])
                            {
                                for (int j = 0; j < 5; j++) backup[j] = p[x, j];
                                for (int j = 0; j < lengthforRow[x]; j++) p[x, j+startposRow[x]] = t[i][j];
                                rec(n + 1, p, point+m2);
                                for (int j = 0; j < 5; j++) p[x, j] = backup[j];
                            }
                        }
                    }
                    rec(n + 1, p, point);
                }
                else
                {
                    Dictionary<string, int> t = temp.GetFromGoogle(Row[x]);
                    for(int i=0;i<30;i++)
                    {
                        string str = t.Keys.ElementAt(i);
                        if(str.Length == lengthforRow[x])
                        {
                            int match = 0;
                            int m2 = 0;
                            for (int j = 0; j < lengthforRow[x]; j++)
                            {
                                if (p[x, j + startposRow[x]] == str[j] || p[x, j + startposRow[x]] == '0')
                                    match++;
                                if (p[x, j + startposRow[x]] == '0')
                                    m2++;
                            }
                            if(match == lengthforRow[x])
                            {
                                for (int j = 0; j < 5; j++) backup[j] = p[x, j];
                                for (int j = 0; j < lengthforRow[x]; j++) p[x, j + startposRow[x]] = str[j];
                                rec(n + 1, p, point+m2);
                                for (int j = 0; j < 5; j++) p[x, j] = backup[j];
                            }
                        }
                    }
                    rec(n + 1, p, point);
                }
            }
            else
            {
                int blank = Col[x].Split(' ').Length;
                if(blank == 1)
                {
                    List<string> t = temp.GetSynonym(Col[x]);
                    for(int i=0;i<20;i++)
                    {
                        if(t[i].Length == lengthforCol[x])
                        {
                            int match = 0;
                            int m2 = 0;
                            for(int j=0;j<lengthforCol[x];j++)
                            {
                                if (p[j + startposCol[x], x] == t[i][j] || p[j + startposCol[x], x] == '0')
                                    match++;
                                if (p[j + startposCol[x], x] == '0')
                                    m2++;
                            }
                            if(match == lengthforCol[x])
                            {
                                for (int j = 0; j < 5; j++) backup[j] = p[j, x];
                                for (int j = 0; j < lengthforCol[x]; j++) p[j + startposCol[x], x] = t[i][j];
                                rec(n+1,p,point+m2);
                                for (int j = 0; j < 5; j++) p[j, x] = backup[j];
                            }
                        }
                    }
                    rec(n + 1, p, point);
                }
                else
                {
                    Dictionary<string, int> t = temp.GetFromGoogle(Col[x]);
                    for (int i = 0; i < t.Count; i++)
                    {
                        string str = t.Keys.ElementAt(i);
                        if (str.Length == lengthforCol[x])
                        {
                            int match = 0;
                            int m2 = 0;
                            for (int j = 0; j < lengthforCol[x]; j++)
                            {
                                if (p[j + startposCol[x], x] == str[j] || p[j + startposCol[x], x] == '0')
                                    match++;
                                if (p[j + startposCol[x], x] == '0')
                                    m2++;
                            }
                            if (match == lengthforCol[x])
                            {
                                for (int j = 0; j < 5; j++) backup[j] = p[j, x];
                                for (int j = 0; j < lengthforCol[x]; j++) p[j + startposCol[x], x] = str[j];
                                rec(n + 1, p, point+m2);
                                for (int j = 0; j < 5; j++) p[j, x] = backup[j];
                            }
                        }
                    }
                    rec(n + 1, p, point);
                }
            }
        }

        public char[,] Solve(char[,] table, List<string> rowQ, List<string> colQ)
        {
            Console.WriteLine("----------Start of Solver-----------");
            Row = rowQ;
            Col = colQ;
            max_res = 0;
            resultTable = new char[5, 5];
            startposRow = new int[5];
            startposCol = new int[5];
            lengthforRow = new int[5];
            lengthforCol = new int[5];
            printp(table);
            for (int i=0;i<5;i++)
            {
                bool f = false;
                for(int j=0;j<5;j++)
                {
                    if(f == false && table[i,j] == '0')
                    {
                        startposRow[i] = j;
                        f = true;
                    }
                    if (table[i, j] == '1' && f == true)
                        break;
                    if (f)
                        lengthforRow[i]++;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                bool f = false;
                for (int j = 0; j < 5; j++)
                {
                    if (f == false && table[j, i] == '0')
                    {
                        startposCol[i] = j;
                        f = true;
                    }
                    if (table[j, i] == '1' && f == true)
                        break;
                    if (f)
                        lengthforCol[i]++;
                }
            }
            // kontrol etme baslangic ve uzunluk
            // for (int i = 0; i < 5; i++)  Console.WriteLine("RS: " + startposRow[i] + " RL: " + lengthforRow[i] + " CS: " + startposCol[i] + " CL: " + lengthforCol[i]);    

            Console.WriteLine("*************start of rec***************");
            rec(0, table, 0);
           /* Parser t = new Parser();
            Dictionary<string, int> tt = t.GetFromGoogle(Row[4]);
            for(int i=0;i<tt.Count;i++)
            {
                Console.WriteLine(tt.Keys.ElementAt(i));
            }
            Console.WriteLine(Col[4]);*/
            printp(resultTable);
            return resultTable;
        }
    }
}
