using Sensors.Extensions;
using Sensors.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Tizen.Sensor;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace Sensors.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GyroscopePage : CirclePage
    {
        public GyroscopePage()
        {
            Model = new GyroscopeModel
            {
                IsSupported = Gyroscope.IsSupported,
                SensorCount = Gyroscope.Count
            };

            InitializeComponent();

            if (Model.IsSupported)
            {
                Gyroscope = new Gyroscope();
                Gyroscope.DataUpdated += Gyroscope_DataUpdated;

                canvas.ChartScale = 1000;
                canvas.Series = new List<Series>()
                {
                    new Series()
                    {
                        Color = SKColors.Red,
                        Name = "X",
                        FormattedText = "X={0:f2}°/s",
                    },
                    new Series()
                    {
                        Color = SKColors.Green,
                        Name = "Y",
                        FormattedText = "Y={0:f2}°/s",
                    },
                    new Series()
                    {
                        Color = SKColors.Blue,
                        Name = "Z",
                        FormattedText = "Z={0:f2}°/s",
                    },
                };
            }
        }

        public Gyroscope Gyroscope { get; private set; }

        public GyroscopeModel Model { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Gyroscope?.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Gyroscope?.Stop();
        }

        private void Gyroscope_DataUpdated(object sender, GyroscopeDataUpdatedEventArgs e)
        {
            Model.X = e.X;
            Model.Y = e.Y;
            Model.Z = e.Z;

            long ticks = DateTime.UtcNow.Ticks;
            foreach (var serie in canvas.Series)
            {
                switch (serie.Name)
                {
                    case "X":
                        serie.Points.Add(new Extensions.Point() { Ticks = ticks, Value = e.X });
                        break;

                    case "Y":
                        serie.Points.Add(new Extensions.Point() { Ticks = ticks, Value = e.Y });
                        break;

                    case "Z":
                        serie.Points.Add(new Extensions.Point() { Ticks = ticks, Value = e.Z });
                        break;
                }
            }
            canvas.InvalidateSurface();
        }
    }
}