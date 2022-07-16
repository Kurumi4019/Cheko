using System.Windows;

namespace mp4Utl.MainWindow
{
    /// <summary>
    /// メイン画面.xaml の相互作用ロジック
    /// </summary>
    public partial class メイン画面 : Window
    {
        public メイン画面()
        {
            InitializeComponent();
        }

        //　MenuItem_Clickは実装順にやってますので視認性ちょっと悪いかもしれません。
        //　ご了承下さい。

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        //｢ファイル｣→｢ソフトウェアの終了｣をクリックしたときの動作
        {
            {
                MessageBoxResult Result = MessageBox.Show("上書き保存は､きちんと済ませましたか?\r\n※保存していても､していなくても､最終チェックをぬかしてはいけません!!", "確認", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (Result == MessageBoxResult.Yes)  //｢はい｣を押したとき
                {
                    MessageBox.Show("では､また会えるのを楽しみに待ってますね!\r\n……へんたいふしんしゃさん♪", "確認");
                    this.Close();
                }
                else if (Result == MessageBoxResult.No) //｢いいえ｣を押したとき
                {
                    MessageBox.Show("まったくもう…\r\nこまめな保存は､とても大事です!!\r\nよ～く肝に銘じておいてくださいね!!!!", "確認");
                }
            }

        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e) //｢ヘルプ｣→｢バージョン情報｣をクリックしたときの動作
        {
            MessageBox.Show("バージョン：α11\r\n作者：くるみ白羽\r\n\r\nこのソフトウェアは [GNU Lesser General Public License v2.1] ライセンスに基づき配布されています｡たぶんね");
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) //｢ファイル｣→｢カスタム設定出力｣をクリックしたときの動作
        {
            var win = new カスタム設定出力();
            win.Show();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e) //｢ヘルプ｣→｢GitHubへのリンク｣をクリックしたときの動作
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo("https://github.com/Kurumi4019/mp4Utl");
            startInfo.UseShellExecute = true;
            System.Diagnostics.Process.Start(startInfo);
        }


    }
}
