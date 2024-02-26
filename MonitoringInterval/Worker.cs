using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using AcerProProject1.Data;
namespace MonitoringInterval
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ApplicationDbContext _dbContext;

        public Worker(ILogger<Worker> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var targetApps = await _dbContext.TargetApis.ToListAsync(stoppingToken);
                foreach (var targetApp in targetApps)
                {
                    using (var httpClient = new HttpClient())
                    {
                        try
                        {
                            HttpResponseMessage response = await httpClient.GetAsync(targetApp.Url, stoppingToken);
                            if (!response.IsSuccessStatusCode)
                            {
                                await SendEmail("Target app is down!", $"The target app {targetApp.Url} returned status code {response.StatusCode}");
                            }
                            _logger.LogInformation($"[{DateTime.Now}] {targetApp.Url} is up and running.");
                        }
                        catch (HttpRequestException)
                        {
                            await SendEmail("Target app is down!", $"The target app {targetApp.Url} could not be reached.");
                            _logger.LogInformation($"[{DateTime.Now}] {targetApp.Url} is down.");
                        }
                    }
                    await Task.Delay(TimeSpan.FromSeconds(targetApp.MonitoringInterval), stoppingToken);
                }
            }
        }

        private async Task SendEmail(string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mailkit", "mailkitforacer@gmail.com"));
            message.To.Add(new MailboxAddress("", "arianaswan003@gmail.com"));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("mailkitforacer@gmail.com", "a.A123456");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
