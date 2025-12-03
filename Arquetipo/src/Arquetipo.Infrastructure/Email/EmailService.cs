


using Arquetipo.Application.Abstractions.Email;

namespace Arquetipo.Infrastructure;

internal sealed class EmailService : IEmailService
{
    public Task SendAsync(string from, string to, string subject, string body)
    {
        return Task.CompletedTask;
    }
}