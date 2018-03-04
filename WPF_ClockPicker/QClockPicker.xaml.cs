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
        public static DependencyProperty SelectedTimeProperty;
        static QClockPicker()
        {
            SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(QClockPicker), new PropertyMetadata(SelectedTimePropertyChange));
        }

        
        public TimeSpan SelectedTime { set { SetValue(SelectedTimeProperty, value); } get { return (TimeSpan)GetValue(SelectedTimeProperty); } }
        static void SelectedTimePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            QClockPicker oo = (QClockPicker)obj;
            if(oo.m_IsStartDrag == false)
            {

            }
        }


        public enum QClockPick_OperateTypes
        {
            Hour,
            Min
        }
        QClockPick_OperateTypes m_QClockPick_OperateType;

        void UpdateOperateType()
        {
            this.itemscontrol_hour_am.Visibility = Visibility.Collapsed;
            this.itemscontrol_hour_pm.Visibility = Visibility.Collapsed;

        }

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
            this.m_QClockPick_OperateType = QClockPick_OperateTypes.Min;
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

                double center_x = this.ActualWidth / 2;
                double center_y = this.ActualHeight / 2;
                this.line.X1 = center_x;
                this.line.Y1 = center_y;
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
            Point pt = e.GetPosition(this.ellipse);
            if(this.m_IsStartDrag == true)
            {
                double distance = this.GetDistane(this.ellipse, pt);
                switch(this.m_QClockPick_OperateType)
                {
                    case QClockPick_OperateTypes.Min:
                        {
                            if (distance > 50)
                            {
                                this.line.X2 = pt.X;
                                this.line.Y2 = pt.Y;
                                double angle = this.GetAngle(this.ellipse, pt);

                                double min = angle * 60 / 360;
                                if(this.SelectedTime.Minutes != min)
                                {
                                    this.SelectedTime = new TimeSpan(this.SelectedTime.Hours, (int)min, 0);
                                }
                                //System.Diagnostics.Trace.WriteLine((int)min);

                                angle = angle - 90;

                                double radain = Math.PI * angle / 180;
                                float x1 = (float)(Math.Cos(radain) * this.ellipse.ActualWidth / 2);
                                float y1 = (float)(Math.Sin(radain) * this.ellipse.ActualHeight / 2);
                                Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - 15);
                                Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - 15);
                            }
                            
                        }
                        break;
                    case QClockPick_OperateTypes.Hour:
                        {

                        }
                        break;
                }
                
                
            }
            
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
                angle = angle - 15;
                if(angle < 0)
                {
                    angle = 360.0 - angle;
                }



                System.Diagnostics.Trace.WriteLine(angle);
                int dd = (int)(angle / 30.0);



                //System.Diagnostics.Trace.WriteLine(string.Format("{0}  {1}", angle, dd));

                dd = dd - 3;
                if(dd <0)
                {
                    dd = this.m_Hours_AM.Count + dd;
                }
                //this.m_Hours_AM[dd].IsSelected = true;
                
                this.ellipse.ReleaseMouseCapture();
            }
        }

        double GetDistane(FrameworkElement element, Point pt)
        {
            double len = 0;
            double center_x = element.ActualWidth / 2;
            double center_y = element.ActualHeight / 2;
            double d_x = Math.Pow((pt.X - center_x), 2);
            double d_y = Math.Pow((pt.Y - center_y), 2);
            len = Math.Sqrt(d_x + d_y);
            return len;
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
