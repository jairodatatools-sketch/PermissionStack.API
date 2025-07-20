using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using PermissionStack.Application.Commands;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Application.Handlers;

namespace PermissionStack.Tests.Handlers
{
    public class RequestPermissionHandlerTests
    {
        [Fact]
        public async Task Handle_RequestPermission_ReturnsId_AndPublishesEvent()
        {
            // Arrange
            var dto = new PermissionRequestDto(); // puedes añadir datos simulados si lo deseas

            var permissionServiceMock = new Mock<IPermissionService>();
            var eventPublisherMock = new Mock<IPermissionEventPublisher>();

            permissionServiceMock
                .Setup(s => s.RequestPermissionAsync(It.IsAny<PermissionRequestDto>()))
                .ReturnsAsync(42); // valor simulado para el ID

            eventPublisherMock
                .Setup(e => e.PublishAsync(It.IsAny<PermissionOperationEventDto>()))
                .Returns(Task.CompletedTask);

            var handler = new RequestPermissionHandler(permissionServiceMock.Object, eventPublisherMock.Object);
            var command = new RequestPermissionCommand(dto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(42, result);
            permissionServiceMock.Verify(s => s.RequestPermissionAsync(dto), Times.Once);
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<PermissionOperationEventDto>(
                m => m.NameOperation == "request")), Times.Once);
        }
    }
}

