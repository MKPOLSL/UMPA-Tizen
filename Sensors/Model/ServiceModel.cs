namespace Sensors.Model
{
    public class ServiceModel : BaseSensorModel
    {
        private string message;
        private bool isRunning;

        public string Message { get => message; set => SetProperty(ref message, value); }
        public bool IsRunning { get => isRunning; set => SetProperty(ref isRunning, value); }
    }
}
