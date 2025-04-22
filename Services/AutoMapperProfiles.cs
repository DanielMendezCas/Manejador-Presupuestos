using AutoMapper;
using ManejadorPresupuestos.Models;

namespace ManejadorPresupuestos.Services
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Account, AccountCreateViewModel>();
        }
    }
}
