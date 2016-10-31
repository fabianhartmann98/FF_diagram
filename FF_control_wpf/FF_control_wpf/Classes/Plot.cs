using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FF_control_wpf.Classes
{
    public class Plot
    {
        #region private variables, not accesable form outside
        private double scaleX;              //by wich scale do i have to multiplie to use canvas properly (set in setAxisAuto and set Can)
        private double offsetX;             //don't start at the left end (set in setAxisAuto) 
        private double scaleY;              //by wich scale do i have to multiplie to use canvas properly (set in setAxisAuto and set Can) 
        private double offsetY;             //don't start at the bottom end (set in setAxisAuto) 
        private double plotheight;          //whats the canvas hight (default = 100)
        private double plotwidth;           //whats the canvas width (default = 100) 
        #endregion 

        #region full prop variables         
        private List<MeasurementPoint> points;      //the point that are going to get dawn 
        private double xmin;                        //min x (time) value
        private double xmax;                        //maximum x (time) value 
        private double ymin;                        //minimum y (i_value) value
        private double ymax;                        //maximum y (i_value) value
        private Canvas can;                         //the canvas to draw on 

        /// <summary>
        /// drasw on given can
        /// set: addapts scale 
        /// </summary>
        public Canvas Can
        {
            get { return can; }            
            set 
            {
                scaleY = value.Height / plotheight * scaleY;             //addapting scale 
                scaleX = value.Width / plotwidth * scaleX; 
                can = value; 
                plotheight = can.Height;                                //setting height and Width
                plotwidth = can.Width; 
            }
        }

        public double AxisYmax
        {
            get { return ymax; }
            set 
            { 
                ymax = value;
                OffsetScaleCalculation();
            }
        }                             //setting scale and offset
        public double AxisYmin
        {
            get { return ymin; }
            set 
            {
                ymin = value;
                OffsetScaleCalculation();
            }
        }
        public double AxisXmax
        {
            get { return xmax; }
            set             
            { 
                xmax = value;
                OffsetScaleCalculation();
            }
        }
        public double AxisXmin
        {
            get { return xmin; }
            set 
            {
                xmin = value; 
                OffsetScaleCalculation();
            }
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
            plotheight = 100;
            plotwidth = 100; 
        }
        public Plot(List<Point> Points): this()         //calls Plot() first
        {
            foreach (var item in Points)
            {
                points.Add(new MeasurementPoint(item)); 
            }
        }
        public Plot(Canvas ca) : this()                 //calls Plot() first
        {
            can = ca;
            plotheight = can.Height;
            plotwidth = can.Width;
        }
        #endregion

        #region public methods
        /// <summary>
        /// drawing the Points in a Polyline (thickness = 3, color = Black) 
        /// clears can before adding Polyline        
        /// </summary>
        /// <returns>Canvas with Polyline as Child</returns>
        public Canvas draw()
        {
           
            if (can != null && points.Count!=0)     //only if some points are available and if can is existing 
            {
                Polyline pl = new Polyline();       //defining new Polyline (Thickness = 3, Color = Black) 
                pl.StrokeThickness = 3;
                pl.Stroke = Brushes.Black;
                foreach (MeasurementPoint item in points)       //adding the Point in the list
                {
                    pl.Points.Add(scalingPoint(item.getPoint()));   //editing the points to fit to Canvas an plot 
                }
                can.Children.Clear();                       //clear Can before adding Polyline
                can.Children.Add(pl); 
            }
            return can;
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
                if (item.Time < xmin)
                    xmin = item.Time;
                if (item.Time > xmax)
                    xmax = item.Time; 
                if (item.I_Value > ymax)
                    ymax = item.I_Value;
                if (item.I_Value < ymin)
                    ymin = item.I_Value;
                               
            }

            OffsetScaleCalculation();

        }


        public void addPoint(MeasurementPoint mp)
        {
            points.Add(mp);             
        }

        /// <summary>
        /// just for debugging purpose
        /// </summary>
        /// <returns></returns>
        public static Plot createTestingPlot()
        {
            Plot p = new Plot();
            for (int i = 0; i < 20; i++)
            {
                p.addPoint(new MeasurementPoint(new Point(i, 5-i%10), i)); 
            }
            return p; 
        }
        #endregion
        
        #region private methods 
        /// <summary>
        /// scales the given point with the scale and offset (also plotheight) 
        /// </summary>
        /// <param name="p">edits this point</param>
        /// <returns>scaled point with fitting values for plot and canvas</returns>
        private Point scalingPoint(Point p)
        {
            Point q = new Point();
            q.X = (p.X - offsetX) * scaleX;
            q.Y = plotheight - (p.Y - offsetY) * scaleY; 
            return q; 
        }

        /// <summary>
        /// sets offset and Sclae for given min and max value 
        /// </summary>
        private void OffsetScaleCalculation()
        {
            offsetX = xmin - (xmax - xmin) * 0.1;     //give them some margin 20% of the canvas is margin 
            offsetY = ymin - (ymax - ymin) * 0.1;
            scaleX = plotwidth / (xmax + (xmax - xmin) * 0.2 - offsetX);
            scaleY = plotheight / (ymax + (ymax - ymin) * 0.2 - offsetY);
        }
        #endregion

    }
}
