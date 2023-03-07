namespace AnnuityVerification.Models
{
    public class VerificationModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
        public string PolicyNo { get; set; }
        public bool IsCompliance { get; set; } = false;
        public bool IsFaceMatch { get; set; } = true;
        public string Token { get; set; }
    }
}
