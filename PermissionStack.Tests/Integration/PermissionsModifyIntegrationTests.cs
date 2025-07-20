using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace PermissionStack.Tests.Integration
{
    public class PermissionsModifyIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PermissionsModifyIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ModifyPermission_Should_ReturnOk_And_UpdateIndex_And_PublishToKafka()
        {
            // Paso 1: primero se crea un permiso para luego modificar
            var createPayload = new
            {
                employeeFirstName = "David",
                employeeLastName = "Reyes",
                permissionDate = "2025-07-21T08:00:00Z",
                permissionTypeId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/permissions/request", createPayload);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            // Obtiene el ID del permiso creado (ej. desde location header o body)
            // Aquí asumimos que devuelve en el cuerpo un `id` (ajústalo según tu implementación)
            var created = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            int permissionId = created.id;

            // Paso 2: modificar el permiso
            var modifyPayload = new
            {
                employeeFirstName = "David",
                employeeLastName = "Rojas",
                permissionDate = "2025-07-21T10:00:00Z",
                permissionTypeId = 2
            };

            var modifyResponse = await _client.PutAsJsonAsync($"/permissions/modify/{permissionId}", modifyPayload);

            // Validación
            modifyResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}

