using Microsoft.AspNetCore.Mvc;
using receiver_and_producer.Dtos;
using receiver_and_producer.Services.GenericDispatcherService;
using System.Net;

namespace receiver_and_producer.Controllers
{
    /// <summary>
    /// Controller responsável por receber webhooks do banco Qi e despachar para filas de processamento específicas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QiController : ControllerBase
    {
        private readonly IGenericDispatcherService _genericDispatcherService;

        public QiController(IGenericDispatcherService genericDispatcherService)
        {
            _genericDispatcherService = genericDispatcherService;
        }

        /// <summary>
        /// Recebe uma mensagem via webhook do FGTS e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Webhook FGTS recebido com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook/Fgts")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookFgts([FromBody] object message)
        {
            if (message == null)
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));

            await _genericDispatcherService.DispatchAsync("qi-fgts", message);
            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook FGTS recebido com sucesso"));
        }

        /// <summary>
        /// Recebe uma mensagem via webhook do INSS e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Webhook INSS recebido com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook/Inss")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookInss([FromBody] object message)
        {
            if (message == null)
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));

            await _genericDispatcherService.DispatchAsync("qi-inss", message);
            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook INSS recebido com sucesso"));
        }

        /// <summary>
        /// Recebe uma mensagem via webhook de Installments e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Webhook Installments recebido com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook/Installments")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> WebhookInstallments([FromBody] object message)
        {
            if (message == null)
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));

            await _genericDispatcherService.DispatchAsync("qi-installments", message);
            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook Installments recebido com sucesso"));
        }
    }
}
