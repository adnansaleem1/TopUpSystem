﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCredit.Data.Entities
{
    public class TopUpOption
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TopUpOptionId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
