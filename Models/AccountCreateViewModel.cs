using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManejadorPresupuestos.Models
{
    public class AccountCreateViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountTypes {  get; set; }
    }
}
