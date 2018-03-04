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
using System.ComponentModel;
using System.Globalization;

namespace WPF_ClockPicker
{
    /// <summary>
    /// QClockPicker.xaml 的互動邏輯
    /// </summary>
    public partial class QClockPicker : UserControl
    {
        public QClockPicker()
        {
            InitializeComponent();
        }

        bool m_IsFirstLoad = true;
        List<CQClockData> m_Hours_AM = new List<CQClockData>();
        List<CQClockData> m_Hours_PM = new List<CQClockData>();
        List<CQClockData> m_Mins = new List<CQClockData>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.m_IsFirstLoad == true)
            {
                this.m_IsFirstLoad = false;
                for(int i=0; i<12; i++)
                {
                    this.m_Hours_AM.Add(new CQClockData());
                    this.m_Hours_PM.Add(new CQClockData());
                    this.m_Mins.Add(new CQClockData());
                }
                double radus = this.ellipse.ActualWidth / 2;
                this.Calac_Hour(radus, this.m_Hours_AM, true);
                this.itemscontrol_hour_am.ItemsSource = this.m_Hours_AM;

                radus = (this.ellipse.ActualWidth-100) / 2;
                this.Calac_Hour(radus, this.m_Hours_PM, false, 50);
                this.itemscontrol_hour_pm.ItemsSource = this.m_Hours_PM;

                radus = this.ellipse.ActualWidth / 2;
                this.Calac_Min(radus, this.m_Mins);
                this.itemscontrol_min.ItemsSource = this.m_Mins;
                

            }
            
            
        }

        void Calac_Min(double radus, List<CQClockData> datas, double offset = 0)
        {
            double angle = 0;
            for (int i = 0; i < 12; i++)
            {
                angle = i * 30;
                double radain = Math.PI * angle / 180;
                float x = (float)(Math.Cos(radain) * radus);
                float y = (float)(Math.Sin(radain) * radus);
                CQClockData ckd = datas[i];
                ckd.Value = ((i * 5)+15)%60;
                ckd.Left = x + radus - 15 + offset;
                ckd.Top = y + radus - 15 + offset;

            }
            //this.itemscontrol_hour_am.ItemsSource = this.m_Hours_AM;
        }

        void Calac_Hour(double radus, List<CQClockData>datas, bool is_am=true, double offset=0)
        {
            double angle = 0;
            for (int i = 0; i < 12; i++)
            {
                angle = i * 30;
                double radain = Math.PI * angle / 180;
                float x = (float)(Math.Cos(radain) * radus);
                float y = (float)(Math.Sin(radain) * radus);
                CQClockData ckd = datas[i];
                if(is_am == true)
                {
                    ckd.Value = (i + 3) % 12;
                    if (ckd.Value == 0)
                    {
                        ckd.Value = 12;
                    }
                }
                else
                {
                    ckd.Value = (i + 3) % 12;
                    if (ckd.Value != 0)
                    {
                        ckd.Value = ckd.Value + 12;
                    }
                }
                
                ckd.Left = x + radus - 15 + offset;
                ckd.Top = y + radus - 15+ offset;

            }
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if(this.m_IsStartDrag == true)
            {

            }
            //Point pt = e.GetPosition(this.ellipse);
            //double w = this.ellipse.ActualWidth/2;
            //double h = this.ellipse.ActualHeight/2;
            //double x = pt.X-w;
            //double y =  pt.Y-h;
            ////double dd = this.getAngle((int)x, (int)y);
            //double radians = Math.Atan2(x, y);
            //double  angle = radians * (180 / Math.PI);
            //if(angle > 0)
            //{
            //    angle = 180.0 - angle;
            //}
            //else
            //{
            //    angle = 180.0 - angle;
            //}
            //angle = angle - 90;

            //double radain = Math.PI * angle / 180;
            //float x1 = (float)(Math.Cos(radain) * w);
            //float y1 = (float)(Math.Sin(radain) * h);
            //Canvas.SetLeft(ellipse_min_select, x1+w-15);
            //Canvas.SetTop(ellipse_min_select, y1+h-15);
        }

        double GetAngle(double x, double y)
        {
            double radians = Math.Atan2(x, y);
            double angle = radians * (180 / Math.PI);
            if (angle > 0)
            {
                angle = 180.0 - angle;
            }
            else
            {
                angle = 180.0 - angle;
            }
            return angle;
        }
        bool m_IsStartDrag = false;
        private void ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.m_IsStartDrag = true;
            this.ellipse.CaptureMouse();
        }

        private void ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(this.m_IsStartDrag == true)
            {
                this.m_IsStartDrag = false;
                double angle = this.GetAngle(this.ellipse, e.GetPosition(this.ellipse));
            }
        }


        double GetAngle(FrameworkElement element, Point pt)
        {
            //Point pt = e.GetPosition(this.ellipse);
            double w = this.ellipse.ActualWidth / 2;
            double h = this.ellipse.ActualHeight / 2;
            double x = pt.X - w;
            double y = pt.Y - h;
            //double dd = this.getAngle((int)x, (int)y);
            double radians = Math.Atan2(x, y);
            double angle = radians * (180 / Math.PI);
            if (angle > 0)
            {
                angle = 180.0 - angle;
            }
            else
            {
                angle = 180.0 - angle;
            }
            //angle = angle - 90;

            return angle;
        }
    }

    public class CQClockData:INotifyPropertyChanged
    {
        double m_Top;
        double m_Left;
        public int Value { set; get; }
        bool m_IsSelected;

        public double Left { set { this.m_Left = value; this.Update("Left"); } get { return this.m_Left; } }
        public double Top { set { this.m_Top = value; this.Update("Top"); } get { return this.m_Top; } }
        public bool IsSelected { set { this.m_IsSelected = value; this.Update("IsSelected"); } get { return this.m_IsSelected; } }
        public event PropertyChangedEventHandler PropertyChanged;
        void Update(string name) { if (this.PropertyChanged != null) { this.PropertyChanged(this, new PropertyChangedEventArgs(name)); } }
    }

    public class CQBool2Brush:IValueConverter
    {
        public Brush True { set; get; }
        public Brush False { set; get; }

        public CQBool2Brush()
        {
            this.True = Brushes.LightGreen;
            this.False = null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool bb = (bool)value;
            return bb == true ? this.True : this.False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
