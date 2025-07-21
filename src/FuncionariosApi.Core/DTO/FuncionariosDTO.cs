using System.ComponentModel.DataAnnotations;

namespace FuncionariosAPI.DTO;

public class FuncionariosDTO
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }
    
    [Required(ErrorMessage = "O Cargo é obrigatório")]
    [StringLength(50, ErrorMessage = "O cargo deve ter no máximo 50 caracteres")]
    public string Cargo { get; set; }
    
    [Required(ErrorMessage = "A data de nascimento é obrigatória")]
    public DateTime DataNascimento { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0")]
    public decimal Salario { get; set; }
}
