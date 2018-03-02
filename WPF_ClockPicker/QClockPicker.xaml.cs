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

        public double getAngle(float x, float y)
        {
            if (y == 0 && x >= 0)
            {
                return 0;
            }
            else if (x == 0 && y >= 0)
            {
                return 90;
            }
            else if (y == 0 && x < 0)
            {
                return 180;
            }
            else if (x == 0 && y < 0)
            {
                return 270;
            }

            double sA = Math.Asin(Math.Abs(y) / Math.Sqrt(x * x + y * y));

            if (x >= 0 && y >= 0)
            {
                return sA;
            }
            else if (x <= 0 && y >= 0)
            {
                return Math.PI - sA;
            }
            else if (x <= 0 && y <= 0)
            {
                return Math.PI + sA;
            }
            else if (x >= 0 && y <= 0)
            {
                return Math.PI + Math.PI / 2 + Math.Asin(Math.Abs(x) / Math.Sqrt(x * x + y * y));
            }
            return 0;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.Calac();
        }

        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this.ellipse);
            double dd = this.getAngle((int)pt.X, (int)pt.Y);
            System.Diagnostics.Trace.WriteLine(dd);
        }
    }

    public class CQClockData:INotifyPropertyChanged
    {
        double m_Top;
        double m_Left;
        public int Value { set; get; }


        public double Left { set { this.m_Left = value; this.Update("Left"); } get { return this.m_Left; } }
        public double Top { set { this.m_Top = value; this.Update("Top"); } get { return this.m_Top; } }
        public event PropertyChangedEventHandler PropertyChanged;
        void Update(string name)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
