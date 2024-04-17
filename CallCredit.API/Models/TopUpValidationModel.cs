using CallCredit.Data.Entities;

namespace CallCredit.API.Models
{
    public class TopUpValidationModel : TopUpRequestModel
    {
        
        public TopUpOption? TopUpOption { get; set; }
        public TopUpValidationModel(TopUpRequestModel topUpRequest, TopUpOption? topUpOption):base(topUpRequest)
        {
        
            TopUpOption = topUpOption;
        }
    }
}
