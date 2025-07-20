using Microsoft.EntityFrameworkCore;

using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Domain.Entities;
using PermissionStack.Infrastructure.Persistence;

namespace PermissionStack.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly IElasticService _elasticService;
        public PermissionService(AppDbContext context
                               , IElasticService elasticService
                                 )
        {
            _context = context;
            _elasticService = elasticService;
        }

        public async Task<int> RequestPermissionAsync(PermissionRequestDto dto)
        {
            var entity = new Permission
            {
                EmployeeForename = dto.EmployeeFirstName,
                EmployeeSurname = dto.EmployeeLastName,
                PermissionDate = dto.PermissionDate,
                PermissionTypeId = dto.PermissionTypeId
            };

            _context.Permissions.Add(entity);
            await _context.SaveChangesAsync();

            // 🔍 Indexar en Elasticsearch
            var indexDto = new PermissionIndexDto
            {
                Id = entity.Id,
                EmployeeForename = entity.EmployeeForename,
                EmployeeSurname = entity.EmployeeSurname,
                PermissionDate = entity.PermissionDate,
                PermissionTypeId = entity.PermissionTypeId
            };

            await _elasticService.IndexPermissionAsync(indexDto);

            return entity.Id;
        }




        public async Task<bool> ModifyPermissionAsync(int id, PermissionRequestDto dto)
        {
            var permission = await _context.Permissions.FindAsync(id);
            if (permission == null) return false;

            permission.EmployeeForename = dto.EmployeeFirstName;
            permission.EmployeeSurname = dto.EmployeeLastName;
            permission.PermissionDate = dto.PermissionDate;
            permission.PermissionTypeId = dto.PermissionTypeId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PermissionResponseDto>> GetPermissionsAsync()
        {
            var permissions = await _context.Permissions
                .Include(p => p.PermissionType)
                .ToListAsync();

            return permissions.Select(p => new PermissionResponseDto
            {
                Id = p.Id,
                EmployeeFirstName = p.EmployeeForename,
                EmployeeLastName = p.EmployeeSurname,
                PermissionDate = p.PermissionDate,
                PermissionTypeDescription = p.PermissionType.Description
            });
        }

    }
}
