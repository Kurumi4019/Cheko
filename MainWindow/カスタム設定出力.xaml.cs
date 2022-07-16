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

namespace mp4Utl.MainWindow
{
    /// <summary>
    /// カスタム設定出力.xaml の相互作用ロジック
    /// </summary>
    public partial class カスタム設定出力 : Window
    {
        public カスタム設定出力()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) //右下｢キャンセル｣をクリックしたときの動作
        {
            this.Close();
        }
    }
}
