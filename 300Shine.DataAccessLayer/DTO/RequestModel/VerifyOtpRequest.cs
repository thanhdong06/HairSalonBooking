﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.DataAccessLayer.DTO.RequestModel
{
    public class VerifyOtpRequest
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
    }
}
