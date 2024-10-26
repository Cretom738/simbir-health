using System.ComponentModel.DataAnnotations;

namespace Application.Configurations
{
    public class SessionsConfiguration
    {
        [Range(0, int.MaxValue)]
        public required int UserSessionsLimit { get; set; }
    }
}
