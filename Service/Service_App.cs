using System;
using Tizen.Applications;
using Tizen.Applications.Notifications;
using Tizen.NUI;
using Tizen.Sensor;
using Tizen.System;
using static Tizen.NUI.Timer;

namespace Service
{
    public class App : ServiceApplication
    {
        private const uint TimeoutSec = 10;

        private string SharedPath { get; set; }

        private Timer Timer { get; set; }
        private LinearAccelerationSensor LinearAcceleration { get; set; }

        protected override void OnCreate()
        {
            base.OnCreate();
            Power.RequestLock(PowerLock.Cpu, 0);

            DirectoryInfo info = Current.DirectoryInfo;
            SharedPath = info.SharedResource;

            LinearAcceleration = new LinearAccelerationSensor();
            LinearAcceleration.DataUpdated += LinearAcceleration_DataUpdated;
            LinearAcceleration.Start();

            Timer = new Timer(TimeoutSec * 1000);
            Timer.Tick += OnTimedEvent;
            Timer.Start();
        }

        private void LinearAcceleration_DataUpdated(object sender, LinearAccelerationSensorDataUpdatedEventArgs e)
        {
            var resultant = (float)Math.Sqrt(e.X * e.X + e.Y * e.Y * e.Z * e.Z);
            if (CheckForActivity(resultant))
            {
                Timer.Start();
            }
        }

        private bool CheckForActivity(float resultant)
        {
            return resultant > 10;
        }

        private bool OnTimedEvent(object sender, TickEventArgs e)
        {
            var notification = new Notification
            {
                Title = "Wykryty bezruch",
                Content = $"Od {TimeoutSec} sekund nie wykryto ruchu.",
                Count = 1,
                Icon = $"{SharedPath}Service.png",
                Tag = "timeout",
                Accessory = new Notification.AccessorySet
                {
                    SoundOption = AccessoryOption.On,
                    CanVibrate = true
                }
            };

            notification.AddStyle(new Notification.ActiveStyle
            {
                IsAutoRemove = false
            });
            NotificationManager.Post(notification);

            return true;
        }

        protected override void OnAppControlReceived(AppControlReceivedEventArgs e)
        {
            base.OnAppControlReceived(e);
            // Reply to a launch request
            ReceivedAppControl receivedAppControl = e.ReceivedAppControl;
            
            string operation;
            try
            {
                operation = receivedAppControl.ExtraData.Get<string>("app");
            }
            catch
            {
                operation = null;
            }

            if (receivedAppControl.IsReplyRequest)
            {
                AppControl replyRequest = new AppControl();
                receivedAppControl.ReplyToLaunchRequest(replyRequest, AppControlReplyResult.Succeeded);

                if (operation == "kill")
                {
                    Exit();
                }
            }
        }

        protected override void OnDeviceOrientationChanged(DeviceOrientationEventArgs e)
        {
            base.OnDeviceOrientationChanged(e);
        }

        protected override void OnLocaleChanged(LocaleChangedEventArgs e)
        {
            base.OnLocaleChanged(e);
        }

        protected override void OnLowBattery(LowBatteryEventArgs e)
        {
            base.OnLowBattery(e);
        }

        protected override void OnLowMemory(LowMemoryEventArgs e)
        {
            base.OnLowMemory(e);
        }

        protected override void OnRegionFormatChanged(RegionFormatChangedEventArgs e)
        {
            base.OnRegionFormatChanged(e);
        }

        protected override void OnTerminate()
        {
            Timer.Dispose();
            LinearAcceleration.Stop();
            Power.ReleaseLock(PowerLock.Cpu);
            base.OnTerminate();
        }

        static void Main(string[] args)
        {
            App app = new App();
            app.Run(args);
        }
    }
}
