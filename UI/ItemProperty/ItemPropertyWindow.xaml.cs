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
        /// <summary>
        /// ファイルタイプ
        /// </summary>
        public abstract string ElementType { get; }
        /// <summary>
        /// デフォルトで表示されるコンポーネント
        /// </summary>
        public abstract List<ItemPropertyComponent> DefaultComponents { get; }

        public ItemPropertyWindow() {
            InitializeComponent();
            SetComponents();

            ElementType_Label.Content = ElementType;
        }

        private void SetComponents() {
            DefaultComponents.ForEach(c => AddComponent(c));
        }

        /// <summary>
        /// 指定した名前のコンポーネントの移動前の値を取得します
        /// </summary>
        /// <param name="componentName">値を得たいコンポーネントの名前</param>
        /// <returns>取得した値</returns>
        public double GetValue_Before(string componentName) => GetComponentByName(componentName).GetValue_Before();

        /// <summary>
        /// 指定した名前のコンポーネントの移動後の値を取得します
        /// </summary>
        /// <param name="componentName">値を得たいコンポーネントの名前</param>
        /// <returns>取得した値</returns>
        public double GetValue_After(string componentName) => GetComponentByName(componentName).GetValue_After();

        /// <summary>
        /// アイテムを追加します
        /// </summary>
        /// <param name="component">追加するコンポーネント</param>
        public void AddComponent(ItemPropertyComponent component) {
            component.SetValue(Grid.RowProperty, Components_Grid.RowDefinitions.Count);
            component.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            Components_Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20, GridUnitType.Pixel) });
            Components_Grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(480, GridUnitType.Pixel) });           
            Components_Grid.Children.Add(component);
        }

        /// <summary>
        /// 名前をからコンポーネントを探します
        /// </summary>
        /// <param name="text">探したいコンポーネントの名前</param>
        /// <returns>見つかったコンポーネント</returns>
        /// <exception cref="InvalidOperationException">探したいコンポーネントがなかった場合</exception>
        public ItemPropertyComponent GetComponentByName(string text) {
            for (int i = 0; i < Components_Grid.Children.Count; i++) {
                object c = Components_Grid.Children[i];
                if (c is ItemPropertyComponent) {
                    if ((c as ItemPropertyComponent).Text == text) {
                        return (ItemPropertyComponent)c;
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
