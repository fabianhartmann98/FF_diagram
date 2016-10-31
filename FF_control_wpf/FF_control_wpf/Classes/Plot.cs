using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace FF_control_wpf.Classes
{
    public class Plot
    {
        #region private variables, not accesable form outside
        private double scaleX;
        private double offsetX;
        private double scaleY;
        private double offsetY;
        private double plothight;
        private double plotwidth; 
        #endregion 

        #region full prop variables         
        private List<MeasurementPoint> points;
        private double xmin;
        private double xmax;
        private double ymin;
        private double ymax;
        private Canvas can;

        /// <summary>
        /// drasw on given can
        /// set: addapts scale 
        /// </summary>
        public Canvas Can
        {
            get { return can; }            
            set 
            {
                scaleY = value.Height / plothight * scaleY;             //addapting scale 
                scaleX = value.Width / plotwidth * scaleX; 
                can = value; 
                plothight = can.Height; 
                plotwidth = can.Width; 
            }
        }
        
        public double AxisYmax
        {
            get { return ymax; }
            set { ymax = value; }
        }
        public double AxisYmin
        {
            get { return ymin; }
            set { ymin = value; }
        }
        public double AxisXmax
        {
            get { return xmax; }
            set { xmax = value; }
        }
        public double AxisXmin
        {
            get { return xmin; }
            set { xmin = value; }
        }
        public List<MeasurementPoint> Points
        {
            get { return points; }
            set { points = value; }
        }
        #endregion

        #region constructors
        /// <summary>
        /// creats default plot 
        /// hight = 100, width = 100 
        /// gets allways called 
        /// </summary>
        public Plot()
        {
            points = new List<MeasurementPoint>();
            plothight = 100;
            plotwidth = 100; 
        }
        public Plot(List<Point> Points): this()
        {
            foreach (var item in Points)
            {
                points.Add(new MeasurementPoint(item)); 
            }
        }
        public Plot(Canvas ca) : this()
        {
            can = ca;
            plothight = can.Height;
            plotwidth = can.Width;
        }
        #endregion

        #region public methods
        public void draw()
        {
            if (can != null && points.Count!=0)
            {
                Polyline pl = new Polyline();
                foreach (MeasurementPoint item in points)
                {
                    pl.Points.Add(scalingPoint(item.getPoint())); 
                }
                can.Children.Clear();
                can.Children.Add(pl); 
            }
        }

        /// <summary>
        /// sets offset and scale depending on the plotwidth and height and the points 
        /// doesn't need to be called if resized
        /// </summary>
        public void setAxisAuto()
        {
            xmin = points[0].Time;
            xmax = xmin;
            ymax = points[0].I_Value;
            ymin = ymax; 
            foreach (MeasurementPoint item in points)   //get min and max values for both axis
            {
                if (item.I_Value > ymax)
                    ymax = item.I_Value;
                if (item.I_Value < ymin)
                    ymin = item.I_Value;
                if (item.Time < xmin)
                    xmin = item.Time;
                if (item.Time > xmax)
                    xmin = item.Time;                 
            }

            offsetX = xmin*0.9;     //give them some margin
            offsetY = ymin*0.9;
            scaleX = plotwidth / (xmax*1.1 - offsetX);
            scaleY = plothight / (ymax*1.1 - offsetY); 

        }

        public void addPoint(MeasurementPoint mp)
        {
            points.Add(mp);             
        }
        #endregion
        
        #region private methods 
        private Point scalingPoint(Point p)
        {
            Point q = new Point();
            q.X = (p.X - offsetX) * scaleX;
            q.Y = (p.Y - offsetY) * scaleY; 
            return q; 
        }
        #endregion

    }
}
