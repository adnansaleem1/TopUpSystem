using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CallCredit.Data.Entities
{
    /// <summary>
    /// Contact
    /// </summary>
    public class Beneficiary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeneficiaryId { get; set; }
        [StringLength(20, ErrorMessage = "Nickname cannot exceed 20 characters.")]
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }      
        public virtual User User { get; set; }
    }
}
