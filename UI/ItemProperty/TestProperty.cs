using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp4Utl.UI.ItemProperty {
    internal class TestProperty : ItemPropertyWindow {
        public override string ElementType => "ほげ";
        public override List<ItemPropertyComponent> Components { get {
                var l = new List<ItemPropertyComponent>();
                l.Add(new ItemPropertyComponent("拡大率", 0, 100));
                l.Add(new ItemPropertyComponent("神率", -100, 100));
                l.Add(new ItemPropertyComponent("引率", 50, 1000));
                return l;
            } 
        }

        public TestProperty() : base() { }
    }
}
