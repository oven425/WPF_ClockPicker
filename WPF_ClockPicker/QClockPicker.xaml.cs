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
        public static DependencyProperty ClockPickOperateTypeProperty;
        static QClockPicker()
        {
            SelectedTimeProperty = DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(QClockPicker), new PropertyMetadata(SelectedTimePropertyChange));
            ClockPickOperateTypeProperty = DependencyProperty.Register("ClockPickOperateType", typeof(QClockPick_OperateTypes), typeof(QClockPicker), new PropertyMetadata(ClockPickOperateTypePropertyChange));
        }

        
        public TimeSpan SelectedTime { set { SetValue(SelectedTimeProperty, value); } get { return (TimeSpan)GetValue(SelectedTimeProperty); } }
        static void SelectedTimePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            QClockPicker oo = (QClockPicker)obj;
            if(oo.m_IsStartDrag == false)
            {
                oo.UpdateTime();
            }
        }

        public QClockPick_OperateTypes ClockPickOperateType { set { SetValue(ClockPickOperateTypeProperty, value); } get { return (QClockPick_OperateTypes)GetValue(ClockPickOperateTypeProperty); } }
        static void ClockPickOperateTypePropertyChange(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            QClockPicker oo = (QClockPicker)obj;
            oo.UpdateOperateType();
            oo.UpdateTime();
        }


        public enum QClockPick_OperateTypes
        {
            Hour,
            Min
        }

        void UpdateOperateType()
        {
            switch(this.ClockPickOperateType)
            {
                case QClockPick_OperateTypes.Min:
                    {
                        this.itemscontrol_hour_am.Visibility = Visibility.Collapsed;
                        this.itemscontrol_hour_pm.Visibility = Visibility.Collapsed;
                        this.itemscontrol_min.Visibility = Visibility.Visible;
                        this.textblock_min.Foreground = Brushes.Green;
                        this.textblock_hour.Foreground = Brushes.Gray;
                    }
                    break;
                case QClockPick_OperateTypes.Hour:
                default:
                    {
                        this.itemscontrol_min.Visibility = Visibility.Collapsed;
                        this.itemscontrol_hour_am.Visibility = Visibility.Visible;
                        this.itemscontrol_hour_pm.Visibility = Visibility.Visible;
                        this.textblock_min.Foreground = Brushes.Gray;
                        this.textblock_hour.Foreground = Brushes.Green;
                    }
                    break;
            }
        }

        void UpdateTime()
        {
            switch (this.ClockPickOperateType)
            {
                case QClockPick_OperateTypes.Min:
                    {
                        double angle = 360.0 / 60.0;
                        angle = angle * this.SelectedTime.Minutes;
                        this.Angle2Pos_Min(angle);
                        this.textblock_min.Text = this.SelectedTime.Minutes.ToString("D2");
                    }
                    break;
                case QClockPick_OperateTypes.Hour:
                default:
                    {
                        double angle = 360.0 / 12.0;

                        angle = angle * this.SelectedTime.Hours;
                        this.Angle2Pos_Hour(angle);
                        this.textblock_hour.Text = this.SelectedTime.Hours.ToString("D2");
                    }
                    break;
            }            
        }

        double m_Width = 30;
        double m_Hieght = 30;
        double m_Hour_PM_Margin = 50;
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
            if((this.m_IsFirstLoad == true) && (this.IsVisible==true))
            {
                this.m_IsFirstLoad = false;
                for(int i=0; i<12; i++)
                {
                    this.m_Hours_AM.Add(new CQClockData() { Width=this.m_Width, Height=this.m_Hieght});
                    this.m_Hours_PM.Add(new CQClockData() { Width = this.m_Width, Height = this.m_Hieght });
                    this.m_Mins.Add(new CQClockData() { Width = this.m_Width, Height = this.m_Hieght });
                }
                double radus = this.ellipse.ActualWidth / 2;
                radus = radus - this.m_Width/2;
                this.Calac_Hour(radus, this.m_Hours_AM, true);
                this.itemscontrol_hour_am.ItemsSource = this.m_Hours_AM;

                radus = (this.ellipse.ActualWidth- this.m_Hour_PM_Margin*2) / 2;
                radus = radus - this.m_Width / 2;
                this.Calac_Hour(radus, this.m_Hours_PM, false, this.m_Hour_PM_Margin);
                this.itemscontrol_hour_pm.ItemsSource = this.m_Hours_PM;

                radus = this.ellipse.ActualWidth / 2;
                radus = radus - this.m_Width / 2;
                this.Calac_Min(radus, this.m_Mins);
                this.itemscontrol_min.ItemsSource = this.m_Mins;
                
                
                double center_x = this.ellipse.ActualWidth / 2;
                double center_y = this.ellipse.ActualHeight / 2;
                this.line.X1 = center_x;
                this.line.Y1 = center_y;

                this.ellipse_min_select.Width = this.m_Width;
                this.ellipse_min_select.Height = this.m_Hieght;

                
                this.UpdateOperateType();
                this.UpdateTime();
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
                ckd.Left = x + radus + offset;
                ckd.Top = y + radus  + offset;

            }
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
                
                ckd.Left = x + radus + offset;
                ckd.Top = y + radus + offset;

            }
        }

        void Angle2Pos_Min(double angle)
        {
            angle = angle - 90;
            double radain = Math.PI * angle / 180;
            float x1 = (float)(Math.Cos(radain) * (this.ellipse.ActualWidth / 2 - this.m_Width / 2));
            float y1 = (float)(Math.Sin(radain) * (this.ellipse.ActualHeight / 2 - this.m_Hieght / 2));
            Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - this.m_Width / 2);
            Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - this.m_Hieght / 2);
            this.line.X2 = x1 + this.ellipse.ActualWidth / 2;
            this.line.Y2 = y1 + this.ellipse.ActualHeight / 2;
        }

        void Angle2Pos_Hour(double angle)
        {
            angle = angle - 90;
            double radain = Math.PI * angle / 180;
            if ((this.SelectedTime.Hours > 12) || (this.SelectedTime.Hours == 0))
            {
                double radus = (this.ellipse.ActualWidth - this.m_Hour_PM_Margin * 2) / 2;
                radus = radus - this.m_Width / 2;
                this.itemscontrol_hour_pm.ItemsSource = this.m_Hours_PM;
                float x1 = (float)(Math.Cos(radain) * radus);
                float y1 = (float)(Math.Sin(radain) * radus);
                Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - this.m_Width / 2);
                Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - this.m_Hieght / 2);
                this.line.X2 = x1 + this.ellipse.ActualWidth / 2;
                this.line.Y2 = y1 + this.ellipse.ActualHeight / 2;
            }
            else
            {
                float x1 = (float)(Math.Cos(radain) * (this.ellipse.ActualWidth / 2 - this.m_Width / 2));
                float y1 = (float)(Math.Sin(radain) * (this.ellipse.ActualHeight / 2 - this.m_Hieght / 2));
                Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - this.m_Width / 2);
                Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - this.m_Hieght / 2);
                this.line.X2 = x1 + this.ellipse.ActualWidth / 2;
                this.line.Y2 = y1 + this.ellipse.ActualHeight / 2;
            }
        }

        void Angle2Pos_Hour_Move(double angle)
        {
            angle = angle - 90;
            double radain = Math.PI * angle / 180;
            if ((this.SelectedTime.Hours > 12) || (this.SelectedTime.Hours == 0))
            {
                double radus = (this.ellipse.ActualWidth - this.m_Hour_PM_Margin * 2) / 2;
                radus = radus - this.m_Width / 2;
                this.itemscontrol_hour_pm.ItemsSource = this.m_Hours_PM;
                float x1 = (float)(Math.Cos(radain) * radus);
                float y1 = (float)(Math.Sin(radain) * radus);
                Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - this.m_Width / 2);
                Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - this.m_Hieght / 2);
                this.line.X2 = x1 + this.ellipse.ActualWidth / 2;
                this.line.Y2 = y1 + this.ellipse.ActualHeight / 2;
            }
            else
            {
                float x1 = (float)(Math.Cos(radain) * (this.ellipse.ActualWidth / 2 - this.m_Width / 2));
                float y1 = (float)(Math.Sin(radain) * (this.ellipse.ActualHeight / 2 - this.m_Hieght / 2));
                Canvas.SetLeft(ellipse_min_select, x1 + this.ellipse.ActualWidth / 2 - this.m_Width / 2);
                Canvas.SetTop(ellipse_min_select, y1 + this.ellipse.ActualHeight / 2 - this.m_Hieght / 2);
                this.line.X2 = x1 + this.ellipse.ActualWidth / 2;
                this.line.Y2 = y1 + this.ellipse.ActualHeight / 2;
            }
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this.ellipse);
            if(this.m_IsStartDrag == true)
            {
                double angle = this.GetAngle(this.ellipse, pt);
                double distance = this.GetDistane(this.ellipse, pt);
                switch(this.ClockPickOperateType)
                {
                    case QClockPick_OperateTypes.Min:
                        {
                            if (distance > 50)
                            {
                                

                                double min = angle * 60 / 360;
                                if(this.SelectedTime.Minutes != min)
                                {
                                    this.SelectedTime = new TimeSpan(this.SelectedTime.Hours, (int)min, 0);
                                    this.textblock_min.Text = this.SelectedTime.Minutes.ToString("D2");
                                }
                                this.Angle2Pos_Min(angle);
                            }
                            
                        }
                        break;
                    case QClockPick_OperateTypes.Hour:
                        {
                            double hour_1 = angle * 12 / 360;
                            int hour = (int)hour_1;
                            if(hour > 0)
                            {
                                if (distance < 100)
                                {
                                    hour = hour + 12;
                                }
                            }
                            else
                            {
                                if (distance > 100)
                                {
                                    hour = hour + 12;
                                }
                            }

                            if (this.SelectedTime.Hours != hour)
                            {
                                this.SelectedTime = new TimeSpan((int)hour, this.SelectedTime.Minutes, 0);
                                this.textblock_hour.Text = this.SelectedTime.Hours.ToString("D2");
                            }
                            this.Angle2Pos_Hour_Move(angle);
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
            Point pt = e.GetPosition(this.ellipse);
            if(this.m_IsStartDrag == true)
            {
                double distance = this.GetDistane(this.ellipse, pt);
                double angle = this.GetAngle(this.ellipse, pt);

                switch (this.ClockPickOperateType)
                {
                    case QClockPick_OperateTypes.Min:
                        {
                            if(distance > 80)
                            {
                                double min = angle * 60 / 360;
                                if (this.SelectedTime.Minutes != min)
                                {
                                    this.SelectedTime = new TimeSpan(this.SelectedTime.Hours, (int)min, 0);
                                    this.textblock_min.Text = this.SelectedTime.Minutes.ToString("D2");
                                }
                                this.Angle2Pos_Min(angle);
                            }
                        }
                        break;
                    case QClockPick_OperateTypes.Hour:
                    default:
                        {
                            angle = angle + 15;
                            if (angle > 360)
                            {
                                angle = 360;
                            }
                            double hour_1 = angle * 12 / 360;
                            int hour = (int)hour_1;
                            if (hour > 0)
                            {
                                if (distance < 100)
                                {
                                    hour = hour + 12;
                                }
                            }
                            else
                            {
                                if (distance > 100)
                                {
                                    hour = hour + 12;
                                }
                            }
                            if (this.SelectedTime.Hours != hour)
                            {
                                this.SelectedTime = new TimeSpan((int)hour, this.SelectedTime.Minutes, 0);
                                this.textblock_hour.Text = this.SelectedTime.Hours.ToString("D2");
                            }
                            double angle1 = 360.0 / 12.0;
                            angle1 = angle1 * this.SelectedTime.Hours;
                            this.Angle2Pos_Hour_Move(angle1);

                        }
                        break;
                }
                this.m_IsStartDrag = false;
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

        private void textblock_hour_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ClockPickOperateType = QClockPick_OperateTypes.Hour;
        }

        private void textblock_min_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ClockPickOperateType = QClockPick_OperateTypes.Min;
        }
    }

    public class CQClockData:INotifyPropertyChanged
    {
        double m_Top;
        double m_Left;
        public int Value { set; get; }
        bool m_IsSelected;
        double m_Width;
        double m_Height;
        public CQClockData()
        {
        }
        public double Left { set { this.m_Left = value; this.Update("Left"); } get { return this.m_Left; } }
        public double Top { set { this.m_Top = value; this.Update("Top"); } get { return this.m_Top; } }
        public double Width { set { this.m_Width = value; this.Update("Width"); } get { return this.m_Width; } }
        public double Height { set { this.m_Height = value; this.Update("Height"); } get { return this.m_Height; } }
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
