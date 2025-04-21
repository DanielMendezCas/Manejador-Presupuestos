using ManejadorPresupuestos.Validations;
using System.ComponentModel.DataAnnotations;

namespace ManejadorPresupuestos.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo nombre es requerido")]
        [StringLength(maximumLength: 50)]
        [FirstUpperCaseLetter]
        public string AccountName { get; set; }
        [Display(Name = "Tipo de cuenta")]
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }
        public string AccountType { get; set; }
    }
}
