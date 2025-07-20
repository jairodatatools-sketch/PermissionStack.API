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
    public class ModifyPermissionHandlerTests
    {
        [Fact]
        public async Task Handle_ModificationSuccess_PublishesEvent()
        {
            // Arrange
            var dto = new PermissionRequestDto();
            var permissionServiceMock = new Mock<IPermissionService>();
            var eventPublisherMock = new Mock<IPermissionEventPublisher>();

            permissionServiceMock
                .Setup(s => s.ModifyPermissionAsync(It.IsAny<int>(), It.IsAny<PermissionRequestDto>()))
                .ReturnsAsync(true);

            eventPublisherMock
                .Setup(e => e.PublishAsync(It.IsAny<PermissionOperationEventDto>()))
                .Returns(Task.CompletedTask);

            var handler = new ModifyPermissionHandler(permissionServiceMock.Object, eventPublisherMock.Object);
            var command = new ModifyPermissionCommand(1, dto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            permissionServiceMock.Verify(s => s.ModifyPermissionAsync(1, dto), Times.Once);
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<PermissionOperationEventDto>(
                m => m.NameOperation == "modify")), Times.Once);
        }
    }
}

