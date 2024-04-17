namespace CallCredit.API.Models
{
    public class BeneficiaryRequest:RequestBase
    {
        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
