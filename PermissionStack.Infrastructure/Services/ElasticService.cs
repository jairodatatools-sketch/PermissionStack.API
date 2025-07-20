using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Microsoft.Extensions.Logging;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;

namespace PermissionStack.Infrastructure.Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<ElasticService> _logger;

        public ElasticService(ElasticsearchClient client, ILogger<ElasticService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task IndexPermissionAsync(PermissionIndexDto permission)
        {
            var response = await _client.IndexAsync(permission, idx => idx.Index("permissions"));

            if (!response.IsValidResponse)
            {
                _logger.LogError("Error al indexar el permiso con ID {Id} en Elasticsearch.", permission.Id);
            }
            else
            {
                _logger.LogInformation("Permiso con ID {Id} indexado correctamente en Elasticsearch.", permission.Id);
            }
        }

        // Métodos extra que podemos implementar después
        // public Task DeletePermissionAsync(int permissionId) { ... }
        public async Task<PermissionIndexDto?> GetPermissionByIdAsync(int permissionId)
        {
            var response = await _client.GetAsync<PermissionIndexDto>(permissionId.ToString(), idx => idx.Index("permissions"));

            if (!response.Found)
            {
                _logger.LogWarning("No se encontró el permiso con ID {Id} en Elasticsearch.", permissionId);
                return null;
            }

            _logger.LogInformation("Se recuperó el permiso con ID {Id} desde Elasticsearch.", permissionId);
            return response.Source;
        }

        public async Task<string> GetElasticsearchVersionAsync()
        {
            var info = await _client.InfoAsync(); // _client es ElasticsearchClient
            return info.Version.Number;
        }


        // public Task<IEnumerable<PermissionIndexDto>> SearchByEmployeeNameAsync(string name) { ... }
    }
}


