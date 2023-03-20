namespace AnnuityVerification.Models.Response
{




    public class VerificationResponse
    {
        public Results result { get; set; }
        public object targetUrl { get; set; }
        public bool success { get; set; }
        public object error { get; set; }
        public bool unAuthorizedRequest { get; set; }
        public bool __abp { get; set; }
    }

    public class Results
    {
        public bool data { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string photoUrl { get; set; }
        public string photoHash { get; set; }
        public object photo { get; set; }
        public Tables table { get; set; }
    }

    public class Tables
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string accountNumber { get; set; }
        public string status { get; set; }
        public string gender { get; set; }
        public string phoneNumber { get; set; }
        public string bvn { get; set; }
        public string dateOfBirth { get; set; }
        public string photo { get; set; }
        public string photoURL { get; set; }
        public string photoHash { get; set; }
        public bool isFaceVerification { get; set; }
        public bool isComplianceVerification { get; set; }
        public DateTime dateVerified { get; set; }
        public string bankCode { get; set; }
        public object policyNo { get; set; }
        public bool isFaceVerified { get; set; }
        public string metaMapPicture { get; set; }
    }




}
