using CallCredit.API.Models;
using CallCredit.Data.Entities;

namespace CallCredit.API.Interfaces
{
    public interface ITopUpService
    {
        Task<bool> PerformTopUp(TopUpRequestModel request);
        Task<List<TopUpOption>> GetTopUpOptions();

    }
}
