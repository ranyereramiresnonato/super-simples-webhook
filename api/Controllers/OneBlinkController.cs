using Microsoft.AspNetCore.Mvc;
using api.Dtos;
using api.Services.GenericDispatcherService;
using System.Net;
using api.Consts;

namespace api.Controllers
{
    /// <summary>
    /// Controller responsável por receber mensagens via webhook do banco OneBlink e despachá-las para a fila de processamento.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OneBlinkController : ControllerBase
    {
        private readonly IGenericDispatcherService _genericDispatcherService;
        public OneBlinkController(IGenericDispatcherService IGenericDispatcherService)
        {
            _genericDispatcherService = IGenericDispatcherService;
        }

        /// <summary>
        /// Recebe uma mensagem via webhook e despacha para a fila de processamento.
        /// </summary>
        /// <param name="message">Objeto recebido no corpo da requisição.</param>
        /// <returns>Retorna um status HTTP indicando o sucesso ou falha do processamento.</returns>
        /// <response code="200">Webhook recebido com sucesso.</response>
        /// <response code="400">Requisição inválida.</response>
        /// <response code="500">Erro interno no servidor.</response>
        [HttpPost("Webhook")]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 200)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 400)]
        [ProducesResponseType(typeof(GenericResponseApiDTO), 500)]
        public async Task<IActionResult> Webhook([FromBody] object message)
        {
            if (message == null)
            {
                var badRequestResponse = new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo.");
                return BadRequest(badRequestResponse);
            }

            await _genericDispatcherService.DispatchAsync(ProviderIdentifiers.OneBlink, message);

            var okResponse = new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook recebido com sucesso");
            return Ok(okResponse);
        }
    }
}
