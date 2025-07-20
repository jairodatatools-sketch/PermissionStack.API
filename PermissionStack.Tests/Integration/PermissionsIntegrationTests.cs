using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace PermissionStack.Tests.Integration
{
    public class PermissionsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PermissionsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RequestPermission_Should_ReturnCreated()
        {
            // Arrange
            var payload = new
            {
                employeeFirstName = "Laura",
                employeeLastName = "Gómez",
                permissionDate = "2025-07-21T10:00:00Z",
                permissionTypeId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/permissions/request", payload);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }
    }
}

