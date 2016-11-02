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

        #region constants
        private double arrowpercentage = 0.0125; //what percentage is the arrow wide  
        private double arrowlengthmultiplier = 2; //how many times longer than wide 
        private double LableMarkerLenght = 10; //how long ist the Marker for a lable at a Axis 
        private int AxisStrokeThickness = 1;    //default StrokeThickness for the Axis
        private int PlotStrokeThickness = 3; //default StrokeThickness for the Plot
        private int LabelMarginTopX = 5;         //whats the Margin to the Label Marker X Axis
        private int LabelMarginLeftX = -10;        //Margin to the Label Marker XAxis
        private int LabelMarginTopY = -10;         //whats the Margin to the Label Marker YAxis
        private int LabelMarginLeftY = -25;        //Margin to the Label Marker YAxis
        #endregion

        #endregion

        #region full prop variables
        private List<MeasurementPoint> points;      //the point that are going to get dawn 
        private double xmin;                        //min x (time) value
        private double xmax;                        //maximum x (time) value 
        private double ymin;                        //minimum y (i_value) value
        private double ymax;                        //maximum y (i_value) value
        private Canvas can;                         //the canvas to draw on 
        private int xAxisLabelCount=5;                //how many labels should be placed on the x Axis (default = 5)
        private int yAxisLabelCount=5;                //how many labels should be placed on the y Axis  (default = 5)

	    public int YAxisLabelCount
	    {
		    get { return yAxisLabelCount;}
		    set { yAxisLabelCount = value;}
	    }
        public int XAxisLabelCount 
        {
            get { return xAxisLabelCount; }
            set { xAxisLabelCount = value; }
        }
        

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
                pl.StrokeThickness = PlotStrokeThickness;
                pl.Stroke = Brushes.Black;
                foreach (MeasurementPoint item in points)       //adding the Point in the list
                {
                    pl.Points.Add(scalingPoint(item.getPoint()));   //editing the points to fit to Canvas an plot 
                }
                can.Children.Clear();                       //clear Can before adding Polyline  
                AddAxis();
                can.Children.Add(pl);
                
            }
            return can;
        }

        /// <summary>
        /// sets offset and scale depending on the plotwidth and height and the points 
        /// doesn't need to be called if resized
        /// </summary>
        public void setScalingAuto()
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
                p.addPoint(new MeasurementPoint(new Point(i-5, 5-i%10), i)); 
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

        private void AddAxis()
        {
            #region xAxis
            //########Line################
            Line xAxis = new Line();
            xAxis.Stroke = Brushes.Black;
            xAxis.StrokeThickness = AxisStrokeThickness;
            xAxis.X1 = 0;
            xAxis.X2 = plotwidth;
            if (ymin <= 0 && ymax > 0)
            {                
                xAxis.Y1 = scalingPoint(new Point(0, 0)).Y;
                xAxis.Y2 = xAxis.Y1; 
            }
            else
            {
                xAxis.Y1 = scalingPoint(new Point(0, ymin)).Y;
                xAxis.Y2 = xAxis.Y1;
            }
            can.Children.Add(xAxis);

            //#########Arrow################
            Polygon pX = new Polygon();
            pX.Fill = Brushes.Black;
            pX.Stroke = Brushes.Black;
            pX.StrokeThickness = AxisStrokeThickness;
            pX.Points.Add(new Point(plotwidth, xAxis.Y1));
            pX.Points.Add(new Point(plotwidth * (1 - arrowpercentage * arrowlengthmultiplier), xAxis.Y1 - plotwidth * arrowpercentage));
            pX.Points.Add(new Point(plotwidth * (1 - arrowpercentage * arrowlengthmultiplier), xAxis.Y1 + plotwidth * arrowpercentage));
            can.Children.Add(pX);

            //#########Labels##############
            for (int i = 0; i < xAxisLabelCount; i++)
            {
                double x; 
                if (xmin <= 0 && xmax > 0)
                {
                    double q = (i + Math.Ceiling(xmin / (xmax - xmin) * (xAxisLabelCount - 1)));
                    x =  q* (xmax - xmin) / (xAxisLabelCount - 1) ;
                }
                else
                {
                    x = i * (xmax - xmin) / (xAxisLabelCount - 1) + xmin;
                }
                
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.StrokeThickness = AxisStrokeThickness;
                Point p = new Point(x, 0);
                p = scalingPoint(p);
                l.X1 = p.X;
                l.X2 = p.X;
                l.Y1 = xAxis.Y1 - LableMarkerLenght / 2;
                l.Y2 = xAxis.Y1 + LableMarkerLenght / 2;
                can.Children.Add(l);

                TextBlock tb = new TextBlock();
                tb.Text= String.Format("{0:f2}",x);
                Canvas.SetLeft(tb, l.X1 + LabelMarginLeftX);
                Canvas.SetTop(tb, l.Y2 + LabelMarginTopX);
                can.Children.Add(tb);
            }
            #endregion

            #region yAxis
            //########Line################
            Line yAxis = new Line();
            yAxis.Stroke = Brushes.Black;
            yAxis.StrokeThickness = AxisStrokeThickness;
            yAxis.Y1 = 0;
            yAxis.Y2 = plotheight;
            if (xmin <= 0 && xmax > 0)
            {
                yAxis.X1 = scalingPoint(new Point(0, 0)).X;
                yAxis.X2 = yAxis.X1;
            }
            else
            {
                yAxis.X1 = scalingPoint(new Point(0, xmin)).X;
                yAxis.X2 = yAxis.X1;
            }
            can.Children.Add(yAxis);

            //#########Arrow################
            Polygon pY = new Polygon();
            pY.Fill = Brushes.Black;
            pY.Stroke = Brushes.Black;
            pY.StrokeThickness = AxisStrokeThickness;
            pY.Points.Add(new Point(yAxis.X1,0));
            pY.Points.Add(new Point(plotwidth * arrowpercentage + yAxis.X1, plotheight * arrowpercentage * arrowlengthmultiplier));
            pY.Points.Add(new Point(-plotwidth * arrowpercentage + yAxis.X1, plotheight * arrowpercentage * arrowlengthmultiplier));
            can.Children.Add(pY);

            //#########Labels##############
            for (int i = 0; i < yAxisLabelCount; i++)
            {
                double y = i * (ymax - ymin) / (yAxisLabelCount - 1) + ymin;
             
                if (ymin <= 0 && ymax > 0)
                {
                    double q = (i + Math.Ceiling(ymin / (ymax - ymin) * (yAxisLabelCount - 1)));
                    y = q * (ymax - ymin) / (yAxisLabelCount - 1);
                }
                else
                {
                   y = i * (ymax - ymin) / (yAxisLabelCount - 1) + ymin;
                }
                Line l = new Line();
                l.Stroke = Brushes.Black;
                l.StrokeThickness = AxisStrokeThickness;
                Point p = new Point(0,y);
                p = scalingPoint(p);
                l.X1 = yAxis.X1 - LableMarkerLenght / 2;
                l.X2 = yAxis.X1 + LableMarkerLenght/2;
                l.Y1 = p.Y;
                l.Y2 = p.Y;
                can.Children.Add(l);

                TextBlock tb = new TextBlock();
                tb.Text = String.Format("{0:f2}", y);
                Canvas.SetLeft(tb, l.X1 + LabelMarginLeftY);
                Canvas.SetTop(tb, l.Y2 + LabelMarginTopY);
                can.Children.Add(tb);
            }
            #endregion
        }
        #endregion

    }
}
