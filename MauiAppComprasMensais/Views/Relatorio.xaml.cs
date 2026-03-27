using MauiAppComprasMensais.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppComprasMensais.Views
{
    public partial class Relatorio : ContentPage
    {
        ObservableCollection<Produto> resultados = new ObservableCollection<Produto>();

        public Relatorio()
        {
            InitializeComponent();
            dateInicio.Date = DateTime.Now.AddDays(-60); // últimos 60 dias
            dateFinal.Date = DateTime.Now; // data de hoje
            lst_relatorio.ItemsSource = resultados;
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            DateTime inicio = dateInicio.Date;
            DateTime final = dateFinal.Date;

            if (inicio > final)
            {
                await DisplayAlert("Ops", "Data de Início deve ser menor que a Data Final!", "OK");
                return;
            }

            if ((final - inicio).TotalDays > 365)
            {
                await DisplayAlert("Ops", "O intervalo deve ser até um ano.", "OK");
                return;
            }

            try
            {
                var todosProdutos = await App.Db.GetAll();
                var filtrados = todosProdutos
                    .Where(p => p.DataCadastro != default &&
                                p.DataCadastro.Date >= inicio &&
                                p.DataCadastro.Date <= final)
                    .ToList();

                resultados.Clear();
                filtrados.ForEach(p => resultados.Add(p));

                if (!filtrados.Any())
                {
                    await DisplayAlert("Ops!", "Nenhum produto encontrado nesse período!", "OK");
                }
                else
                {
                    double soma = filtrados.Sum(p => p.Total);
                    int qtd = filtrados.Count;
                    await DisplayAlert("Resumo",
                        $"Produtos no período: {qtd}\nValor total: {soma:C}",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}
