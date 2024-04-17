namespace CallCredit.API.Services
{
    public class RulesServiceBase
    {       
        public const int ChargeAmount = 1;  // Flat fee for transactions
        protected const decimal MaxAmountMonth = 3000;  // Maximum amount a user can top-up in a month
        protected const decimal MaxBeneficiary = 5;  // Maximum number of active beneficiaries a user can have
        protected const decimal MaxNickNameLength = 20;  // Maximum length for beneficiary nicknames
        protected const decimal NotVerifiedUserMaxAmount = 1000;  // Max top-up per month for unverified users per beneficiary
        protected const decimal VerifiedUserMaxAmount = 500;  // Max top-up per month for verified users per beneficiary
    }
}