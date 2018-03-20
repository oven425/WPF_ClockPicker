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

        public static Point GetCoordinate(int a, int b, float rotate)
        {
            double x = 0, y = 0, tan = 0, Rad = 0;

            if (Math.Abs(rotate) > 90)
                Rad = (Math.Abs(rotate) - 90);
            else
                Rad = (90 - Math.Abs(rotate));
            Rad = Rad * 2 * Math.PI / 360;
            tan = Math.Tan(Rad);

            x = Math.Sqrt((double)1 / ((double)1 / (a * a) + (tan * tan) / (b * b)));
            y = x * tan;

            if (rotate < 0)
                x = 0 - x;
            if (rotate > -90 && rotate < 90)
                y = 0 - y;
            x = a + x;
            y = b + y;

            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for(float i=-180; i<180; i++)
            {
                Point pt = GetCoordinate(100, 50, i);
                Rectangle rr = new Rectangle();
                rr.Fill = Brushes.Red;
                rr.Width = rr.Height = 1;
                this.canvas.Children.Add(rr);
                Canvas.SetLeft(rr, pt.X);
                Canvas.SetTop(rr, pt.Y);
            }
        }
    }
}
