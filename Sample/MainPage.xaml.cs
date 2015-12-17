using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Diagnostics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Win2D.YUV;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        byte[] source;

        CanvasDevice device;

        CanvasSwapChain swapChain;

        CanvasRenderTarget renderTarget;

        public MainPage()
        {
            this.InitializeComponent();

            this.source = File.ReadAllBytes("frame_nv12.yuv");

            YUVHelper.SharedInstance.DrawImage(null, this.source, 4000, 3000);
        }

        private void CanvasSwapChainPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var swapChainPanel = sender as CanvasSwapChainPanel;

            float width = (float)swapChainPanel.ActualWidth;

            float height = (float)swapChainPanel.ActualHeight;

            this.device = YUVHelper.SharedInstance.Device;

            this.swapChain = new CanvasSwapChain(this.device, width, height, 96);

            swapChainPanel.SwapChain = this.swapChain;

            this.renderTarget = new CanvasRenderTarget(this.device, width, height, 96);

            YUVHelper.SharedInstance.DrawImage(this.renderTarget.CreateDrawingSession(), this.source, 4000, 3000);

            //unsafe
            //{
            //    fixed(byte* dataPtr = this.source)
            //    {
            //        YUVHelper.SharedInstance.DrawImage(this.renderTarget.CreateDrawingSession(), ((IntPtr)dataPtr).ToInt32(), 4000, 3000);
            //    }
            //}

            this.draw();
        }

        private void CanvasSwapChainPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.swapChain == null) { return; }

            this.swapChain.ResizeBuffers(e.NewSize);

            this.draw();
        }

        private void CanvasSwapChainPanel_CompositionScaleChanged(SwapChainPanel sender, object args)
        {
            if (this.swapChain == null) { return; }

            this.draw();
        }

        private void draw()
        {
            using (var session = this.swapChain.CreateDrawingSession(Colors.Black))
            {
                session.DrawImage(this.renderTarget);
            }

            this.swapChain.Present();
        }
    }
}
