using CadastroClientes.Context;
using CadastroClientes.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Repository
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly AppDbContext _context;

        public FornecedorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Fornecedor>> GetAllAsync(string searchString)
        {
            var fornecedores = from f in _context.Fornecedores select f;
            if (!string.IsNullOrEmpty(searchString))
            {
                fornecedores = fornecedores.Where(f => f.Nome.Contains(searchString));
            }
            return await fornecedores.ToListAsync();
        }

        public async Task<Fornecedor> GetByIdAsync(int? id)
        {
            if (id == null) return null;
            return await _context.Fornecedores.FirstOrDefaultAsync(f => f.ID == id);
        }

        public async Task AddAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Fornecedor fornecedor)
        {
            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
                await _context.SaveChangesAsync();
            }
        }

        public bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(f => f.ID == id);
        }
    }
}
