using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace PermissionStack.Tests.Integration
{
    public class PermissionsGetIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PermissionsGetIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPermissions_Should_ReturnPermissionsList()
        {
            // Arrange: Crear un permiso primero (para asegurar que hay datos)
            var createPayload = new
            {
                employeeFirstName = "Juliana",
                employeeLastName = "Lozano",
                permissionDate = DateTime.UtcNow,
                permissionTypeId = 1
            };

            var createResponse = await _client.PostAsJsonAsync("/permissions/request", createPayload);
            createResponse.EnsureSuccessStatusCode();

            // Act: consultar todos los permisos
            var getResponse = await _client.GetAsync("/permissions");

            // Assert: verifica que la respuesta sea válida y tenga contenido
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var permissions = await getResponse.Content.ReadFromJsonAsync<List<dynamic>>();
            permissions.Should().NotBeNull();
            permissions.Should().NotBeEmpty();
        }
    }
}

