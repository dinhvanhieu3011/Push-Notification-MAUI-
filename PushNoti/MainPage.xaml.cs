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

   
    }
}