using Microsoft.AspNetCore.Mvc;
using receiver_and_producer.Dtos;
using receiver_and_producer.Services.GenericDispatcherService;
using System.Net;

namespace receiver_and_producer.Controllers
{
    /// <summary>
    /// Controller responsável por receber mensagens via webhook da J17 e despachá-las para a fila de processamento.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class J17Controller : ControllerBase
    {
        private readonly IGenericDispatcherService _genericDispatcherService;
        public J17Controller(IGenericDispatcherService genericDispatcherService)
        {
            _genericDispatcherService = genericDispatcherService;
        }

        /// <summary>
        /// Recebe uma webhook de atualização de status da operação e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Mensagem processada com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook/Operation")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookOperation([FromBody] object message)
        {
            if (message == null)
            {
                var badRequestResponse = new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo.");
                return BadRequest(badRequestResponse);
            }

            await _genericDispatcherService.DispatchAsync("j17-operation", message);

            var okResponse = new GenericResponseApiDTO((int)HttpStatusCode.OK, "Mensagem de operação recebida com sucesso.");
            return Ok(okResponse);
        }

        /// <summary>
        /// Recebe uma mensagem de retorno da busca de parcelas e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Mensagem processada com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook/Simulation")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookSimulation([FromBody] object message)
        {
            if (message == null)
            {
                var badRequestResponse = new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo.");
                return BadRequest(badRequestResponse);
            }

            await _genericDispatcherService.DispatchAsync("j17-simulation", message);

            var okResponse = new GenericResponseApiDTO((int)HttpStatusCode.OK, "Mensagem de simulação recebida com sucesso.");
            return Ok(okResponse);
        }
    }
}
