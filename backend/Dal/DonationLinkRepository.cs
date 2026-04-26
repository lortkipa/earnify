using Data;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dal
{
    public interface IDonationLinkRepository : IBaseRepository<DonationLink>
    {
        Task<IEnumerable<DonationLink>> GetByCreatorId(int creatorId);
    }

    public class DonationLinkRepository : BaseRepository<DonationLink>, IDonationLinkRepository
    {
        private readonly ProjectContext _context;
        public DonationLinkRepository(ProjectContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DonationLink>> GetByCreatorId(int creatorId)
        {
            return _context.DonationLinks
                .Where(link => link.CreatorId == creatorId)
                .ToList();
        }
    }
}
