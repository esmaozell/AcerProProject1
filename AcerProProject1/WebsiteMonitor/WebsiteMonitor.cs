using AcerProProject1.Data;
using AcerProProject1.Models;
using AcerProProject1.Repository.IRepository;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;




public class WebsiteMonitor : BackgroundService
{
    private readonly ILogger<WebsiteMonitor> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly HttpClient _httpClient;

    public WebsiteMonitor(ILogger<WebsiteMonitor> logger, ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _httpClient = new HttpClient();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var targetApps = await dbContext.TargetApis
                .Select(t => new
                {
                    Url = t.Url,
                    MonitoringInterval = t.MonitoringInterval
                })
                .AsNoTracking()
                .ToListAsync();

            foreach (var app in targetApps)
            {
                var url = app.Url;
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    url = "https://" + url;
                }

                try
                {

                    if (string.IsNullOrEmpty(url))
                    {
                        Console.WriteLine("URL is empty or null.");
                        return;
                    }

                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (HttpRequestException ex)
                {

                    SendEmail("Target app is down!", $"The target app {url} could not be reached.");
                    _logger.LogInformation($"[{DateTime.Now}] {url} is down.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
                
                await Task.Delay(TimeSpan.FromMinutes(app.MonitoringInterval), stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _httpClient.Dispose();
        await base.StopAsync(cancellationToken);
    }

    private void SendEmail(string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mailkit", "mailkitforacer@gmail.com"));
            message.To.Add(new MailboxAddress("Ben", "arianaswan003@gmail.com"));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("mailkitforacer@gmail.com", "a.A123456");
                client.Send(message);
                client.Disconnect(true);
                Console.WriteLine("E-posta başarıyla gönderildi.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("E-posta gönderilirken bir hata oluştu: " + ex.Message);
        }

    }
}
