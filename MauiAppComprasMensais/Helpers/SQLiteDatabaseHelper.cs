// importa o namespace onde está contida a classe Produto
using MauiAppComprasMensais.Models;
// importa a biblioteca SQLite para manipular o banco de dados
using SQLite;

namespace MauiAppComprasMensais.Helpers
{
    // Classe responsável por interagir com o banco de dados
    public class SQLiteDatabaseHelper
    {
        // Conexão assíncrona com o banco
        readonly SQLiteAsyncConnection _conn;

        // Construtor: abre conexão e cria tabela Produto se não existir
        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            Task.Run(async () => await _conn.CreateTableAsync<Produto>()).Wait();
        }

        // Inserir novo produto
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Atualizar produto existente
        public async Task<int> Update(Produto p)
        {
            // Atualiza todos os campos, incluindo DataCadastro
            string sql = "UPDATE Produto " +
                         "SET Descricao = ?, Quantidade = ?, Preco = ?, DataCadastro = ? " +
                         "WHERE Id = ?";

            return await _conn.ExecuteAsync(sql,
                p.Descricao,
                p.Quantidade,
                p.Preco,
                p.DataCadastro,
                p.Id);
        }

        // Remover produto pelo Id
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        // Buscar todos os produtos
        public async Task<List<Produto>> GetAll()
        {
            try
            {
                return await _conn.Table<Produto>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar produtos: {ex.Message}");
                return new List<Produto>();
            }
        }

        // Buscar produtos por descrição
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, $"%{q}%");
        }
    }
}
