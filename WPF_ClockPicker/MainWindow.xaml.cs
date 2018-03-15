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

namespace WPF_ClockPicker
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void checkbox_type_Click(object sender, RoutedEventArgs e)
        {
            if(this.checkbox_type.IsChecked == true)
            {
                this.clock_picker.ClockPickOperateType = QClockPicker.QClockPick_OperateTypes.Min;
            }
            else
            {
                this.clock_picker.ClockPickOperateType = QClockPicker.QClockPick_OperateTypes.Hour;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
