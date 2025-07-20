using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Get;
using Elastic.Transport;
using Microsoft.Extensions.Logging;
using Moq;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Infrastructure.Services;
using Xunit;

namespace PermissionStack.Tests.Services
{
    public class ElasticServiceTests
    {
        [Fact]
        public async Task GetPermissionByIdAsync_Should_ReturnIndexedPermission()
        {
            // Arrange
            var permissionId = 101;
            var expectedPermission = new PermissionIndexDto
            {
                Id = permissionId,
                EmployeeForename = "Sofía",
                EmployeeSurname = "Gutiérrez",
                PermissionTypeId = 2,
                PermissionDate = DateTime.UtcNow
            };

            var mockResponse = new Mock<GetResponse<PermissionIndexDto>>();
            mockResponse.Setup(r => r.Found).Returns(true);
            mockResponse.Setup(r => r.Source).Returns(expectedPermission);

            var mockClient = new Mock<ElasticsearchClient>();
            mockClient
                .Setup(c => c.GetAsync<PermissionIndexDto>(
                    permissionId.ToString(),
                    It.IsAny<Func<GetRequestDescriptor<PermissionIndexDto>, GetRequestDescriptor<PermissionIndexDto>>>(),
                    default))
                .ReturnsAsync(mockResponse.Object);

            var mockLogger = new Mock<ILogger<ElasticService>>();

            var service = new ElasticService(mockClient.Object, mockLogger.Object);

            // Act
            var result = await service.GetPermissionByIdAsync(permissionId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPermission.Id, result.Id);
            Assert.Equal(expectedPermission.EmployeeForename, result.EmployeeForename);
            Assert.Equal(expectedPermission.EmployeeSurname, result.EmployeeSurname);
        }
    }
}
