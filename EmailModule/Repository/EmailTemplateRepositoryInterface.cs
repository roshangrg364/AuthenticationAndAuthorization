using EmailModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailModule.Repository
{
    public interface EmailTemplateRepositoryInterface
    {

     
        Task<EmailTemplate?> GetByType(string name);
        Task<IList<EmailTemplate>> GetAllAsync();
        Task InsertAsync(EmailTemplate entity);
        Task InsertRange(IList<EmailTemplate> entities);
        Task UpdateAsync(EmailTemplate entity);
        Task DeleteAsync(EmailTemplate entity);
        IQueryable<EmailTemplate> GetQueryable();
        Task<EmailTemplate> GetById(long id);
    }
}
