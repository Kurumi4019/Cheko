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
        public abstract List<ItemPropertyComponent> DefalutComponents { get; }
        public ItemPropertyWindow() {
            InitializeComponent();
            SetComponents();

            ElementType_Label.Content = ElementType;
        }

        private void SetComponents() {
            DefalutComponents.ForEach(c => AddComponent(c));
        }

        public void AddComponent(ItemPropertyComponent component) {
            component.SetValue(Grid.RowProperty, Components_Grid.RowDefinitions.Count);
            component.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            Components_Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Pixel) });
            Components_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(480, GridUnitType.Pixel) });           
            Components_Grid.Children.Add(component);
        }

        public void DeleteComponent(string text) {
            for (int i = 0; i < Components_Grid.Children.Count; i++) {
                var c = Components_Grid.Children[i];
                if (c is ItemPropertyComponent) {
                    if ((c as ItemPropertyComponent).Text == text) {
                        Components_Grid.RowDefinitions.RemoveAt(i);                      
                        return;
                    }
                }
            }
            throw new InvalidOperationException();
        }
        
        private void Button_Click_AddEffect(object sender, RoutedEventArgs e) {
            var win = new AddEffect();
            win.Show();
        }
    }
}
