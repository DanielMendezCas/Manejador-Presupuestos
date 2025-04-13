using ManejadorPresupuestos.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejadorPresupuestos.Models
{
    public class AccountType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [FirstUpperCaseLetter]
        [Remote(action: "CheckAccountTypeExists", controller:"AccountTypes")]
        public String? AccountName { get; set; }
        public int UserId { get; set; }
        public int Order { get; set; }
    }
}
