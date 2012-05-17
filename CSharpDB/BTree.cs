using System;
using System.Collections.Generic;

namespace CSharpDB{
    public class BTree<T1> where T1 : IComparable {
        //TODO: measure trees weight to use heavier tree as replacement when current is removed
        private long weight = 0;
        public BTree() { Equal = new List<DBIndex>(); }
        public BTree(ref List<List<T1>> column, DBIndex row) {
            this.Column = column;
            this.Row = row;
            Equal = new List<DBIndex>();
        }
        private void _Add(BTree<T1> tree) {
            if (tree == null) return;
            weight += tree.weight;
            int c = Column[tree.Row.A][tree.Row.B].CompareTo(Column[this.Row.A][this.Row.B]);
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
            }
            if (c > 0) {
                if (Greater != null) Greater._Add(tree);
                else Greater = tree;
                return;
            }
            if (c < 0) {
                if (Less != null) Less._Add(tree);
                else Less = tree;
                return;
            }
        }
        public void Add(DBIndex row) {
            if (row.A >= Column.Count || row.B >= Column[row.A].Count)
                throw new Exception("row out of bounds in Btree<T1>.Add(long row)");
            int c = Column[row.A][row.B].CompareTo(Column[this.Row.A][this.Row.B]);
            if (c < 0) {
                if (Less == null) {
                    Less = new BTree<T1>(ref Column, row);
                }
                else {
                    Less.Add(row);
                }
                weight++;
                return;
            }
            if (c == 0) {
                Equal.Add(row);
                weight++;
            }
            if (c > 0) {
                if (Greater == null) {
                    Greater = new BTree<T1>(ref Column, row);
                }
                else {
                    Greater.Add(row);
                }
                weight++;
                return;
            }
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
                    ret._Add(Less.GetGreaterAndEqual(item, equal));
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
        public List<DBIndex> GetSortedList(bool inverse = false) {
            List<DBIndex> ret = new List<DBIndex>();
            if (inverse && Greater != null) ret.AddRange(Greater.GetSortedList(inverse));
            else if (Less != null) ret.AddRange(Less.GetSortedList(inverse));
            ret.Add(Row);
            ret.AddRange(Equal);
            if (inverse && Less != null) ret.AddRange(Less.GetSortedList(inverse));
            else if (Greater != null) ret.AddRange(Greater.GetSortedList(inverse));
            return ret;
        }
        private void resetWeight() {
            this.weight = ((Greater != null) ? Greater.weight : 0) + ((Less != null) ? Less.weight : 0) + Equal.Count + 1;
        }
        private bool _RemoveAll(T1 item) {
            int c = item.CompareTo(Column[this.Row.A][this.Row.B]);
            if (c == 0) {
                if (Greater != null && Less != null) {
                    if (Greater.weight >= Less.weight) {
                        this.Row = Greater.Row;
                        this.Equal = Greater.Equal;
                        if (Greater.Greater.weight >= Greater.Less.weight) {
                            BTree<T1> tmpTreeLess = Greater.Less;
                            Greater = Greater.Greater;
                            if (Greater != null) Greater._Add(tmpTreeLess);
                            else Greater = tmpTreeLess;
                        }
                        else {
                            BTree<T1> tmpTreeGreater = Greater.Greater;
                            Greater = Greater.Less;
                            if (Greater != null) Greater._Add(tmpTreeGreater);
                            else Greater = tmpTreeGreater;
                        }
                        resetWeight();
                        return false;
                    }
                    else {
                        this.Row = Less.Row;
                        this.Equal = Less.Equal;
                        if (Less.Greater.weight >= Less.Less.weight) {
                            BTree<T1> tmpTreeLess = Less.Less;
                            Less = Less.Greater;
                            if (Less != null) Less._Add(tmpTreeLess);
                            else Less = tmpTreeLess;
                        }
                        else {
                            BTree<T1> tmpTreeGreater = Less.Greater;
                            Less = Less.Less;
                            if (Less != null) Less._Add(tmpTreeGreater);
                            else Less = tmpTreeGreater;
                        }
                        resetWeight();
                        return false;
                    }
                }
                if (Greater != null) {
                    this.Row = Greater.Row;
                    this.Equal = Greater.Equal;
                    if (Greater.Greater != null && Greater.Less != null && Greater.Greater.weight >= Greater.Less.weight) {
                        BTree<T1> tmpTreeLess = Greater.Less;
                        Greater = Greater.Greater;
                        if (Greater != null) Greater._Add(tmpTreeLess);
                        else Greater = tmpTreeLess;
                    }
                    else {
                        if (Greater.Greater != null && Greater.Greater != null) {
                            BTree<T1> tmpTreeGreater = Greater.Greater;
                            Greater = Greater.Less;
                            if (Greater != null) Greater._Add(tmpTreeGreater);
                            else Greater = tmpTreeGreater;
                        }
                        else {
                            if (Greater.Greater != null) {
                                Greater = Greater.Greater;
                            }
                            else {
                                Greater = Greater.Less;
                            }
                        }
                    }
                    resetWeight();
                    return false;
                }
                if (Less != null) {
                    this.Row = Less.Row;
                    this.Equal = Less.Equal;
                    this.weight = Less.weight;
                    if (Less.Greater.weight >= Less.Less.weight) {
                        BTree<T1> tmpTreeLess = Less.Less;
                        Less = Less.Greater;
                        if (Less != null) Less._Add(tmpTreeLess);
                        else Less = tmpTreeLess;
                    }
                    else {
                        BTree<T1> tmpTreeGreater = Less.Greater;
                        Less = Less.Less;
                        if (Less != null) Less._Add(tmpTreeGreater);
                        else Less = tmpTreeGreater;
                    }
                    resetWeight();
                    return false;
                }
                return true;
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

    }
}
