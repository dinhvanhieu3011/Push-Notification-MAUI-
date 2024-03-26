using PushNoti.Enum;

namespace PushNoti.Models.Biometric
{
    public class BiometricAuthenticationResult
    {
        public BiometricAuthenticationStatus Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
