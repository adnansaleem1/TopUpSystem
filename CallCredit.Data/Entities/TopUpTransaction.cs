using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCredit.Data.Entities
{
    /// <summary>
    /// Transaction
    /// </summary>
    public class TopUpTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int BeneficiaryId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Charge { get; set; } = 1;
        public Beneficiary Beneficiary { get; set; }
        public virtual User User { get; set; }
    }
}
