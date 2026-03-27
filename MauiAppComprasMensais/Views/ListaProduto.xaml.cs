using MauiAppComprasMensais.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace MauiAppComprasMensais.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                lista.Clear();
                List<Produto> tmp = await App.Db.GetAll();

                // ✅ Ordena pela DataCadastro (mais recente primeiro)
                tmp = tmp.OrderByDescending(p => p.DataCadastro).ToList();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;
                lst_produtos.IsRefreshing = true;

                lista.Clear();
                List<Produto> tmp = await App.Db.Search(q);

                // ✅ Ordena resultados da busca
                tmp = tmp.OrderByDescending(p => p.DataCadastro).ToList();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            await DisplayAlert("Total dos Produtos", msg, "OK");
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is MenuItem selecionado && selecionado.BindingContext is Produto p)
                {
                    bool confirm = await DisplayAlert(
                        "Tem certeza", $"Remover {p.Descricao}?", "Sim", "Não");

                    if (confirm)
                    {
                        await App.Db.Delete(p.Id);
                        lista.Remove(p);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem is Produto p)
                {
                    await Navigation.PushAsync(new EditarProduto(p));

                    // ✅ Recarrega lista ordenada
                    lista.Clear();
                    List<Produto> tmp = await App.Db.GetAll();
                    tmp = tmp.OrderByDescending(i => i.DataCadastro).ToList();
                    tmp.ForEach(i => lista.Add(i));

                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            try
            {
                lista.Clear();
                List<Produto> tmp = await App.Db.GetAll();

                // ✅ Ordena ao atualizar
                tmp = tmp.OrderByDescending(p => p.DataCadastro).ToList();

                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Relatorio());
        }
    }
}
