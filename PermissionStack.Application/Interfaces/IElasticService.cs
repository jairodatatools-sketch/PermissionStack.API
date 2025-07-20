using PermissionStack.Application.DTOs;

namespace PermissionStack.Application.Interfaces
{
    public interface IElasticService
    {
        // Indexa un permiso (ya lo tienes)
        Task IndexPermissionAsync(PermissionIndexDto permission);

        //// Elimina un documento por ID
        //Task DeletePermissionAsync(int permissionId);

        // Busca uno por ID (útil si quieres confirmar que se indexó)
        Task<PermissionIndexDto?> GetPermissionByIdAsync(int permissionId);

        //// Opcional: búsqueda por nombre (para futuros filtros o reportes)
        //Task<IEnumerable<PermissionIndexDto>> SearchByEmployeeNameAsync(string name);

        Task<string> GetElasticsearchVersionAsync();
    }
}

