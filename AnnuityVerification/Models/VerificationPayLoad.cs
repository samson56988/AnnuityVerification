using System.ComponentModel.DataAnnotations;

namespace AnnuityVerification
{
    public class VerificationPayLoad
    {
        [Key]
        public string resource { get; set; }
        public string eventName { get; set; }
        public string PolicyNo { get; set; }
        public string MetaPicture { get; set; }
        public DateTime timestamp { get; set; }

    }


    public class VerificationDetails
    {
        public Metadata? metadata { get; set; }
        public string? resource { get; set; }
        public Devicefingerprint? deviceFingerprint { get; set; }
        public string? identityStatus { get; set; }
        public string? matiDashboardUrl { get; set; }
        public string? status { get; set; }
        public string? eventName { get; set; }
        public string? flowId { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Metadata
    {
        public string? userPhotoLink { get; set; }
        public string? userPhotoLinkHash { get; set; }
    }

    public class Devicefingerprint
    {
        public string? ua { get; set; }
        public Browser? browser { get; set; }
        public Engine? engine { get; set; }
        public Os? os { get; set; }
        public Cpu? cpu { get; set; }
        public App? app { get; set; }
        public string? ip { get; set; }
        public bool vpnDetectionEnabled { get; set; }
    }

    public class Browser
    {
        public string? name { get; set; }
        public string? version { get; set; }
        public string? major { get; set; }
    }

    public class Engine
    {
        public string? name { get; set; }
        public string? version { get; set; }
    }

    public class Os
    {
        public string? name { get; set; }
        public string? version { get; set; }
    }

    public class Cpu
    {
        public string? architecture { get; set; }
    }

    public class App
    {
        public string? platform { get; set; }
        public string? version { get; set; }
    }

}
