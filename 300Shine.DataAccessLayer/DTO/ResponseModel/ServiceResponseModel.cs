﻿using _300Shine.DataAccessLayer.DTO.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.DTO.ResponseModel
{
    public class ServiceResponseModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SalonId { get; set; }
        public bool IsDeleted { get; set; }
        public List<ServiceStyleRequestModel> ServiceStyles { get; set; }
    }

    public class ServiceResponseForChooseStylistFirst
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SalonId { get; set; }
        public int Duration { get; set; }
        public bool IsDeleted { get; set; }
        public List<StyleResponseDTO> ServiceStyles { get; set; }
    }
}
