using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CadastroClientes.Models
{
    public class Fornecedor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve ter exatamente 14 dígitos numéricos!")]
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [RegularExpression("Comércio|Serviço|Indústria", ErrorMessage = "O segmento deve ser Comércio, Serviço ou Indústria!")]
        [Required]
        [Display(Name = "Segmento")]
        public string Segmento { get; set; }

        [Required]
        [Display(Name = "CEP")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "O CEP deve ter exatamente 8 dígitos numéricos!")]
        public string CEP { get; set; }

        [Required]
        [Display(Name = "Endereço")]
        [StringLength(255, ErrorMessage = "O endereço deve ter no máximo 255 caracteres!")]
        public string Endereco { get; set; }

        [StringLength(255)]
        public string FotoUsuarioPath { get; set; }

        [Display(Name = "Foto")]
        [NotMapped]
        public IFormFile FotoUsuario { get; set; }
    }
}
