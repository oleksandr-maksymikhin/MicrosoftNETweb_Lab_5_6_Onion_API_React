using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOffice.Model.Ticket
{
    public interface ITicketOrchestrator
    {
        Task<Ticket> CreateAsync(Ticket model);
        Task<List<Ticket>> GetAllAsync();
        Task<Ticket> GetByIdAsync(int id);
        Task<Ticket> DeleteAsync(int id);
        Task<Ticket> UpdateAsync(int id, Ticket modelToUpdate);
    }
}
