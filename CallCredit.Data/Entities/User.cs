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
    /// User
    /// </summary>
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public bool IsVerified { get; set; }        
        public decimal AccountBalance { get; set; }        
        public virtual List<Beneficiary> Beneficiaries { get; set; }
    }
}
