using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDB {
    struct DBCloumDescriptor {
        public DBCloumDescriptor(string name, BTree<IComparable> columnDescriptor) {
            this.Name = name;
            this.ColumnDescriptor = columnDescriptor;
        }
        public string Name;
        public BTree<IComparable> ColumnDescriptor;
    }
}
