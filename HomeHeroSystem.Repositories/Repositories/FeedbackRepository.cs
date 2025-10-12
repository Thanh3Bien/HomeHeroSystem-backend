using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHeroSystem.Repositories.Entities;
using HomeHeroSystem.Repositories.Infrastructures;
using HomeHeroSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeHeroSystem.Repositories.Repositories
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(HomeHeroContext context, ILogger logger)
           : base(context, logger)
        {
        }
        public async Task<List<Feedback>> GetFeedbacksByBookingIdsAsync(List<int> bookingIds)
        {
            try
            {
                return await _context.Feedbacks
                    .Where(f => bookingIds.Contains(f.BookingId) &&
                               f.Rating.HasValue)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
