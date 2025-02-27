﻿using _300Shine.DataAccessLayer.DTO.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Service.Styles
{
    public interface IStyleService
    {
        Task<List<StyleResponseDTO>> GetAllStyles(string? search, int pageIndex, int pageSize);
    }
}
