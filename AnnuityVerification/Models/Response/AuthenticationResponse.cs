using Newtonsoft.Json;

namespace AnnuityVerification.Models.Response
{
    public class AuthenticationResponse
    {
        public Result result { get; set; }
    }
    public class Result
    {
        public bool data { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public bool table { get; set; }
    }
}
