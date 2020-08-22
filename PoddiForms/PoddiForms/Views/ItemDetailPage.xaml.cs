using System.ComponentModel;
using Xamarin.Forms;
using PoddiForms.ViewModels;

namespace PoddiForms.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}