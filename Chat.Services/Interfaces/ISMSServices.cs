using Chat.Services.Helper;
using Twilio.Rest.Api.V2010.Account;

namespace Chat.Services.Interfaces
{
    public interface ISMSServices
    {
        MessageResource SendSms(SMS sms);

    }
}
