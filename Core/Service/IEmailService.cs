using Core.Shared;

namespace Core.Service
{
    public interface IEmailService
    {
        Result Send(string receiver, string subject, string emailBody);
        Result Send(IEnumerable<string> receivers, string subject, string emailBody);

        Task<Result> SendAsync(string receiver, string subject, string emailBody);
        Task<Result> SendAsync(IEnumerable<string> receivers, string subject, string emailBody);
    }
}
