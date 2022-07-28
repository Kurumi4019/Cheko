using System.Windows;
using mp4Utl.UI.ItemProperty;
using FFmpeg.AutoGen;

namespace mp4Utl.NewFolder

/// <summary>
/// PreviewWindow.xaml の相互作用ロジック
/// </summary>
{ public partial class PreviewWindow : Window
{
    public PreviewWindow()
    {
        InitializeComponent();
    }
    //　MenuItem_Clickは実装順にやってますので視認性ちょっと悪いかもしれません。
    //　ご了承下さい。

    private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
    //｢ファイル｣→｢ソフトウェアの終了｣をクリックしたときの動作
    {
        {
            MessageBoxResult Result = MessageBox.Show("ソフトウェアを終了しますか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)  //｢はい｣を押したとき
            {
                this.Close();
            }
            else if (Result == MessageBoxResult.No) //｢いいえ｣を押したとき
            {
            }
        }

    }
    private void MenuItem_Click_Version(object sender, RoutedEventArgs e) //｢ヘルプ｣→｢バージョン情報｣をクリックしたときの動作
    {
        MessageBox.Show("バージョン:α12\r\n作者:くるみ白羽\r\n\r\nこのソフトウェアは [GNU Lesser General Public License v3.1] ライセンスに基づき配布されています｡たぶんね");
    }

    private void MenuItem_Click_GitHub(object sender, RoutedEventArgs e) //｢ヘルプ｣→｢GitHubへのリンク｣をクリックしたときの動作
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo("https://github.com/Kurumi4019/mp4Utl");
        startInfo.UseShellExecute = true;
        System.Diagnostics.Process.Start(startInfo);
    }

    private void MenuItem_Click_Settings(object sender, RoutedEventArgs e) //｢ファイル｣→｢環境設定｣をクリックしたときの動作
    {
        var win = new Settings();
        win.Show();
    }

    private void MenuItem_Click_ItemProperty(object sender, RoutedEventArgs e) //｢表示｣→｢アイテムプロパティの表示｣をクリックしたときの動作
    {
            //var win = new Video();
            var win = new TestProperty();
            win.Show();
    }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) //シークバー
    {

    }
}
}
