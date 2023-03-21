namespace AnnuityVerification.Models
{



    public class PolicyUpdateResponse
    {
        public bool data { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string photoUrl { get; set; }
        public string photoHash { get; set; }
        public string photo { get; set; }
        public Table table { get; set; }
    }

    public class Table
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
        public string policyNo { get; set; }
        public bool isFaceVerified { get; set; }
        public string metaMapPicture { get; set; }
    }


}
