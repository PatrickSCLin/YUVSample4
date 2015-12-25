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

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using Windows.UI;
using Windows.Graphics.DirectX.Direct3D11;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.Graphics.Canvas.Text;
using Win2D.YUV;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        byte[] source = File.ReadAllBytes("nv12_448_252.yuv");

        List<Timer> updateTimers = new List<Timer>();

        List<Timer> TaskTimers = new List<Timer>();

        DispatcherTimer timer = new DispatcherTimer();

        int fps = 0;

        int fpsCount = 0;

        float unitWidth = 0;

        float unitHeight = 0;

        Size renderSize = new Size(448, 252);

        CanvasRenderTarget renderTarget;

        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += this.fpsTask;

            timer.Start();
        }

        private void busyTask(Object stateInfo)
        {
            int index = (int)stateInfo;

            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.TaskTimers.ElementAt(index).Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask(Object stateInfo)
        {
            int index = (int)stateInfo;

            using (var locker = this.renderTarget.Device.Lock())
            using (var session = this.renderTarget.CreateDrawingSession())
            {
                int row = (int)(index / 8);

                int col = (int)(index % 8);

                Rect destinationRect = new Rect(col * unitWidth, row * unitHeight, unitWidth, unitHeight);

                Rect sourceRect = new Rect(0, 0, 448, 252);

                unsafe
                {
                    fixed (byte* dataPtr = this.source)
                    {
                        YUVHelper.SharedInstance.DrawImage(session, ((IntPtr)dataPtr).ToInt32(), destinationRect, sourceRect);
                    }
                }
            }

            this.updateTimers.ElementAt(index).Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void fpsTask(object sender, object e)
        {
            this.fps = this.fpsCount;

            this.fpsCount = 0;
        }

        private void CanvasAnimatedControl_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasAnimatedControl control = sender as CanvasAnimatedControl;

            control.TargetElapsedTime = TimeSpan.FromTicks(333333);

            control.CustomDevice = YUVHelper.SharedInstance.Device;
        }

        private void CanvasAnimatedControl_CreateResources(CanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            this.renderTarget = new CanvasRenderTarget(YUVHelper.SharedInstance.Device, (int)sender.Size.Width, (int)sender.Size.Height, 96);

            for (int i = 0; i < 64; i++)
            {
                TimerCallback callback = new TimerCallback(timeTask);

                Timer timer = new Timer(callback, i, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

                this.updateTimers.Add(timer);
            }

            for (int i = 0; i < 20; i++)
            {
                TimerCallback callback = new TimerCallback(busyTask);

                Timer timer = new Timer(callback, i, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

                this.TaskTimers.Add(timer);
            }
        }

        private void CanvasAnimatedControl_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            
        }

        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                session.Antialiasing = CanvasAntialiasing.Aliased;  

                session.DrawImage(this.renderTarget);

                session.DrawText(" FPS: " + this.fps, 0, 0, Colors.Red);
            }

            this.fpsCount++;
        }

        private void CanvasAnimatedControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.renderTarget = new CanvasRenderTarget(YUVHelper.SharedInstance.Device, (float)e.NewSize.Width, (float)e.NewSize.Height, 96);

            this.unitWidth = (float)e.NewSize.Width / 8;

            this.unitHeight = (float)e.NewSize.Height / 8;
        }
    }
}
