using System.ComponentModel.DataAnnotations;

namespace FuncionariosAPI.Models;

public class Funcionarios
{
    public long Id { get; set; }

    public string Nome { get; set; }
    
    public string Cargo { get; set; }
    
    public DateTime DataNascimento { get; set; }
    public decimal Salario { get; private set;}

    private Funcionarios()
    {
        
    }

    public Funcionarios(string nome, string cargo, DateTime dataNascimento, decimal salario)
    {
        Nome = nome ?? throw new ArgumentNullException("Nome não pode ser vazio",nameof(nome));
        Cargo = cargo ?? throw new ArgumentNullException("Cargo não pode ser vazio",nameof(cargo));
        DataNascimento = dataNascimento;
        Salario = ValidarSalario(salario);
    }
    
    private decimal ValidarSalario(decimal salario)
    {
        if(salario < 0)
            throw new ArgumentException("Salario Invalido", nameof(salario));
        
        return salario;
    }
    
    public decimal SalarioBruto()
    {
        return Salario;
    }
    
    public decimal SalarioLiquido()
    {
        var descontos = Salario * 0.075m + Salario * 0.07m + Salario * 0.06m + Salario * 0.10m;
        return Salario - descontos;
    }
    
    public decimal MudarSalario(decimal novoSalario)
    {
        Salario = ValidarSalario(novoSalario);

        return Salario;
    }
}
