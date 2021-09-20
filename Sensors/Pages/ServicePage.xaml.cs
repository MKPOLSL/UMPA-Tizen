using Sensors.Model;
using Tizen.Applications;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace Sensors.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServicePage : CirclePage
    {
        public ServiceModel Model { get; private set; }

        public ServicePage()
        {
            Model = new ServiceModel();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            AppControl appcontrol = new AppControl
            {
                ApplicationId = "umpa.tizen.Service",
                Operation = AppControlOperations.Default,
            };

            if (!Model.IsRunning)
            {
                appcontrol.ExtraData.Add("app", "run");
                AppControl.SendLaunchRequest(appcontrol, (launchRequest, replyRequest, result) =>
                {
                    switch (result)
                    {
                        case AppControlReplyResult.Succeeded:
                            Model.Message = "Usługa została pomyślnie uruchomiona.";
                            Model.IsRunning = true;
                            break;
                        case AppControlReplyResult.Failed:
                            Model.Message = "Nie udało się uruchomić usługi.";
                            Model.IsRunning = false;
                            break;
                        case AppControlReplyResult.AppStarted:
                            Model.Message = "Usługa już działa.";
                            Model.IsRunning = true;
                            break;
                        case AppControlReplyResult.Canceled:
                            Model.Message = "Uruchomienie usługi zostało anulowane.";
                            Model.IsRunning = false;
                            break;
                    }
                });
            }
            else
            {
                appcontrol.ExtraData.Add("app", "kill");
                AppControl.SendLaunchRequest(appcontrol, (launchRequest, replyRequest, result) =>
                {
                    switch (result)
                    {
                        case AppControlReplyResult.Succeeded:
                            Model.Message = "Usługa została zatrzymana.";
                            Model.IsRunning = false;
                            break;
                        case AppControlReplyResult.Failed:
                            Model.Message = "Nie udało się zatrzymać usługi.";
                            break;
                        case AppControlReplyResult.Canceled:
                            Model.Message = "Zatrzymanie usługi zostało anulowane.";
                            break;
                    }
                });
            }
        }
    }
}