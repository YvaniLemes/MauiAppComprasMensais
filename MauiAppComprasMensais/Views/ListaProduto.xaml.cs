using MauiAppComprasMensais.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppComprasMensais.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
        public ListaProduto()
        {
            InitializeComponent();

            //Define a fonte de dados da ListView como a coleção lista 
            lst_produtos.ItemsSource = lista;
        }

        //Evento chamado quando a página aparece na tela
        protected async override void OnAppearing()
        {
            try
            {
                //Limpa a lista atual
                lista.Clear();

                //Busca todos os produtos do banco de dados
                List<Produto> tmp = await App.Db.GetAll();

                //Adiciona os produtos à coleção observável
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                //exibe menssagem em caso de erro
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        //Evento chamado ao clicar no botão da toolbar para adicionar o novo produto 
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Navega para página (NovoProduto) para adicionar o novo produto
                Navigation.PushAsync(new Views.NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        //Evento chamado quando o texto da busca é alterado
        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;

                lst_produtos.IsRefreshing = true;

                lista.Clear();

                //Busca produtos que correspondem à pesquisa
                List<Produto> tmp = await App.Db.Search(q);

                //Adiciona os resultados à coleção observável
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

        //Evento chamado ao clicar no botão da toolbar para calcular o total 
        private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            //soma os valores totais dos produtos
            double soma = lista.Sum(i => i.Total);

            string msg = $"O total é {soma:C}";

            //Exibe o total em um alerta
            await DisplayAlert("Total dos Produtos", msg, "OK");
        }

        //Evento chamado ao clicar na opção Delete 
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Obtém o item do menu que foi clicado
                MenuItem selecionado = sender as MenuItem;

                //obtém o produto associado ao item clicado
                Produto p = selecionado.BindingContext as Produto;

                //confirma se o usuário deseja remover o produto
                bool confirm = await DisplayAlert(
                    "Tem certeza", $"Remover {p.Descricao}?", "Sim", "Não");

                if (confirm)
                {
                    //Remove o produto do banco de dados e da lista
                    await App.Db.Delete(p.Id);
                    lista.Remove(p);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        //Evento chamado ao selecionar um item da lista
        private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    // Obtem o produto selecionado
                    Produto p = e.SelectedItem as Produto;

                    // Exemplo: navegar para edição
                    await Navigation.PushAsync(new Views.EditarProduto
                    {
                        BindingContext = p,
                    });

                    // 🔄 Reload da lista após selecionar
                    lista.Clear();
                    List<Produto> tmp = await App.Db.GetAll();
                    tmp.ForEach(i => lista.Add(i));

                    // Limpa a seleção para não ficar marcado
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
                //Limpa a lista atual
                lista.Clear();

                //Busca todos os produtos do banco de dados
                List<Produto> tmp = await App.Db.GetAll();

                //Adiciona os produtos à coleção observável
                tmp.ForEach(i => lista.Add(i));
            }
            catch (Exception ex)
            {
                //exibe menssagem em caso de erro
                await DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Relatorio());
        }
    }
}