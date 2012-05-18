using System;
using System.Collections.Generic;

namespace CSharpDB {
    public class BTree<T1> where T1 : IComparable {
        //TODO: check weight before adding to rearrange the tree if senseful
        private long weight = 1;
        public BTree() { Equal = new List<DBIndex>(); }
        public BTree(ref List<List<T1>> column, DBIndex row) {
            this.Column = column;
            this.Row = row;
            Equal = new List<DBIndex>();
        }
        private BTree<T1> GetMax() {
            if (Greater == null) return this;
            else return Greater.GetMax();
        }
        private BTree<T1> GetMin() {
            if (Less == null) return this;
            else return Less.GetMin();
        }
        private void _rotate() {
            if (Greater == null && Less == null) return;
            BTree<T1> tmp = new BTree<T1>();
            tmp.Row = Row;
            tmp.Column = Column;
            tmp.Equal = Equal;
            tmp.resetWeight();
            if (Greater == null && Less.weight > tmp.weight * 2) {
                this.Row = Less.Row;
                this.Equal = Less.Equal;
                this.Greater = Less.Greater;
                this.Less = Less.Less;
                if (this.Greater != null) this.Greater._Add(tmp);
                else this.Greater = tmp;
                resetWeight();
                return;
            }
            if (Less == null && Greater.weight > tmp.weight * 2) {
                this.Row = Greater.Row;
                this.Equal = Greater.Equal;
                this.Less = Greater.Less;
                this.Greater = Greater.Greater;
                if (this.Less != null) this.Less._Add(tmp);
                else this.Less = tmp;
                resetWeight();
                return;
            }
            if (Greater != null && Less != null) {
                if (Greater.weight > Less.weight + tmp.weight * 2) {
                    this.Row = Greater.Row;
                    this.Equal = Greater.Equal;
                    if (this.Less != null) this.Less._Add(Greater.Less);
                    else this.Less = Greater.Less;
                    this.Greater = Greater.Greater;
                    if (this.Less != null) this.Less._Add(tmp);
                    else this.Less = tmp;
                    resetWeight();
                    return;
                }
                else if (Less.weight > Greater.weight + tmp.weight * 2) {
                    this.Row = Less.Row;
                    this.Equal = Less.Equal;
                    if (this.Greater != null) this.Greater._Add(Less.Greater);
                    else this.Greater = Less.Greater;
                    this.Less = Less.Less;
                    if (this.Greater != null) this.Greater._Add(tmp);
                    else this.Greater = tmp;
                    resetWeight();
                    return;
                }
            }
        }
        private void _Add(BTree<T1> tree, bool optimize = true) {
            if (tree == null) return;
            weight += tree.weight;
            int c = tree.CompareTo(this);
            if (c == 0) {
                if (Greater != null) {
                    Greater._Add(tree.Greater);
                }
                else Greater = tree.Greater;
                if (Less != null) {
                    Less._Add(tree.Less);
                }
                else Less = tree.Less;
                Equal.Add(tree.Row);
                if (optimize) _rotate();
                return;
            }
            if (tree.GetMin().CompareTo(this) == 1) {
                if (c > 0) {
                    if (Greater != null) Greater._Add(tree);
                    else Greater = tree;
                    if (optimize) _rotate();
                    return;
                }
            }
            else if (tree.GetMax().CompareTo(this) == -1) {
                if (c < 0) {
                    if (Less != null) Less._Add(tree);
                    else Less = tree;
                    if (optimize) _rotate();
                    return;
                }
            }
            else {
                Console.WriteLine("Tree does not fit in here!\n" + this.ToString() + "\nTree: " + tree.ToString());
            }
        }
        public void Add(DBIndex row) {
            BTree<T1> toAdd = new BTree<T1>(ref Column, row);
            _Add(toAdd);
            return;
        }
        public List<DBIndex> GetRow(T1 item) {
            int c = item.CompareTo(Column[this.Row.A][this.Row.B]);
            if (c == 0) {
                return GetThisRow();
            }
            if (c < 0) {
                if (Less == null)
                    return new List<DBIndex>();
                return Less.GetRow(item);
            }
            if (c > 0) {
                if (Greater == null)
                    return new List<DBIndex>();
                return Greater.GetRow(item);
            }
            return null;
        }
        private List<DBIndex> GetThisRow() {
            List<DBIndex> ret = new List<DBIndex>();
            ret.Add(Row);
            ret.AddRange(Equal);
            return ret;
        }
        public BTree<T1> GetGreaterAndEqual(T1 item, bool equal = true) {
            int c = item.CompareTo(Column[this.Row.A][this.Row.B]);
            if (c < 0 || c == 0 && equal) {
                BTree<T1> ret = new BTree<T1>(ref Column, Row);
                ret.Equal = this.Equal;
                ret.Greater = this.Greater;
                if (c < 0 && Less != null)
                    ret._Add(Less.GetGreaterAndEqual(item, equal), false);
                return ret.Clone();
            }
            if ((c > 0 || c == 0 && !equal) && Greater != null) {
                return Greater.GetGreaterAndEqual(item, equal);
            }
            return null;
        }
        public BTree<T1> GetLessAndEqual(T1 item, bool equal = true) {
            int c = item.CompareTo(Column[this.Row.A][this.Row.B]);
            if (c > 0 || c == 0 && equal) {
                BTree<T1> ret = new BTree<T1>(ref Column, Row);
                ret.Equal = this.Equal;
                ret.Less = this.Less;
                if (c > 0 && Greater != null) {
                    ret._Add(Greater.GetLessAndEqual(item, equal));
                }
                return ret.Clone();
            }
            if ((c < 0 || c == 0 && !equal) && Less != null) {
                return Less.GetLessAndEqual(item, equal);
            }
            return null;
        }
        public bool WeightCheck() {
            long w = 1 + Equal.Count;
            bool ret = true;
            if (Greater != null) {
                w += Greater.weight;
                ret = Greater.WeightCheck();
            }
            if (Less != null) {
                w += Less.weight;
                ret = Less.WeightCheck() && ret;
            }
            ret = w == weight && ret;
            if (!ret)
                Console.WriteLine("0.0");
            return ret;
        }
        private int _integrityCheck(T1 value) {
            int c = value.CompareTo(GetItem());
            if (Greater != null) {
                int tmp = value.CompareTo(Greater.GetItem());
                if (tmp != c) {
                    Console.WriteLine("Disordered:\t" + this.ToString());
                    return 0;
                }
                tmp = Greater._integrityCheck(GetItem());
                if (tmp != -1) {
                    Console.WriteLine("Disordered:\t" + this.ToString());
                    return 0;
                }
            }
            if (Less != null) {
                int tmp = value.CompareTo(Less.GetItem());
                if (tmp != c) {
                    Console.WriteLine("Disordered:\t" + this.ToString());
                    return 0;
                }
                tmp = Less._integrityCheck(GetItem());
                if (tmp != 1) {
                    Console.WriteLine("Disordered:\t" + this.ToString());
                    return 0;
                }
            }
            return c;
        }
        public bool IntegrityCheck() {
            bool ret = Greater == null || Greater._integrityCheck(GetItem()) == -1;
            ret = (Less == null || Less._integrityCheck(GetItem()) == 1) && ret;
            return ret;
        }
        public List<DBIndex> GetSortedList(bool inverse = false) {
            List<DBIndex> ret = new List<DBIndex>();
            if (inverse && Greater != null) ret.AddRange(Greater.GetSortedList(inverse));
            else if (!inverse && Less != null) ret.AddRange(Less.GetSortedList(inverse));
            ret.Add(Row);
            ret.AddRange(Equal);
            if (inverse && Less != null) ret.AddRange(Less.GetSortedList(inverse));
            else if (!inverse && Greater != null) ret.AddRange(Greater.GetSortedList(inverse));
            return ret;
        }
        private void resetWeight() {
            this.weight = ((Greater != null) ? Greater.weight : 0) + ((Less != null) ? Less.weight : 0) + Equal.Count + 1;
        }
        private bool _RemoveThis() {
            if (Greater != null && Less != null) {
                if (Greater.weight >= Less.weight) {
                    this.Row = Greater.Row;
                    this.Equal = new List<DBIndex>(Greater.Equal);
                    BTree<T1> tmpTreeLess = Greater.Less;
                    Greater = Greater.Greater;
                    _Add(tmpTreeLess);
                    resetWeight();
                    if (!IntegrityCheck()) {
                        T1 v = GetItem();
                        v = v;
                    }
                    return false;
                }
                else {
                    this.Row = Less.Row;
                    this.Equal = new List<DBIndex>(Less.Equal);
                    BTree<T1> tmpTreeGreater = Less.Greater;
                    Less = Less.Less;
                    _Add(tmpTreeGreater);
                    resetWeight();
                    if (!IntegrityCheck()) {
                        T1 v = GetItem();
                        v = v;
                    }
                    return false;
                }
            }
            if (Greater != null) {
                this.Row = Greater.Row;
                this.Equal = new List<DBIndex>(Greater.Equal);
                BTree<T1> tmpTreeLess = Greater.Less;
                Greater = Greater.Greater;
                _Add(tmpTreeLess);
                resetWeight();
                if (!IntegrityCheck()) {
                    T1 v = GetItem();
                    v = v;
                }
                return false;
            }
            if (Less != null) {
                this.Row = Less.Row;
                this.Equal = new List<DBIndex>(Less.Equal);
                BTree<T1> tmpTreeGreater = Less.Greater;
                Less = Less.Less;
                _Add(tmpTreeGreater);
                resetWeight();
                if (!IntegrityCheck()) {
                    T1 v = GetItem();
                    v = v;
                }
                return false;
            }
            return true;
        }
        private bool _RemoveAll(T1 item) {
            int c = item.CompareTo(Column[this.Row.A][this.Row.B]);
            if (c == 0) {
                return _RemoveThis();
            }
            if (c > 0) {
                if (Greater != null) {
                    if (Greater._RemoveAll(item)) {
                        Greater = null;
                    }
                    resetWeight();
                }
                return false;
            }
            if (c < 0) {
                if (Less != null) {
                    if (Less._RemoveAll(item)) {
                        Less = null;
                    }
                    resetWeight();
                }
                return false;
            }
            return !(false || true);
        }
        public BTree<T1> RemoveAll(T1 item) {
            if (_RemoveAll(item))
                return null;
            else return this;
        }
        private bool _Remove(DBIndex row) {
            int c = Column[row.A][row.B].CompareTo(Column[this.Row.A][this.Row.B]);
            if (c == 0) {
                if (Greater == null && Less == null && Equal.Count == 0)
                    return true;
                if (Equal.Count != 0) {
                    this.Row = Equal[0];
                    Equal.RemoveAt(0);
                    weight--;
                    return false;
                }
                return _RemoveThis();
            }
            if (c > 0) {
                if (Greater != null) {
                    if (Greater._Remove(row)) {
                        Greater = null;
                    }
                    resetWeight();
                }
                return false;
            }
            if (c < 0) {
                if (Less != null) {
                    if (Less._Remove(row)) {
                        Less = null;
                    }
                    resetWeight();
                }
                return false;
            }
            return !(false || true);
        }
        public BTree<T1> Remove(DBIndex row) {
            if (_Remove(row))
                return null;
            else return this;
        }
        List<List<T1>> Column;
        DBIndex Row;
        BTree<T1> Less;
        List<DBIndex> Equal;
        BTree<T1> Greater;
        public BTree<T1> Clone() {
            BTree<T1> ret = new BTree<T1>();
            ret.Row = Row; //Keeps being a reference
            ret.Equal = new List<DBIndex>(Equal);
            ret.Column = Column; //Keeps being a reference
            if (Less != null)
                ret.Less = Less.Clone();
            if (Greater != null)
                ret.Greater = Greater.Clone();
            ret.resetWeight();
            return ret;

        }
        public T1 GetItem() {
            return Column[Row.A][Row.B];
        }
        public string ToString(string lvl = ":") {
            string ret = "";
            if (Less != null) {
                ret += Less.ToString(lvl + ":");
                ret += lvl + Less.GetItem();
            }
            else {
                ret += lvl + "null";
            }
            ret += " < " + GetItem() + " < ";
            if (Greater != null) {
                ret += Greater.GetItem();
                ret += "\n";
                ret += Greater.ToString(lvl + ":");
            }
            else ret += "null\n";
            return ret;
        }
        public int CompareTo(BTree<T1> obj) {
            return Column[Row.A][Row.B].CompareTo(Column[obj.Row.A][obj.Row.B]);
        }
    }
}
