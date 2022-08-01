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

            // 合成モードコンボボックスの初期値設定
            var defaultBlendingModes = new string[] { "通常", "加算(RED)", "加算(GREEN)", "加算(BLUE)", "加算(RED-GREEN)", "加算(GREEN-BLUE)", "加算(BLUE-RED)", "加算(unsigned)", "減算", "減算(unsigned)", "乗算", "除算", "スクリーン", "オーバーレイ", "オーバーレイ(GIMP)", "比較(明)", "比較(暗)", "輝度", "色差", "陰影", "陰影(焼きこみ(リニア))", "明暗", "明暗(リニアライト)", "明暗(逆)", "差分", "除外", "ネガ", "反転", "カラー比較(明)" }; // 途中で力尽きた
            defaultBlendingModes.ToList().ForEach(x => BlendingMode_ComboBox.Items.Add(x));
            BlendingMode_ComboBox.Text = defaultBlendingModes[0];

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
        /// 指定した名前のコンポーネントが移動可能かどうかを切り替えます
        /// </summary>
        /// <param name="componentName">切り替えたいコンポーネントの名前</param>
        public void ToggleMovable(string componentName) => GetComponentByName(componentName).IsMovable = !GetComponentByName(componentName).IsMovable;

        /// <summary>
        /// 現在選択されている合成モードを取得します
        /// </summary>
        /// <returns>現在選択されている合成モード</returns>
        public string GetCurrentBlendingMode() => BlendingMode_ComboBox.Text;

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
