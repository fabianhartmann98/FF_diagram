using FF_control_wpf.Classes;
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

namespace FF_control_wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Plot p = new Plot();
        Plot p2; 
        public MainWindow()
        {
            InitializeComponent();

            p = Plot.createTestingPlot();
            p.Can = can;
            p.setScalingAuto();
            can = p.draw();
            p.AddAxis();

            p2 = new Plot();
            p2.Can = can;
            p2.Points.Add(new MeasurementPoint(new Point(-5, 2)));
            p2.Points.Add(new MeasurementPoint(new Point(-2, 4)));
            p2.Points.Add(new MeasurementPoint(new Point(2, -2)));
            p2.Points.Add(new MeasurementPoint(new Point(5, 4)));
            p2.PlotColor = Brushes.Black;
            p2.AxisXmax = p.AxisXmax;
            p2.AxisXmin = p.AxisXmin;
            p2.AxisYmax = p.AxisYmax;
            p2.AxisYmin = p.AxisYmin;
            can = p2.draw(); 

        }

        private void can_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            p.Scrole(e.GetPosition(can),e.Delta);
            can.Children.Clear();
            p.AddAxis(); 
            p.draw(); 
            p2.AxisXmax = p.AxisXmax;
            p2.AxisXmin = p.AxisXmin;
            p2.AxisYmax = p.AxisYmax;
            p2.AxisYmin = p.AxisYmin;
            can = p2.draw(); 
        }

        private void can_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void can_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
