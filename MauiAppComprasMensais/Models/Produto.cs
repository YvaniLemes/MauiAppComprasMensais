using SQLite;
using System.ComponentModel;

namespace MauiAppComprasMensais.Models
{
    public class Produto : INotifyPropertyChanged
    {
        // Inicializações para evitar CS8618
        private string _descricao = string.Empty;
        private double _quantidade = 0;
        private double _preco = 0;
        private DateTime _dataCadastro = DateTime.Now;

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

        // Evento ajustado para ser anulável (evita CS8612/CS8618)
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

