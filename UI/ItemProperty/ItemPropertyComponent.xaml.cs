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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace mp4Utl.UI.ItemProperty {
    /// <summary>
    /// ItemPropertyElement.xaml の相互作用ロジック
    /// </summary>
    public partial class ItemPropertyComponent : UserControl {
        /// <summary>
        /// 要素名
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 最小値
        /// </summary>
        public int MinValue => _minValue;
        /// <summary>
        /// 最大値
        /// </summary>
        public int MaxValue => _maxValue;
        /// <summary>
        /// 移動可能かどうか
        /// </summary>
        public bool IsMovable { 
            get { 
                return _isMovable; 
            } 
            set { 
                Toggle_Movable(value); _isMovable = value; 
            } 
        }

        private bool _isMovable = false;
        private int _minValue;
        private int _maxValue;

        public ItemPropertyComponent(string text, int min, int max) {
            Text = text; 
            _minValue = min;
            _maxValue = max; 

            InitializeComponent();

            // イベントのバインド
            Decrease_Before.Click += Decrease_Before_Click;
            Decrease_After.Click += Decrease_After_Click;
            Increase_Before.Click += Increase_Before_Click;
            Increase_After.Click += Increase_After_Click;

            // 値の設定
            ElementButton.Content = Text;
            Slider_Before.Minimum = _minValue;
            Slider_After.Minimum = _minValue;
            Slider_Before.Maximum = _maxValue;
            Slider_After.Maximum = _maxValue;

        }

        private void Toggle_Movable(bool val) {
            if (_isMovable && !val) {
                Decrease_After.IsEnabled = false;
                Increase_After.IsEnabled = false;
                Slider_After.IsEnabled = false;
            } else if (!_isMovable && val){
                Decrease_After.IsEnabled = true;
                Increase_After.IsEnabled = true;
                Slider_After.IsEnabled = true;
            }
        }

        private void Increase_After_Click(object sender, RoutedEventArgs e) {
            Slider_After.Value++;
        }

        private void Increase_Before_Click(object sender, RoutedEventArgs e) {
            Slider_Before.Value++;
        }

        private void Decrease_After_Click(object sender, RoutedEventArgs e) {
            Slider_After.Value--;
        }

        private void Decrease_Before_Click(object sender, RoutedEventArgs e) {
            Slider_Before.Value--;
        }

        /// <summary>
        /// 移動前の値を取得します
        /// </summary>
        /// <returns>移動前の値</returns>
        public double GetValue_Before() => Slider_Before.Value;
        /// <summary>
        /// 移動後の値を取得します
        /// </summary>
        /// <returns>移動後の値</returns>
        public double GetValue_After() => Slider_After.Value;
    }
}
