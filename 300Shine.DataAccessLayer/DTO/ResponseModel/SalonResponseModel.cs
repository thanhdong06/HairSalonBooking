﻿using _300Shine.DataAccessLayer.DTO.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.DTO.ResponseModel
{
    public class SalonResponseModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string District { get; set; }
        public List<ServiceRequestModel> Services { get; set; }
        public List<StylistRequestModel> Stylists { get; set; }
    }
}
