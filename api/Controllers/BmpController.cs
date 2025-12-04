using Microsoft.AspNetCore.Mvc;
using api.Dtos;
using api.Services.GenericDispatcherService;
using System.Net;
using api.Consts;

namespace api.Controllers
{
    /// <summary>
    /// Controller responsável por receber webhooks do BMP e despachar para a fila de processamento.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BmpController : ControllerBase
    {
        private readonly IGenericDispatcherService _genericDispatcherService;

        public BmpController(IGenericDispatcherService genericDispatcherService)
        {
            _genericDispatcherService = genericDispatcherService;
        }

        /// <summary>
        /// Recebe uma mensagem via webhook do BMP e despacha para a fila de processamento.
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
        public async Task<IActionResult> Webhook([FromQuery] string proposta, [FromQuery] string situacao, [FromQuery] string identificador)
        {
            if (proposta == null || situacao == null || identificador == null)
            {
                return BadRequest(new GenericResponseApiDTO((int)HttpStatusCode.BadRequest, "O corpo da requisição não pode ser nulo."));
            }

            var message = new
            {
                Proposta = proposta,
                Situacao = situacao,
                Identificador = identificador
            };

            await _genericDispatcherService.DispatchAsync(ProviderIdentifiers.Bmp, message);

            return Ok(new GenericResponseApiDTO((int)HttpStatusCode.OK, "Webhook recebido com sucesso"));
        }
    }
}
