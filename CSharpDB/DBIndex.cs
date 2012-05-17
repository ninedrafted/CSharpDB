
namespace CSharpDB {
    public class DBIndex {
        public long Index {
            get { return _index; }
            set {
                _index = value;
                A = (int)(value / int.MaxValue);
                B = (int)(value % int.MaxValue);
            }

        }
        private long _index;
        public int A { get; private set; }
        public int B { get; private set; }
    }
}
