using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDB {
    class DB {
        public DB(params string[] desc) {
            Trees = new BTree<IComparable>[desc.Length];
            Names = new string[desc.Length];
            Values = new List<List<IComparable>>[desc.Length];
            Indices = new List<List<DBIndex>>();
            Indices.Add(new List<DBIndex>());
            for (int i = 0; i < desc.Length; i++) {
                Values[i] = new List<List<IComparable>>();
                Values[i].Add(new List<IComparable>());
                Names[i] = desc[i];
            }
        }
        List<List<IComparable>>[] Values;
        int level = 0;
        List<List<DBIndex>> Indices;
        BTree<IComparable>[] Trees;
        string[] Names;
        bool hasroot = false;
        public void Add(bool optimize, params IComparable[] values) {
            if (this.Values.Length != values.Length) throw new Exception("To less values to add, this DB needs " + this.Values.Length);

            if (Values[0][level].Count == int.MaxValue) {
                if (level == int.MaxValue) throw new Exception("There's no more space left in this DB!");
                else level++;
                for (int i = 0; i < Values.Length; i++) {
                    Values[i].Add(new List<IComparable>());
                    Indices.Add(new List<DBIndex>());
                }
            }
            Indices[level].Add(new DBIndex(level, Values[0][level].Count));
            if (!hasroot) {
                for (int i = 0; i < Trees.Length; i++) {
                    Trees[i] = new BTree<IComparable>(ref Values[i], Indices[level][Indices[level].Count - 1]);
                }
                hasroot = true;
            }
            else {
                for (int i = 0; i < values.Length; i++) {
                    Values[i][level].Add(values[i]);
                    Trees[i].Add(Indices[level][Indices[level].Count - 1], optimize);
                }
            }
        }
        public void RotateAll(int times) {
            for (int i = 0; i < Trees.Length; i++) {
                for (int ii = 0; ii < times; ii++) {
                    Trees[i].RotateAll();
                }
            }
        }
        public bool WeightCheck() {
            for (int i = 0; i < Trees.Length; i++) {
                if (!Trees[i].WeightCheck())
                    return false;
            }
            return true;
        }
        private List<IComparable> GetRow(DBIndex index) {
            List<IComparable> ret = new List<IComparable>();
            for (int i = 0; i < Values.Length; i++) {
                ret.Add(Values[i][index.A][index.B]);
            }
            return ret;
        }
        public List<List<IComparable>> GetWhere(string name, IComparable equals) {
            List<List<IComparable>> ret = new List<List<IComparable>>();
            for (int i = 0; i < Names.Length; i++) {
                if (name == Names[i]) {
                    List<DBIndex> rows = Trees[i].GetRow(equals);
                    for (int ii = 0; ii < rows.Count; ii++) {
                        ret.Add(GetRow(rows[ii]));
                    }
                    break;
                }
            }
            return ret;
        }
    }
}
