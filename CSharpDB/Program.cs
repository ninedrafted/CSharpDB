using System;
using System.Collections.Generic;

namespace CSharpDB {
    class Program {
        static void Main(string[] args) {
            DBTest cdb = new DBTest();
            Random r = new Random();
            for (int i = 0; i < 800; i++) {
                cdb.Add(r.Next(1000));
            }
            List<int> toRemove = new List<int>();
            for (int i = 0; i < 100; i++) {
                cdb.Add(r.Next(1000));
            }
            cdb.Add(5);
            cdb.Add(25);
            cdb.Add(7);
            cdb.Add(15);
            cdb.Add(18);
            cdb.Add(16);
            Console.WriteLine(cdb.Root.ToString());
            Console.WriteLine("IntegrityCheck:\t" + cdb.Root.IntegrityCheck());
            Console.WriteLine("WeightCheck:\t" + cdb.Root.WeightCheck());
            Console.ReadLine();
            Console.WriteLine("Select * WHERE int>15 AND int!=16 AND int!=45 AND int!=(some random numbers);");
            BTree<int> t = cdb.Root.GetGreaterAndEqual(15, false);
            Console.WriteLine("WeightCheck (>15):\t" + t.IntegrityCheck());
            t.RemoveAll(16);
            Console.WriteLine("WeightCheck (!16):\t" + t.IntegrityCheck());
            t.RemoveAll(45);
            Console.WriteLine("WeightCheck (!45):\t" + t.IntegrityCheck());
            for (int i = 0; i < toRemove.Count; i++) {
                t.RemoveAll(toRemove[i]);
            }
            Console.WriteLine("WeightCheck (!some random numbers):\t" + t.IntegrityCheck());
            List<DBIndex> list = t.GetSortedList();
            Console.WriteLine("WeightCheck:\t" + t.IntegrityCheck());
            Console.ReadLine();
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