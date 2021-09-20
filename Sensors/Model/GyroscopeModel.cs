namespace Sensors.Model
{
    /// <summary>
    /// GyroscopeModel class.
    /// </summary>
    public class GyroscopeModel : BaseSensorModel
    {
        private float x;

        private float y;

        private float z;

        /// <summary>
        /// Property for actual X value.
        /// </summary>
        public float X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for actual Y value.
        /// </summary>
        public float Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property for actual Z value.
        /// </summary>
        public float Z
        {
            get { return z; }
            set
            {
                z = value;
                OnPropertyChanged();
            }
        }
    }
}