namespace OdeToFood.Services
{
    using Interfaces;
    using Microsoft.Extensions.Configuration;

    public class Greeter : IGreeter
    {
        private string _greeting;

        public Greeter(IConfiguration configuration)
        {
            _greeting = configuration["Greeting"];
        }
        
        public string GetGreeting()
        {
            return _greeting;
        }
    }
}
