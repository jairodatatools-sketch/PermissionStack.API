using MediatR;
using Microsoft.AspNetCore.Mvc;
using PermissionStack.Application.Commands;
using PermissionStack.Application.DTOs;
using PermissionStack.Application.Interfaces;
using PermissionStack.Application.Queries;

namespace PermissionStack.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly KafkaProducerService _kafkaProducer;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IMediator mediator, KafkaProducerService kafkaProducer, ILogger<PermissionsController> logger)
        {
            _mediator = mediator;
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            _logger.LogInformation("Operation: get");

            var result = await _mediator.Send(new GetPermissionsQuery());

            await _kafkaProducer.PublishAsync(new PermissionOperationEventDto
            {
                NameOperation = "get"
            });

            return Ok(result);
        }

        [HttpPost("request")]
        public async Task<IActionResult> RequestPermission([FromBody] PermissionRequestDto dto)
        {
            _logger.LogInformation("Operation: request");

            try
            {
                // Validación rápida del modelo
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Modelo inválido recibido.");
                    return BadRequest(ModelState);
                }

                var command = new RequestPermissionCommand(dto);
                var id = await _mediator.Send(command);

                if (id == Guid.Empty)
                {
                    _logger.LogWarning("No se pudo generar el permiso.");
                    return StatusCode(500, "Error interno: ID nulo.");
                }

                await _kafkaProducer.PublishAsync(new PermissionOperationEventDto
                {
                    NameOperation = "request"
                });

                _logger.LogInformation($"Permiso generado correctamente con ID: {id}");

                return Ok(new { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al procesar la solicitud de permiso.");
                return StatusCode(500, "Se produjo un error inesperado.");
            }
        }



        [HttpPut("modify/{id}")]
        public async Task<IActionResult> ModifyPermission(int id, [FromBody] PermissionRequestDto dto)
        {
            _logger.LogInformation("Operation: modify");

            var result = await _mediator.Send(new ModifyPermissionCommand(id, dto));
            if (!result) return NotFound();

            await _kafkaProducer.PublishAsync(new PermissionOperationEventDto
            {
                NameOperation = "modify"
            });

            return NoContent();
        }

        [HttpGet("test-elastic")]
        public async Task<IActionResult> TestElastic([FromServices] IElasticService elasticService)
        {
            var version = await elasticService.GetElasticsearchVersionAsync();
            return Ok($"Elasticsearch versión conectada: {version}");
        }

    }
}

