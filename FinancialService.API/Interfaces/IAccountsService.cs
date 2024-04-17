using FinancialService.API;


namespace CallCredit.API.Interfaces
{
    public interface IAccountsService
    {
        Task<bool> Debit(TransactionRequest request);
        Task<bool> Credit(TransactionRequest request);
        Task<decimal> GetBalance(int userId);
    }
}
