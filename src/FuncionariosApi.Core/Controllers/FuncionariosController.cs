using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FuncionariosAPI.Data;
using FuncionariosAPI.DTO;
using FuncionariosAPI.Models;

namespace FuncionariosAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionariosController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Funcionarios>>> GetAll()
        {
            var funcionarios = await _context
                .Funcionarios
                .AsNoTracking()
                .ToListAsync();
            
            return Ok(funcionarios);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Funcionarios>> Get([FromRoute] long id)
        {
            var funcionarios = await _context
                .Funcionarios
                .FindAsync(id);
            
            return funcionarios ==null? NotFound() : Ok(funcionarios);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Post([FromBody] FuncionariosDTO funcionariosDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var funcionarios = new Funcionarios(
                funcionariosDTO.Nome,
                funcionariosDTO.Cargo,
                funcionariosDTO.DataNascimento,
                funcionariosDTO.Salario
            );
            
            _context.Add(funcionarios);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = funcionarios.Id }, funcionarios);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Put([FromBody] FuncionariosDTO funcionariosDTO, [FromRoute] long id)
        {
            var funcionarios = await _context.Funcionarios.FindAsync(id);
            if (funcionarios == null)
                return NotFound(funcionariosDTO);

            try
            {
                funcionarios.Nome = funcionariosDTO.Nome;
                funcionarios.Cargo = funcionariosDTO.Cargo;
                funcionarios.DataNascimento = funcionariosDTO.DataNascimento;
                funcionarios.MudarSalario(funcionariosDTO.Salario);
              
                await _context.SaveChangesAsync();
                
                return Ok(funcionarios);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] long id)
        {
            var funcionarios = await _context.Funcionarios.FindAsync(id);
            if (funcionarios == null)
                return NotFound(id);

            _context.Funcionarios.Remove(funcionarios);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpGet("{id}/salario-bruto")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Decimal>> GetSalarioBruto([FromRoute] long id)
        {
            var funcionarios = await _context.Funcionarios.FindAsync(id);
            if (funcionarios == null)
                return NotFound(id);
            
            return Ok(funcionarios.SalarioBruto());
        }

        [HttpGet("{id}/salario-liquido")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Decimal>> GetSalarioLiquido([FromRoute] long id)
        {
            var funcionarios = await _context.Funcionarios.FindAsync(id);
            if (funcionarios == null)
                return NotFound(id);
            
            return Ok(funcionarios.SalarioLiquido());
        }
    }
}
