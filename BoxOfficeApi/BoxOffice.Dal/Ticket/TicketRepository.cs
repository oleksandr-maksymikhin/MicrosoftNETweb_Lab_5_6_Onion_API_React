using AutoMapper;
using BoxOffice.Model.Ticket;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Dal.Ticket
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _dbContext;

        public TicketRepository(
            IMapper mapper,
            AppDbContext context)
        {
            _mapper = mapper;
            _dbContext = context;
        }
        public async Task<Model.Ticket.Ticket> CreateAsync(Model.Ticket.Ticket model)
        {
            var entity = _mapper.Map<Model.Ticket.Ticket, TicketDao>(model);
            var result = await _dbContext.Tickets.AddAsync(entity);
            //Error when saving Changes:
            /*Microsoft.Data.SqlClient.SqlException(0x80131904): 
                Cannot insert the value NULL into column 'id', table 'TheaterTickets.dbo.Tickets'; 
                column does not allow nulls.INSERT fails.*/
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TicketDao, Model.Ticket.Ticket>(result.Entity);
        }
        public async Task<List<Model.Ticket.Ticket>> GetAllAsync()
        {
            var entities = await _dbContext.Tickets
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<TicketDao>, List<Model.Ticket.Ticket>>(entities);
        }
        public async Task<Model.Ticket.Ticket?> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);
            /*var entity = await _dbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == id);*/
            /*if (entity == null)
            {
                return null;
            }*/
            return _mapper.Map<TicketDao, Model.Ticket.Ticket>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var existingEntity = await _dbContext.Tickets.FirstAsync(t => t.Id == id);
            _dbContext.Tickets.Remove(existingEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Model.Ticket.Ticket> UpdateAsync(Model.Ticket.Ticket entity)
        {
            var existingEntity = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == entity.Id);
            existingEntity.Row = entity.Row;
            existingEntity.Seat = entity.Seat;
            existingEntity.Price = entity.Price;
            await _dbContext.SaveChangesAsync();
            return entity;

            //var existingEntity = _mapper.Map<Model.Ticket.Ticket, TicketDao>(entity);
            //var entityEntry = _dbContext.Tickets.Update(existingEntity);
            //return _mapper.Map<TicketDao, Model.Ticket.Ticket>(entityEntry.Entity);

            /*System.InvalidOperationException: 'The instance of entity type 'TicketDao' 
            cannot be tracked because another instance 
            with the same key value for {'Id'} is already being tracked. 
            When attaching existing entities, ensure that only one entity instance 
            with a given key value is attached. 
            Consider using 'DbContextOptionsBuilder.EnableSensitiveDataLogging' 
            to see the conflicting key values.'*/

        }
    }
}


