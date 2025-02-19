﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.DTO.ResponseModel
{
    public class ShiftResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MinStaff { get; set; }
        public int MaxStaff { get; set; }
        public string Status { get; set; }
        public int SalonId { get; set; }
    }

    public class ShiftForChoosingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int MinStaff { get; set; }
        public int MaxStaff { get; set; }
        public string Status { get; set; }
        public int SalonId { get; set; }
        public bool isChosen { get; set; }
    }
}
