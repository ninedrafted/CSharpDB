using System;
using System.Collections.Generic;

namespace CSharpDB {
    class Program {
        static void Main(string[] args) {
            DBTest cdb = new DBTest();
            Random r = new Random();
            for (int i = 10; i < 50; i++) {
                cdb.Add(r.Next(50));
            }
            cdb.Add(5);
            cdb.Add(25);
            cdb.Add(7);
            cdb.Add(15);
            cdb.Add(18);
            cdb.Add(16);
            Console.WriteLine(cdb.Root.ToString());
            Console.ReadLine();
            Console.WriteLine("Select * WHERE int>15 AND int!=16;");
            BTree<int> t = cdb.Root.GetGreaterAndEqual(15, false);
            t.RemoveAll(16);
            List<DBIndex> list = t.GetSortedList();
            int[] v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.WriteLine(v[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Select * WHERE int>=15");
            list = cdb.Root.GetGreaterAndEqual(15).GetSortedList();
            v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.WriteLine(v[i]);
            }
            Console.WriteLine();
            Console.WriteLine("Select * WHERE int>15 AND int <= 25");
            list = cdb.Root.GetGreaterAndEqual(15, false).GetLessAndEqual(25).GetSortedList();
            v = cdb.GetValues(list);
            for (int i = 0; i < v.Length; i++) {
                Console.WriteLine(v[i]);
            }
            Console.ReadLine();
        }
    }
}