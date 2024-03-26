using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushNoti.Enum
{
    public enum BiometricAuthenticationStatus
    {
        Unknown,
        Success,
        FallbackRequest,
        Failed,
        Canceled,
        TooManyAttempts,
        NotAvailable,
        Denied
    }
}
