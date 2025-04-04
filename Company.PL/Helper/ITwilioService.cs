using Company.DAL.Models.Sms;
using NuGet.Protocol.Plugins;
using Twilio.Rest.Api.V2010.Account;

namespace Company.PL.Helper
{
    public interface ITwilioService
    {

        public MessageResource SendSms( Sms sms);           


    }
}
