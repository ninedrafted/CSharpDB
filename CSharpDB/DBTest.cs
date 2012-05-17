using System.Collections.Generic;

namespace CSharpDB {
    public class DBTest {
        public BTree<int> Root;
        public List<List<int>> Column0 = new List<List<int>>();
        public List<DBIndex> indexes = new List<DBIndex>();
        public void Add(int value) {
            if (Column0.Count == 0)
                Column0.Add(new List<int>());
            Column0[0].Add(value);
            DBIndex i = new DBIndex();
            i.Index = Column0[0].Count - 1;
            indexes.Add(i);
            if (Root == null)
                Root = new BTree<int>(ref Column0, indexes[indexes.Count - 1]);
            else
                Root.Add(indexes[indexes.Count - 1]);
        }
        public int[] GetValues(List<DBIndex> list) {
            int[] ret = new int[list.Count];
            for (int i = 0; i < ret.Length; i++) {
                ret[i] = Column0[list[i].A][list[i].B];
            }
            return ret;
        }
        public void RemoveAll(int item) {
            if (Root != null) {
                Root.RemoveAll(item);
            }
        }
    }
}
