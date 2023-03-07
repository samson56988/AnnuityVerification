namespace AnnuityVerification.Models.Response
{
    public class BankResponse
    {
        public BankDetails result { get; set; }
    }    
       

     public class BankDetails
     {
       public bool data { get; set; }
       public bool success { get; set; }
       public string message { get; set; }
       public List<Table> table { get; set; }
      }

      public class Table
      {
         int id { get; set; }
         public string code { get; set; }
         public string bankName { get; set; }
        }



    }



