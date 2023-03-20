namespace AnnuityVerification.Models
{
    

        public class PolicyVerificationResponse
        {
            public Result result { get; set; }
            public object targetUrl { get; set; }
            public bool success { get; set; }
            public object error { get; set; }
            public bool unAuthorizedRequest { get; set; }
            public bool __abp { get; set; }
        }

        public class Result
        {
            public bool isSuccessful { get; set; }
            public object productName { get; set; }
            public object policyStatus { get; set; }
            public string policyNumber { get; set; }
            public object policyCode { get; set; }
            public object dateOfBirth { get; set; }
            public object agentName { get; set; }
            public object agentShortDescription { get; set; }
            public object clientName { get; set; }
            public object clientShortDescription { get; set; }
            public object email { get; set; }
            public object phone { get; set; }
            public object effectiveDate { get; set; }
            public object paidToDate { get; set; }
            public object maturityDate { get; set; }
            public object totalPremium { get; set; }
            public object totalSavings { get; set; }
            public object paidPremium { get; set; }
            public object paymentFrequency { get; set; }
            public object nextInstallmentPremium { get; set; }
            public object hash { get; set; }
            public object loanId { get; set; }
            public object outStandingPremiumBalance { get; set; }
            public object amountToPay { get; set; }
        }

    }

