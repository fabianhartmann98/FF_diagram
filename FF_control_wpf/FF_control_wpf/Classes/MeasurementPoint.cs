﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FF_control_wpf.Classes
{
    public class MeasurementPoint
    {
        private double i_value;             //whats the Current (Strom) value
        private double time;                //when was this recorded
        private int measurementNumber;      //whats the measurement number (not really needed) 

        public int MeasurementNumber
        {
            get { return measurementNumber; }
            set { measurementNumber = value; }
        }
        public double Time
        {
            get { return time; }
            set { time = value; }
        }
        public double I_Value
        {
            get { return i_value; }
            set { i_value = value; }
        }

        public MeasurementPoint()
        {
            i_value = 0;
            time = 0;
            measurementNumber = 0; 
        }
        public MeasurementPoint(double value, double time = 0, int measurementNumber = 0)
        {
            this.measurementNumber = measurementNumber;
            this.i_value = value;
            this.time = time; 
        }
        public MeasurementPoint(Point p, int number = 0)
        {
            i_value = p.Y;
            time = p.X;
            measurementNumber = number; 
        }

        public Point getPoint()
        {
            return new Point(time, i_value); 
        }

        public override string ToString()
        {
            return String.Format("{0:u}: {1:c2}, {2:c2}",measurementNumber,time,i_value); 
        }
        
    }
}
