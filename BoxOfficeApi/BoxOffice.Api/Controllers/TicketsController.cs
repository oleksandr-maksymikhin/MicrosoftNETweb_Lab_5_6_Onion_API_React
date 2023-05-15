using AutoMapper;
using BoxOffice.Model.Ticket;
using BoxOffice.Orchestrators.Ticket.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxOffice.Api.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    [Route("api/v1/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TicketsController> _logger;
        private ITicketOrchestrator _ticketOrchestrator;

        public TicketsController(
            IMapper mapper,
            ILogger<TicketsController> logger,
            ITicketOrchestrator ticketOrchestrator)
        {
            _mapper = mapper;
            _logger = logger;
            _ticketOrchestrator = ticketOrchestrator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ticket = await _ticketOrchestrator.GetAllAsync();

            return Ok(ticket);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticketById = await _ticketOrchestrator.GetByIdAsync(id);

            return Ok(ticketById);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CreateTicket ticket)
        {
            var model = _mapper.Map<CreateTicket, Ticket>(ticket);
            var result = await _ticketOrchestrator.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, EditTicket model)
        {
            var modelToUpdate = _mapper.Map<EditTicket, Ticket>(model);
            //modelToUpdate.Id = id;
            Ticket entity = await _ticketOrchestrator.UpdateAsync(id, modelToUpdate);

            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleteEntity = await _ticketOrchestrator.DeleteAsync(id);
            
            return Ok(deleteEntity);
        }

    }
}