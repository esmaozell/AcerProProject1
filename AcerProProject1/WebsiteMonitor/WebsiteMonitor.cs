using AcerProProject1.Data;
using AcerProProject1.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;

public class WebsiteMonitor
{
    private readonly ApplicationDbContext dbContext;
    private static readonly HttpClient HttpClient = new HttpClient();
    private static Timer _timer;
    private static readonly string Email = "mail@example.com";

    public WebsiteMonitor(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task StartMonitoring()
    {
        var targetApps = await dbContext.TargetApis.ToListAsync();
        foreach (var targetApp in targetApps)
        {
            _timer = new Timer(CheckTargetApp, targetApp, TimeSpan.Zero, TimeSpan.FromSeconds(targetApp.MonitoringInterval));
        }
    }

    private async void CheckTargetApp(object state)
    {
        var targetApp = (TargetAPI)state;
        try
        {
            HttpResponseMessage response = await HttpClient.GetAsync(targetApp.Url);
            if (!response.IsSuccessStatusCode)
            {
                await SendEmail(Email, $"Target app {targetApp.Url} is down!", $"The target app {targetApp.Url} returned status code {response.StatusCode}");
            }
            Console.WriteLine($"[{DateTime.Now}] {targetApp.Url} is up and running.");
        }
        catch (HttpRequestException)
        {
            await SendEmail(Email, $"Target app {targetApp.Url} is down!", $"The target app {targetApp.Url} could not be reached.");
            Console.WriteLine($"[{DateTime.Now}] {targetApp.Url} is down.");
        }
    }

    private async Task SendEmail(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Ariana", "arianaswan003@gmail.com"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("Ariana", "arianaswan003@gmail.com");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
