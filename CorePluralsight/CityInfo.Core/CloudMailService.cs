namespace CityInfo.Core
{
    using System.Diagnostics;

    public class CloudMailService : IMailService
    {
        private string mailTo = Startup.Configuration["mailSettings:mailToAddress"];
        private string mailFrom = Startup.Configuration["mailSettings:mailFromAddress"];

        public void Send(string subject, string message)
        {
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo} with CloudMailService.");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"Message: {message}");
        }
    }
}
