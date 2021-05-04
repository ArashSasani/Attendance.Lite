using AutoMapper;

namespace WebApplication.API.Infrastructure
{
    public class DtoMapping
    {
        public static void Map()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<CMS.Service.Infrastructure.MappingProfile>();
                cfg.AddProfile<AttendanceManagement.Service.Infrastructure.MappingProfile>();
            });
        }
    }
}