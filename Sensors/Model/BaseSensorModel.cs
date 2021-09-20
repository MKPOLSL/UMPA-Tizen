using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sensors.Model
{
    public abstract class BaseSensorModel : INotifyPropertyChanged
    {
        private bool isSupported;
        private int sensorCount;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSupported
        {
            get => isSupported;
            set
            {
                isSupported = value;
                OnPropertyChanged();
            }
        }

        public int SensorCount
        {
            get => sensorCount;
            set
            {
                sensorCount = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value,
        [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}