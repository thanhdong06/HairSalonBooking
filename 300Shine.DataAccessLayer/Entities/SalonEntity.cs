﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.Entities
{
    [Table("Salon")]
    public class SalonEntity : BaseEntity
    {
        public string Address { get; set; }
        public int Phone { get; set; }
        public string District { get; set; }
    }
}