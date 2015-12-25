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

        List<IDirect3DSurface> surfaces = new List<IDirect3DSurface>();

        DispatcherTimer timer = new DispatcherTimer();

        int fps = 0;

        int fpsCount = 0;

        Size renderSize = new Size(448, 252);

        System.Threading.Timer taskTimer;

        System.Threading.Timer taskTimer2;

        System.Threading.Timer taskTimer3;

        System.Threading.Timer taskTimer4;

        System.Threading.Timer taskTimer5;

        System.Threading.Timer taskTimer6;

        System.Threading.Timer taskTimer7;

        System.Threading.Timer taskTimer8;

        System.Threading.Timer taskTimer9;

        System.Threading.Timer taskTimer10;

        System.Threading.Timer taskTimer11;

        System.Threading.Timer taskTimer12;

        System.Threading.Timer taskTimer13;

        System.Threading.Timer taskTimer14;

        System.Threading.Timer taskTimer15;

        System.Threading.Timer taskTimer16;

        System.Threading.Timer taskTimer17;

        System.Threading.Timer taskTimer18;

        System.Threading.Timer taskTimer19;

        System.Threading.Timer taskTimer20;

        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += this.fpsTask;

            timer.Start();

            for (int i = 0; i < 64; i++)
            {
                unsafe
                {
                    fixed (byte* dataPtr = this.source)
                    {
                        this.surfaces.Add(YUVHelper.SharedInstance.CreateDirect3DSurface(((IntPtr)dataPtr).ToInt32(), (int)this.renderSize.Width, (int)this.renderSize.Height));
                    }
                }
            }

            System.Threading.TimerCallback callback = new System.Threading.TimerCallback(timeTask);

            taskTimer = new System.Threading.Timer(callback, null, 0, -1);

            System.Threading.TimerCallback callback2 = new System.Threading.TimerCallback(timeTask2);

            taskTimer2 = new System.Threading.Timer(callback2, null, 0, -1);

            System.Threading.TimerCallback callback3 = new System.Threading.TimerCallback(timeTask3);

            taskTimer3 = new System.Threading.Timer(callback3, null, 0, -1);

            System.Threading.TimerCallback callback4 = new System.Threading.TimerCallback(timeTask4);

            taskTimer4 = new System.Threading.Timer(callback4, null, 0, -1);

            System.Threading.TimerCallback callback5 = new System.Threading.TimerCallback(timeTask5);

            taskTimer5 = new System.Threading.Timer(callback5, null, 0, -1);

            System.Threading.TimerCallback callback6 = new System.Threading.TimerCallback(timeTask6);

            taskTimer6 = new System.Threading.Timer(callback6, null, 0, -1);

            System.Threading.TimerCallback callback7 = new System.Threading.TimerCallback(timeTask7);

            taskTimer7 = new System.Threading.Timer(callback7, null, 0, -1);

            System.Threading.TimerCallback callback8 = new System.Threading.TimerCallback(timeTask8);

            taskTimer8 = new System.Threading.Timer(callback8, null, 0, -1);

            System.Threading.TimerCallback callback9 = new System.Threading.TimerCallback(timeTask9);

            taskTimer9 = new System.Threading.Timer(callback9, null, 0, -1);

            System.Threading.TimerCallback callback10 = new System.Threading.TimerCallback(timeTask10);

            taskTimer10 = new System.Threading.Timer(callback10, null, 0, -1);

            System.Threading.TimerCallback callback11 = new System.Threading.TimerCallback(timeTask11);

            taskTimer11 = new System.Threading.Timer(callback11, null, 0, -1);

            System.Threading.TimerCallback callback12 = new System.Threading.TimerCallback(timeTask12);

            taskTimer12 = new System.Threading.Timer(callback12, null, 0, -1);

            System.Threading.TimerCallback callback13 = new System.Threading.TimerCallback(timeTask13);

            taskTimer13 = new System.Threading.Timer(callback13, null, 0, -1);

            System.Threading.TimerCallback callback14 = new System.Threading.TimerCallback(timeTask14);

            taskTimer14 = new System.Threading.Timer(callback14, null, 0, -1);

            System.Threading.TimerCallback callback15 = new System.Threading.TimerCallback(timeTask15);

            taskTimer15 = new System.Threading.Timer(callback15, null, 0, -1);

            System.Threading.TimerCallback callback16 = new System.Threading.TimerCallback(timeTask16);

            taskTimer16 = new System.Threading.Timer(callback16, null, 0, -1);

            System.Threading.TimerCallback callback17 = new System.Threading.TimerCallback(timeTask17);

            taskTimer17 = new System.Threading.Timer(callback17, null, 0, -1);

            System.Threading.TimerCallback callback18 = new System.Threading.TimerCallback(timeTask18);

            taskTimer18 = new System.Threading.Timer(callback18, null, 0, -1);

            System.Threading.TimerCallback callback19 = new System.Threading.TimerCallback(timeTask19);

            taskTimer19 = new System.Threading.Timer(callback19, null, 0, -1);

            System.Threading.TimerCallback callback20 = new System.Threading.TimerCallback(timeTask20);

            taskTimer20 = new System.Threading.Timer(callback20, null, 0, -1);
        }

        private void timeTask(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask2(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer2.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask3(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer3.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask4(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer4.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask5(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer5.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask6(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer6.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask7(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer7.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask8(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer8.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask9(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer9.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask10(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer10.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask11(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer11.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask12(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer12.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask13(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer13.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask14(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer14.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask15(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer15.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask16(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer16.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask17(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer17.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask18(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer18.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask19(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer19.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void timeTask20(Object stateInfo)
        {
            long j = 0;

            for (int i = 0; i < 2000000; i++)
            {
                j += 1;
            }

            this.taskTimer20.Change(TimeSpan.FromTicks(333333), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void fpsTask(object sender, object e)
        {
            this.fps = this.fpsCount;

            this.fpsCount = 0;
        }

        private void CanvasAnimatedControl_Draw(Microsoft.Graphics.Canvas.UI.Xaml.ICanvasAnimatedControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs args)
        {
            using (var session = args.DrawingSession)
            {
                session.Antialiasing = CanvasAntialiasing.Aliased;

                float unitWidth = (float)sender.Size.Width / 8;

                float unitHeight = (float)sender.Size.Height / 8;

                int index = 0;

                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        Rect destinationRect = new Rect(unitWidth * col, unitHeight * row, unitWidth, unitHeight);

                        Rect sourceRect = new Rect(0, 0, this.renderSize.Width, this.renderSize.Height);

                        YUVHelper.SharedInstance.DrawImage(session, this.surfaces.ElementAt(index), destinationRect, sourceRect);
                    }

                    index++;
                }

                session.DrawText(" FPS: " + this.fps, 0, 0, Colors.Red);
            }

            this.fpsCount++;
        }

        private void CanvasAnimatedControl_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasAnimatedControl control = sender as CanvasAnimatedControl;

            control.TargetElapsedTime = TimeSpan.FromTicks(333333);

            control.CustomDevice = YUVHelper.SharedInstance.Device;
        }
    }
}
