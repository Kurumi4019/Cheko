using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public abstract partial class ItemPropertyWindow : Window {
        public abstract string ElementType { get; }
        public abstract List<ItemPropertyComponent> Components { get; }
        public ItemPropertyWindow() {
            InitializeComponent();
            SetComponents();

            ElementType_Label.Content = ElementType;
        }

        private void SetComponents() {
            for (int i = 0; i < Components.Count; i++) {
                var c = Components[i];

                Debug.WriteLine(c.ActualHeight);
                Components_Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Pixel) });
                Components_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(480, GridUnitType.Pixel) });
                c.SetValue(Grid.RowProperty, i);
                c.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
                Components_Grid.Children.Add(c);
            }
        }
        
        private void Button_Click_AddEffect(object sender, RoutedEventArgs e) {
            var win = new AddEffect();
            win.Show();
        }
    }
}
