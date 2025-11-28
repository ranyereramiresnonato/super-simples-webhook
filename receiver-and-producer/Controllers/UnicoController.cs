using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using receiver_and_producer.Dtos;
using receiver_and_producer.Services.GenericDispatcherService;
using System.Net;

namespace receiver_and_producer.Controllers
{
    /// <summary>
    /// Controller responsável por receber webhooks do banco Unico e despachar para filas de processamento específicas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UnicoController : ControllerBase
    {
        private readonly IGenericDispatcherService _genericDispatcherService;
        private readonly string _validApiKey;

        public UnicoController(IGenericDispatcherService genericDispatcherService, IConfiguration configuration)
        {
            _genericDispatcherService = genericDispatcherService;
            _validApiKey = configuration["Logging:UnicoCheck:ApiKey"]
            ?? throw new Exception("A chave de API para UnicoCheck não está configurada em appsettings.json.");
        }

        /// <summary>
        /// Recebe uma mensagem via webhook do produto Check e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        [HttpPost("Webhook/Check")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookCheck([FromBody] object message)
        {
            if (message == null)
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));

            await _genericDispatcherService.DispatchAsync("unico-check", message);
            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook Check recebido com sucesso"));
        }

        /// <summary>
        /// Recebe uma mensagem via webhook do produto Id e despacha para a fila de processamento. Valida a x-api-key.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        [HttpPost("Webhook/Id")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 401)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookId([FromBody] object message, [FromHeader(Name = "x-api-key")] string apiKey)
        {
            if (message == null)
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));

            if (string.IsNullOrEmpty(apiKey) || apiKey != _validApiKey)
                return Unauthorized(new GenericResponseApiDTO((int)HttpStatusCode.Unauthorized, "Chave de API inválida."));

            await _genericDispatcherService.DispatchAsync("unico-id", message);
            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook Id recebido com sucesso"));
        }
    }
}
