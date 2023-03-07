namespace AnnuityVerification.Models.Response
{
    

    public class VerificationResponse
    {
        public VerificationResult result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class VerificationResult
    {
        public bool data { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string photoUrl { get; set; }
        public string photoHash { get; set; }
        public string Photo { get; set; }
        public object table { get; set; }
    }

    public class Applicant
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string accountNumber { get; set; }
        public string bankCode { get; set; }
    }

    public class BvnNuban
    {
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string birthdate { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string bvn { get; set; }
        public string photo { get; set; }
    }

    public class BvnNubanCheck
    {
        public string status { get; set; }
        public FieldMatches fieldMatches { get; set; }
    }

    public class FieldMatches
    {
        public bool firstname { get; set; }
        public bool lastname { get; set; }
    }

    public class Message
    {
        public int id { get; set; }
        public Applicant applicant { get; set; }
        public Summary summary { get; set; }
        public Status status { get; set; }
        public BvnNuban bvn_nuban { get; set; }
    }

    public class Status
    {
        public string state { get; set; }
        public string status { get; set; }
    }

    public class Summary
    {
        public BvnNubanCheck bvn_nuban_check { get; set; }
    }



}
