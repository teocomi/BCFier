using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bcfier.Bcf.Bcf2;
using log4net;
using Xbim.Presentation;

namespace Bcfier.XbimXplorer
{
    /// <summary>
    /// Interaction logic for AddViewRevit.xaml
    /// </summary>
    public partial class AddViewXbim 
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(AddViewXbim));
        private readonly DrawingControl3D _control;

        public AddViewXbim(Markup issue, string bcfTempFolder, DrawingControl3D control)
        {
            try
            {
                InitializeComponent();
                AddViewControl.Issue = issue;
                AddViewControl.TempFolder = bcfTempFolder;
                _control = control;

                AddViewControl.TextBlockInfo.Text =
                    "3D/2D information of the current view will be included in the viewpoint";

                GetRevitSnapshot();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!", "exception: " + ex);
            }
        }

        private static RenderTargetBitmap Get3DVisual(FrameworkElement element)
        {
            var width = element.ActualWidth;

            var height = element.ActualHeight;
            var bmpCopied = new RenderTargetBitmap((int) Math.Round(width), (int) Math.Round(height), 96, 96,
                PixelFormats.Default);
            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                var vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), new Size(width, height)));
            }
            bmpCopied.Render(dv);
            return bmpCopied;
        }

        private bool SaveControlPng(FrameworkElement control, string outFile)
        {
            try
            {
                var renderTargetBitmap = Get3DVisual(control);

                var bitmapEncoder = new PngBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                using (var stream = File.Create(outFile))
                {
                    bitmapEncoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Counld not create screenshot from model.", ex);               
                return false;
            }
            return true;
        }

        private void GetRevitSnapshot()
        {
            try
            {
                var tempImg = Path.Combine(Path.GetTempPath(), "BCFier", Path.GetTempFileName() + ".png");

                var success = SaveControlPng(_control, tempImg);
                if (!success)
                {
                    return;
                }
                AddViewControl.AddViewpoint(tempImg);
                File.Delete(tempImg);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!", "exception: " + ex);
            }
        }
    }
}