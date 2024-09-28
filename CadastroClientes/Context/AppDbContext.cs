using CadastroClientes.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroClientes.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Fornecedor> Fornecedores { get; set; }
    }
}
