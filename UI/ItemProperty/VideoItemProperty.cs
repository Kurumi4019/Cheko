using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mp4Utl.UI.ItemProperty {
    internal class VideoItemProperty : ItemPropertyWindow {
        public override string ElementType => "動画ファイル";
        public override List<ItemPropertyComponent> DefaultComponents { get {
                var l = new List<ItemPropertyComponent>();
                l.Add(new ItemPropertyComponent("X", -100, 100));
                l.Add(new ItemPropertyComponent("Y", -100, 100));
                l.Add(new ItemPropertyComponent("Z", -100, 100));
                l.Add(new ItemPropertyComponent("拡大率", 0, 100));
                l.Add(new ItemPropertyComponent("透明度", 0, 100));
                l.Add(new ItemPropertyComponent("再生速度", -100, 100));
                l.Add(new ItemPropertyComponent("再生位置", 0, 1000));
                return l;
            } 
        }

        public VideoItemProperty()
            : base() {

        }
    }
}
