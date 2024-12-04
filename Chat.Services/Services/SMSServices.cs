using Chat.Services.Helper;
using Chat.Services.Interfaces;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Chat.Services.Services
{
    public class SMSServices : ISMSServices
    {
        private readonly TwillioSettings _options;

        public SMSServices(IOptions<TwillioSettings> options)
        {
            _options = options.Value;
        }

        public MessageResource SendSms(SMS sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);
            var result = MessageResource.Create(
                body: sms.Body,
                to: sms.PhoneNumber,
                from: new Twilio.Types.PhoneNumber(_options.Number)
                );

            return result;
        }
    }
}
