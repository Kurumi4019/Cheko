using DynamicAero2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace mp4Utl.NewFolder
/// <summary>
/// Settings.xaml の相互作用ロジック
/// </summary>
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Button_Click_Theme(object sender, RoutedEventArgs e)
        {
            var theme = Application.Current.Resources.MergedDictionaries[0] as DynamicAero2.Theme;
            if (theme is null) return;

            theme.Color = theme.Color switch
            {
                ThemeColor.NormalColor => ThemeColor.Black,
                ThemeColor.Black => ThemeColor.Dark,
                ThemeColor.Dark => ThemeColor.Light,
                _ => ThemeColor.NormalColor
            };
        }
    }
}
