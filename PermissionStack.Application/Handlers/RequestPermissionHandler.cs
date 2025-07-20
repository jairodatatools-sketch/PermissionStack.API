using MediatR;
using PermissionStack.Application.Commands;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;

public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, int>
{
    private readonly IPermissionService _permissionService;
    private readonly IPermissionEventPublisher _kafkaProducer;
    private readonly IElasticService _elasticService;

    public RequestPermissionHandler(
        IPermissionService permissionService,
        IPermissionEventPublisher kafkaProducer,
        IElasticService elasticService)
    {
        _permissionService = permissionService;
        _kafkaProducer = kafkaProducer;
        _elasticService = elasticService;
    }

    public async Task<int> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        // Persistencia en SQL
        var id = await _permissionService.RequestPermissionAsync(dto);

        // Kafka: evento de operación
        await _kafkaProducer.PublishAsync(new PermissionOperationEventDto
        {
            NameOperation = "request"
        });

        // Elasticsearch: indexación de permiso
        var indexDto = new PermissionIndexDto
        {
            Id = id,
            EmployeeForename = dto.EmployeeFirstName,
            EmployeeSurname = dto.EmployeeLastName,
            PermissionTypeId = dto.PermissionTypeId,
            PermissionDate = DateTime.UtcNow
        };

        await _elasticService.IndexPermissionAsync(indexDto);

        return id;
    }
}




