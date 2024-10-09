﻿using _300Shine.DataAccessLayer.DBContext;
using _300Shine.DataAccessLayer.DTO.RequestModel;
using _300Shine.DataAccessLayer.DTO.ResponseModel;
using _300Shine.DataAccessLayer.Entities;
using AutoMapper;
using DataAccessLayer.ServiceForCRUD.Paging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _300Shine.Repository.Repositories.Service
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context = new();
       
        private readonly IMapper _mapper;

        public ServiceRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> CreateService(CreateServiceRequestModel s)
        {
            var serviceStyleDTOList = s.ServiceStyles;

            var newService = new ServiceEntity()
            {
                Name = s.Name,
                Price = s.Price,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                ServiceStyles=new List<ServiceStyleEntity>()
            };
            foreach (var serviceStyles in serviceStyleDTOList)
            {
                var checkStyle = await _context.Styles
                .SingleOrDefaultAsync(x => x.Id == serviceStyles.StyleId);
                if (checkStyle == null)
                {
                    throw new InvalidDataException("Style is not found");

                }


                var serviceStyle = new ServiceStyleEntity()
                {
                    Style = checkStyle,
                    Service = newService,

                };
                newService.ServiceStyles.Add(serviceStyle);

            }
            _context.Services.Add(newService);
            try
            {
                await _context.SaveChangesAsync();
                return "Create Service Successfully";
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("Error saving changes", ex);
            }
        }

        public async Task<string> UpdateService(UpdateServiceRequestModel s)
        {
            var serviceStyleList = s.ServiceStyles;
            var serviceEntity = await _context.Services.Include(s=>s.ServiceStyles).SingleOrDefaultAsync(x => x.Id == s.Id && x.IsDeleted == false);
            if (serviceEntity == null)
                throw new InvalidDataException("Service is not found");
            serviceEntity.Name = s.Name;
            serviceEntity.Description = s.Description;
            serviceEntity.Price = s.Price;
            serviceEntity.ImageUrl = s.ImageUrl;
           
            var existedServiceStyleIdInList = serviceEntity.ServiceStyles.Select(x => x.StyleId).ToList();
            var newServiceStyleIdInList = serviceStyleList.Select(x => x.StyleId).ToList();
            var removeServiceStyleIdInList = existedServiceStyleIdInList.Where(existedServiceStyleId => !newServiceStyleIdInList.Contains(existedServiceStyleId)).ToList();

            foreach (var serviceStyles in serviceStyleList)
            {
                int existedStyleIdPosition = existedServiceStyleIdInList.IndexOf(serviceStyles.StyleId);
                if (existedStyleIdPosition == -1)
                {
                    var checkStyle = await _context.Styles.SingleOrDefaultAsync(x => x.Id == serviceStyles.StyleId);
                    if (checkStyle == null)
                        throw new InvalidDataException("Style is not found");
                    var serviceStyle = new ServiceStyleEntity()
                    {
                        Style = checkStyle,
                        Service = serviceEntity

                    };
                    serviceEntity.ServiceStyles.Add(serviceStyle);
                }
                else
                {
                    var existedStyleId = existedServiceStyleIdInList[existedStyleIdPosition];
                    var checkServiceStyle = _context.ServiceStyles.FirstOrDefault(i => i.Style.Id == existedStyleId);
                    serviceEntity.ServiceStyles.Add(checkServiceStyle);
                }
                foreach (var removeServiceStyleId in removeServiceStyleIdInList)
                {
                    var serviceStyle = _context.ServiceStyles.FirstOrDefault(i => i.Service.Id == serviceEntity.Id && i.Style.Id == removeServiceStyleId);

                    serviceEntity.ServiceStyles.Remove(serviceStyle);
                    
                }
            }
            _context.Services.Update(serviceEntity);
           
            if (await _context.SaveChangesAsync() > 0)
                return "Update Service Successfully";
            else
                return "Update Service Failed";
        }

        public async Task<string> DeleteService(int id)
        {
            var service = await _context.Services.SingleOrDefaultAsync(x => x.Id == id);
            if (service == null)
                throw new InvalidDataException("Service is not found");

            service.IsDeleted = true;
            
            _context.Services.Update(service);
            if (await _context.SaveChangesAsync() > 0)
                return "Delete Service Successfully";
            else
                return "Delete Service Failed";
        }

        public async Task<List<ServiceResponseModel>> GetServices(string? search, string? sortBy,
            decimal? fromPrice, decimal? toPrice,
            string? size,
            int pageIndex, int pageSize)
        {
            IQueryable<ServiceEntity> services = _context.Services.Include(ss=>ss.ServiceStyles).Where(x => x.Id != null && x.IsDeleted == false);

            //TÌM THEO TÊN
            if (!string.IsNullOrEmpty(search))
            {
                services = services.Where(x => x.Name.Contains(search));
            }


            //FILTER THEO GIÁ
            if (fromPrice.HasValue)
            {
                services = services.Where(x => x.Price >= fromPrice.Value);
            }

            if (toPrice.HasValue)
            {
                services = services.Where(x => x.Price <= toPrice.Value);
            }

            //SORT THEO TÊN
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("ascName"))
                {
                    services = services.OrderBy(x => x.Name);
                }
                else if (sortBy.Equals("descName"))
                {
                    services = services.OrderByDescending(x => x.Name);
                }
            }

            //SORT THEO GIÁ
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("ascPrice"))
                {
                    services = services.OrderBy(x => x.Price);
                }
                else if (sortBy.Equals("descPrice"))
                {
                    services = services.OrderByDescending(x => x.Price);
                }
            }

            var paginatedUsers = PaginatedList<ServiceEntity>.Create(services, pageIndex, pageSize);

            return _mapper.Map<List<ServiceResponseModel>>(paginatedUsers);
        }

        public async Task<ServiceResponseModel> GetServiceByID(int id)
        {
            var service = await _context.Services.Include(ss=>ss.ServiceStyles).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (service == null)
                throw new Exception("Service is not found");

            return _mapper.Map<ServiceResponseModel>(service);
        }

    }
}