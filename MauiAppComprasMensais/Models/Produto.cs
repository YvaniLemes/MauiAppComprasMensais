using SQLite;
using System.ComponentModel;

namespace MauiAppComprasMensais.Models
{
    public class Produto : INotifyPropertyChanged
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
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged(nameof(Descricao));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Quantidade
        {
            get => _quantidade;
            set
            {
                if (_quantidade != value)
                {
                    _quantidade = value;
                    OnPropertyChanged(nameof(Quantidade));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Preco
        {
            get => _preco;
            set
            {
                if (_preco != value)
                {
                    _preco = value;
                    OnPropertyChanged(nameof(Preco));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Total => Quantidade * Preco;

        public DateTime DataCadastro
        {
            get => _dataCadastro;
            set
            {
                if (_dataCadastro != value)
                {
                    _dataCadastro = value;
                    OnPropertyChanged(nameof(DataCadastro));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
