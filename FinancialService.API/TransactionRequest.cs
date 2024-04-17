namespace FinancialService.API
{
    public class TransactionRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public bool IsCredit { get; set; }  
        public string TransactionType { get; set; }
      
    }
}
