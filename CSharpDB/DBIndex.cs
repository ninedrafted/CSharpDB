
namespace CSharpDB {
    public class DBIndex {
        public DBIndex(long index) {
            this.Index = index;
        }
        public DBIndex(int a, int b) {
            this._a = a;
            this._b = b;
        }
        public long Index {
            get { return _index; }
            set {
                _index = value;
                _a = (int)(value / int.MaxValue);
                _b = (int)(value % int.MaxValue);
            }

        }
        private long _index;
        private int _a;
        public int A { get { return _a; } }
        private int _b;
        public int B { get { return _b; } }
    }
}
