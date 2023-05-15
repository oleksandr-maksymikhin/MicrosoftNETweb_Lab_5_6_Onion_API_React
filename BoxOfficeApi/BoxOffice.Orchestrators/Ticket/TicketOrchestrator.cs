using BoxOffice.Model.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxOffice.Platform.Exception;

namespace BoxOffice.Orchestrators.Ticket
{
    public class TicketOrchestrator : ITicketOrchestrator
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketOrchestrator(
            ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Model.Ticket.Ticket> CreateAsync(Model.Ticket.Ticket model)
        {
            //add validation - if performance exists - create ticket for performance
            return await _ticketRepository.CreateAsync(model);
        }
        public async Task<List<Model.Ticket.Ticket>> GetAllAsync()
        {
            return await _ticketRepository.GetAllAsync();
        }

        public async Task<Model.Ticket.Ticket> GetByIdAsync(int id)
        {
            Model.Ticket.Ticket? ticketById = await _ticketRepository.GetByIdAsync(id);
            if (ticketById == null)
            {
                throw new ResourceNotFoundException($"Ticket with id = {id} not found");
            }

            return ticketById;
        }

        public async Task<Model.Ticket.Ticket> DeleteAsync(int id)
        {
            var entity = await _ticketRepository.GetByIdAsync(id);
            await _ticketRepository.DeleteAsync(id);
            return entity;
        }

        public async Task<Model.Ticket.Ticket> UpdateAsync(int id, Model.Ticket.Ticket modelToUpdate)
        {
            var existingEntity = await _ticketRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                throw new ResourceNotFoundException($"Ticket with id = {id} not found");
            }
            existingEntity.Row = modelToUpdate.Row;
            existingEntity.Seat = modelToUpdate.Seat;
            existingEntity.Price = modelToUpdate.Price;

            Model.Ticket.Ticket updateResult = await _ticketRepository.UpdateAsync(existingEntity);

            return updateResult;
        }
    }
}
