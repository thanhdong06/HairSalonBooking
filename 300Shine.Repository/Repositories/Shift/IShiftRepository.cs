﻿using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.DTO.ResponseModel;
using _300Shine.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Repository.Repositories.Shift
{
    public interface IShiftRepository
    {
        Task<string> CreateShift(ShiftCreateDTO shift);
        Task AutoCreateShiftForWholeWeek();
        Task<string> UpdateShift(ShiftUpdateDTO shift);
        Task<string> DeleteShift(int id);
        Task<ShiftResponseDTO> GetShiftById(int id);
        Task<List<ShiftResponseDTO>> GetShifts(string? search, DateTime? date, string? status, int pageIndex, int pageSize);
        Task<List<ShiftForChoosingDTO>> GetShiftsBySalonAndStylistId(int userId);
        Task<string> ShiftsForStylist(int userId, ShiftCreateForStylistDTO request);
    }

}
