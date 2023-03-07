namespace MetaWebHook
{
        public class VerificationPayLoad
        {
         public string? resource { get; set; }
         public string? eventName { get; set; }
         public string? flowId { get; set; }
         public DateTime timestamp {get; set;}

        }
}
