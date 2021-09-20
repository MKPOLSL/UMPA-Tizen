using System;
using System.Runtime.CompilerServices;
using Tizen.System;

namespace Sensors.Model
{
    public class LinearAccelerationModel : BaseSensorModel
    {
        public Feedback Feedback { get; set; }

        private string accuracy;
        private float x;
        private float y;
        private float z;
        private float resultant;
        private string timer;
        public string Timer { get => timer; set => SetProperty(ref timer, value); }

        public LinearAccelerationModel()
        {
            Feedback = new Feedback();
        }

        public string Accuracy
        {
            get { return accuracy; }
            set
            {
                accuracy = value;
                OnPropertyChanged();
            }
        }

        public float X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        public float Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged();
                
            }
        }

        public float Z
        {
            get { return z; }
            set
            {
                z = value;
                OnPropertyChanged();
            }
        }

        public float Resultant { get => resultant; set => SetProperty(ref resultant, value); }
    }
}