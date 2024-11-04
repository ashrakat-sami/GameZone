
namespace GameZone.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _context;

        public DevicesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetSelectLists()
        {
            return _context.Devices
                .Select(c=> new SelectListItem { Value=c.Id.ToString(), Text=c.Name })
                .OrderBy(x => x.Value)
                .AsNoTracking()
                .ToList();
                
        }
    }
}
