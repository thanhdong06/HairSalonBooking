﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.Entities
{
    [Table("Appointment")]
    public class AppointmentEntity : BaseEntity
    {
        public string Note { get; set; }
        public DateTime Date{ get; set; }
        public DateTime ReturnDate{ get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public int SalonId { get; set; }
        public UserEntity User { get; set; }
        public SalonEntity Salon { get; set;}
    }
}