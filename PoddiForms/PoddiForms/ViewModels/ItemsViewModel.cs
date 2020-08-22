using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using PoddiForms.Views;
using Cave.Poddi;

namespace PoddiForms.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private PoddiEpisode _selectedItem;

        public ObservableCollection<PoddiEpisode> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<PoddiEpisode> ItemTapped { get; }
        public Command<PoddiEpisode> ItemSwiped { get; }
        public Command<PoddiEpisode> ItemSwipedLeft { get; }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<PoddiEpisode>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<PoddiEpisode>(OnItemSelected);
            ItemSwiped = new Command<PoddiEpisode>(OnItemSwiped);
            ItemSwipedLeft = new Command<PoddiEpisode>(OnItemSwipedLeft);

            AddItemCommand = new Command(OnAddItem);
        }


        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public PoddiEpisode SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            // await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(PoddiEpisode item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
        }

        async void OnItemSwiped(PoddiEpisode item)
        {
            if (item == null)
                return;
            await Shell.Current.DisplayAlert("Item", item.ToString(), "Ok");
        }

        async void OnItemSwipedLeft(PoddiEpisode item)
        {
            if (item == null)
                return;
            await DataStore.DeleteItemAsync(item.Id.ToString());
            Items.Remove(item);          
        }

    }
}