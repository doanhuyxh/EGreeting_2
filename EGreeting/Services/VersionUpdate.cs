namespace EGreeting.Services
{
    public class VersionUpdate
    {
        private readonly IConfiguration _configuration;

        public VersionUpdate(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Version()
        {
            return _configuration.GetSection("VersionUpdate").Value;
        }
    }
}
