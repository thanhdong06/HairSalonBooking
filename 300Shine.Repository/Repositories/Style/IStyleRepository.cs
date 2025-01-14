﻿using _300Shine.DataAccessLayer.DTO.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Repository.Repositories.Style
{
    public interface IStyleRepository
    {
        Task<List<StyleResponseDTO>> GetAllStyles(string? search, int pageIndex, int pageSize);    
    }
}
