﻿using FF_control;
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
        Diagram  diagram = new Diagram();
        public MainWindow()
        {
            InitializeComponent();

            diagram = new Diagram();
            diagram.addGraph(Diagram.createTestingPlot());
            Graph g = new Graph();            
            g.addPoint(new MeasurementPoint(new Point(-5, 2)));
            g.addPoint(new MeasurementPoint(new Point(-2, 4)));
            g.addPoint(new MeasurementPoint(new Point(2, -2)));
            g.addPoint(new MeasurementPoint(new Point(5, 4)));
            diagram.addGraph(g);

            diagram.Grpahs[1].PlotColor = Brushes.Blue;
            diagram.Grpahs[0].PlotColor = Brushes.Green;

            diagram.Can = can;
            diagram.setScalingAuto();
            diagram.DrawAxis2dot0();
            can = diagram.draw();
            
        }

        private void can_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            diagram.Scrole(e.GetPosition(can),e.Delta);
            can.Children.Clear();
            diagram.DrawAxis2dot0();
            can = diagram.draw(); 
        }

        private void can_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            diagram.Can = can;
        }

        private void can_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
