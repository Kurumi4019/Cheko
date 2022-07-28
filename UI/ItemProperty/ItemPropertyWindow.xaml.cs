using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace mp4Utl.UI.ItemProperty {
    /// <summary>
    /// ItemPropertyWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ItemPropertyWindow : Window {
        public ItemPropertyWindow() {
            InitializeComponent();
        }

        private void Button_Click_AddEffect(object sender, RoutedEventArgs e) {
            var win = new AddEffect();
            win.Show();
        }
    }
}
