namespace Arquetipo.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync( string from, string to, string subject, string body);
}