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

        DispatcherTimer updateTimer;

        long timestamp = 0;

        UInt16 countsOfFPS = 0;

        UInt16 fpsRate = 0;

        Stopwatch sw = new Stopwatch();

        public MainPage()
        {
            this.InitializeComponent();

            this.source = File.ReadAllBytes("frame_nv12.yuv");
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

            this.updateTimer = new DispatcherTimer();

            this.updateTimer.Interval = TimeSpan.FromTicks(11111);

            this.updateTimer.Tick += this.update;

            this.sw.Start();

            this.updateTimer.Start();
        }

        private void CanvasSwapChainPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.swapChain == null) { return; }

            this.swapChain.ResizeBuffers(e.NewSize);
        }

        private void CanvasSwapChainPanel_CompositionScaleChanged(SwapChainPanel sender, object args)
        {
            if (this.swapChain == null) { return; }
        }

        private void update(object sender, object e)
        {
            using (var session = this.renderTarget.CreateDrawingSession())
            {
                unsafe
                {
                    fixed(byte* dataPtr = this.source)
                    {
                        YUVHelper.SharedInstance.DrawImage(session, ((IntPtr)dataPtr).ToInt32(), 4000, 3000);
                    }
                }       
            }

            this.draw();

            timestamp += sw.ElapsedMilliseconds;

            if (this.timestamp >= 1000)
            {
                this.fpsRate = this.countsOfFPS;

                this.countsOfFPS = 0;

                this.timestamp = 0;
            }

            this.sw.Restart();

            GC.Collect();
        }

        private void draw()
        {
            this.countsOfFPS++;

            using (var session = this.swapChain.CreateDrawingSession(Colors.Black))
            {
                session.Antialiasing = CanvasAntialiasing.Aliased;

                if (this.renderTarget != null)
                {
                    session.DrawImage(this.renderTarget, new Rect(new Point(0, 0), this.swapChain.Size));
                }

                session.DrawText("fps: " + this.fpsRate, 0, 0, Colors.Red);
            }

            this.swapChain.Present();
        }
    }
}
