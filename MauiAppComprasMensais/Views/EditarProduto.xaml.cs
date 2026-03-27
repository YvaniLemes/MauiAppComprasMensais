using MauiAppComprasMensais.Models;
using System;

namespace MauiAppComprasMensais.Views
{
    public partial class EditarProduto : ContentPage
    {
        private Produto _produto;
        public EditarProduto(Produto produto)
        {
            InitializeComponent();
            _produto = produto ?? throw new ArgumentNullException(nameof(produto));
            BindingContext = produto; // recebe o mesmo objeto da lista
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Produto produto_anexado = BindingContext as Produto;

                if (produto_anexado != null)
                {
                    // atualiza as propriedades diretamente
                    produto_anexado.Descricao = txt_descricao.Text;
                    produto_anexado.Quantidade = Convert.ToDouble(txt_quantidade.Text);
                    produto_anexado.Preco = Convert.ToDouble(txt_preco.Text);
                    produto_anexado.DataCadastro = dt_dataCompra.Date;

                    // salva no banco
                    await App.Db.Update(produto_anexado);

                    await DisplayAlert("Sucesso!", "Registro Atualizado", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Erro", "Produto não encontrado no BindingContext", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops!", ex.Message, "OK");
            }
        }
    }
}
