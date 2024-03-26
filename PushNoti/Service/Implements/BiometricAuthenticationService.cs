using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using PushNoti.Service.Interfaces;
using PushNoti.Models.Biometric;
using PushNoti.Enum;

namespace PushNoti.Service.Implements
{
    public class BiometricAuthenticationService : IBiometricAuthenticationService
    {
        public Task<bool> CheckIfBiometricsAreAvailableAsync()
        {
            return CrossFingerprint.Current.IsAvailableAsync();
        }

        public async Task<BiometricAuthenticationResult> AuthenticateAsync(string title, string message)
        {
            var request = new AuthenticationRequestConfiguration
                (title, message)
            {
                AllowAlternativeAuthentication = true,
            };

            var authResult = await CrossFingerprint.Current.AuthenticateAsync(request);

            var result = new BiometricAuthenticationResult()
            {
                Status = ToBiometricAuthenticationStatus(authResult.Status),
                ErrorMessage = authResult.ErrorMessage
            };

            return result;
        }

        private static BiometricAuthenticationStatus ToBiometricAuthenticationStatus(FingerprintAuthenticationResultStatus fingerprintStatus)
        {
            return fingerprintStatus switch
            {
                FingerprintAuthenticationResultStatus.Succeeded => BiometricAuthenticationStatus.Success,
                FingerprintAuthenticationResultStatus.FallbackRequested => BiometricAuthenticationStatus.FallbackRequest,
                FingerprintAuthenticationResultStatus.Failed => BiometricAuthenticationStatus.Failed,
                FingerprintAuthenticationResultStatus.Canceled => BiometricAuthenticationStatus.Canceled,
                FingerprintAuthenticationResultStatus.TooManyAttempts => BiometricAuthenticationStatus.TooManyAttempts,
                FingerprintAuthenticationResultStatus.NotAvailable => BiometricAuthenticationStatus.NotAvailable,
                FingerprintAuthenticationResultStatus.Denied => BiometricAuthenticationStatus.Denied,
                _ => BiometricAuthenticationStatus.Unknown
            };
        }
    }
}
