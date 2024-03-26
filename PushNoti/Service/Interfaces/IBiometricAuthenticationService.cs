using PushNoti.Models.Biometric;

namespace PushNoti.Service.Interfaces
{
    public interface IBiometricAuthenticationService
    {
        public Task<bool> CheckIfBiometricsAreAvailableAsync();
        public Task<BiometricAuthenticationResult> AuthenticateAsync(string title, string message);
    }
}
