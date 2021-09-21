using Sensors.Extensions;
using Sensors.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Tizen.Security;
using Tizen.Sensor;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace Sensors.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HRMPage : CirclePage
    {
        private const string hrmPrivilege = "http://tizen.org/privilege/healthinfo";

        public HRMPage()
        {
            Model = new HRMModel
            {
                IsSupported = HeartRateMonitor.IsSupported,
                SensorCount = HeartRateMonitor.Count
            };

            InitializeComponent();

            SetupPrivilegeHandler();
            CheckResult result = PrivacyPrivilegeManager.CheckPermission(hrmPrivilege);
            switch (result)
            {
                case CheckResult.Allow:
                    CreateHRM();
                    break;

                case CheckResult.Deny:
                    break;

                case CheckResult.Ask:
                    PrivacyPrivilegeManager.RequestPermission(hrmPrivilege);
                    break;
            }
        }

        public HeartRateMonitor HRM { get; private set; }

        public HRMModel Model { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupPrivilegeHandler();
            HRM?.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HRM?.Stop();
        }

        private void CreateHRM()
        {
            if (Model.IsSupported)
            {
                HRM = new HeartRateMonitor();
                HRM.DataUpdated += HRM_DataUpdated;

                canvas.ChartScale = 200;
                canvas.Series = new List<Series>()
                {
                    new Series()
                    {
                        Color = SKColors.Red,
                        Name = "HeartRate",
                        FormattedText = "Heart Rate={0}",
                    },
                };
            }
        }

        private void HRM_DataUpdated(object sender, HeartRateMonitorDataUpdatedEventArgs e)
        {
            Model.HeartRate = e.HeartRate;
            canvas.Series[0].Points.Add(new Extensions.Point() { Ticks = DateTime.UtcNow.Ticks, Value = e.HeartRate });
            canvas.InvalidateSurface();
        }

        private void PrivilegeResponseHandler(object sender, RequestResponseEventArgs e)
        {
            if (e.cause == CallCause.Error)
            {
                return;
            }

            switch (e.result)
            {
                case RequestResult.AllowForever:
                    CreateHRM();
                    HRM.Start();
                    break;

                case RequestResult.DenyForever:
                case RequestResult.DenyOnce:
                    break;
            }
        }

        private void SetupPrivilegeHandler()
        {
            PrivacyPrivilegeManager.ResponseContext context = null;
            if (PrivacyPrivilegeManager.GetResponseContext(hrmPrivilege).TryGetTarget(out context))
            {
                context.ResponseFetched += PrivilegeResponseHandler;
            }
        }
    }
}