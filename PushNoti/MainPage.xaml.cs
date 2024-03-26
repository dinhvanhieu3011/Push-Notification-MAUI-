using CommunityToolkit.Mvvm.Input;
using Plugin.Fingerprint.Abstractions;
using PushNoti.Enum;
using PushNoti.Service.Implements;
using PushNoti.Service.Interfaces;
namespace PushNoti;

public partial class MainPage : ContentPage
{

    public MainPage()
	{
		InitializeComponent();
	}

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        IBiometricAuthenticationService _authService = new BiometricAuthenticationService();

        var isBiometricAuthAvailable = await _authService.CheckIfBiometricsAreAvailableAsync();

        if (isBiometricAuthAvailable)
        {
            var authResult =
                await _authService.AuthenticateAsync("FaceID", "App requires FaceID in order to login");

            switch (authResult.Status)
            {
                case BiometricAuthenticationStatus.Success:
                    // handle success state
                    await Shell.Current.GoToAsync("///MainView", true);
                    break;
                case BiometricAuthenticationStatus.Failed:
                    // handle failed state
                    break;
                case BiometricAuthenticationStatus.Denied:
                    // handle denied state
                    break;
                case BiometricAuthenticationStatus.Unknown:
                case BiometricAuthenticationStatus.FallbackRequest:
                case BiometricAuthenticationStatus.Canceled:
                case BiometricAuthenticationStatus.TooManyAttempts:
                case BiometricAuthenticationStatus.NotAvailable:
                default:
                    // handle other states
                    break;
            }
        }
        else
        {
            // handle not available state
            await Shell.Current.GoToAsync("///UserNameAndPasswordView", true);
        }
    }
}