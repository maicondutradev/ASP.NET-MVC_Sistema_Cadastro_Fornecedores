using CadastroClientes.Models;

namespace CadastroClientes.Repository
{
    public interface IFornecedorRepository
    {
        Task<IEnumerable<Fornecedor>> GetAllAsync(string searchString);
        Task<Fornecedor> GetByIdAsync(int? id);
        Task AddAsync(Fornecedor fornecedor);
        Task UpdateAsync(Fornecedor fornecedor);
        Task DeleteAsync(int id);
        bool FornecedorExists(int id);
    }
}
