using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.Services
{
    public class SettingServices
    {
        private readonly AppDbContext _context;

        public SettingServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetSettings()
        {
            return await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
        }

    }
}
