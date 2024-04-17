using CallCredit.Data.Entities;

namespace CallCredit.API.Models
{
    public class TopUpRequestModel : RequestBase
    {
        public int BeneficiaryId { get; set; }
        public int TopUpOptionId { get; set; }

        public TopUpRequestModel() { }
        public TopUpRequestModel(TopUpRequestModel request)
        {
            BeneficiaryId = request.BeneficiaryId;
            TopUpOptionId = request.TopUpOptionId;
            UserId = request.UserId;
        }
    }
}
