using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace OpenProject.Windows
{
    /// <summary>
    /// Simple image viewer, a bit buggy
    /// </summary>
    /// 

    public partial class SnapWin : Window
    {

        private Point origin;
        private Point start;
        public SnapWin(string s)
        {
            InitializeComponent();
            DataContext = s;

            TransformGroup group = new TransformGroup();

            ScaleTransform xform = new ScaleTransform();
            group.Children.Add(xform);

            TranslateTransform tt = new TranslateTransform();
            group.Children.Add(tt);

            snap.RenderTransform = group;

            SnapWindow.MouseWheel += image_MouseWheel;
            SnapWindow.MouseLeftButtonDown += image_MouseLeftButtonDown;
            SnapWindow.MouseLeftButtonUp += image_MouseLeftButtonUp;
            SnapWindow.MouseMove += image_MouseMove;

        }
        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
          snap.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
          if (!snap.IsMouseCaptured) return;

          var tt = (TranslateTransform)((TransformGroup)snap.RenderTransform).Children.First(tr => tr is TranslateTransform);
          var v = start - e.GetPosition(border);
          tt.X = origin.X - v.X;
          tt.Y = origin.Y - v.Y;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          snap.CaptureMouse();
          var tt = (TranslateTransform)((TransformGroup)snap.RenderTransform).Children.First(tr => tr is TranslateTransform);
          start = e.GetPosition(border);
          origin = new Point(tt.X, tt.Y);
        }

        private void image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
          var mousePosition = e.GetPosition(snap);
          snap.RenderTransformOrigin = new Point(mousePosition.X / snap.ActualWidth, mousePosition.Y / snap.ActualHeight); 
        
          var transformGroup = (TransformGroup)snap.RenderTransform;
          var transform = (ScaleTransform)transformGroup.Children[0];
          
          
          double zoom = e.Delta > 0 ? .2 : -.2;
          if ((transform.ScaleX > 0.21 && zoom < 0) || (zoom > 0 && transform.ScaleX < 4))
          {
            transform.ScaleX += zoom;
            transform.ScaleY += zoom;
          }

          Console.WriteLine(transform.ScaleX);
        }
    }

}
