using SQLite;

namespace MauiAppComprasMensais.Models
{
    public class Produto
    {
        string _descricao;
        double _quantidade;
        double _preco;
        DateTime _dataCadastro = DateTime.Now;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set => _descricao = value;
        }

        public double Quantidade
        {
            get => _quantidade;
            set => _quantidade = value;
        }

        public double Preco
        {
            get => _preco;
            set => _preco = value;
        }

        public double Total => Quantidade * Preco;

        public DateTime DataCadastro
        {
            get => _dataCadastro;
            set => _dataCadastro = value;
        }
    }
}
