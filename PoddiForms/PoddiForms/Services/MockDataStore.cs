using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cave.Poddi;

namespace PoddiForms.Services
{
    public class MockDataStore : IDataStore<PoddiEpisode>
    {
        readonly IList<PoddiEpisode> items;

        public MockDataStore()
        {
            var source = "https://www.swr.de/~podcast/swr1/bw/leute-bw-podcast-100.xml";
            items = PoddiFeed.Load(source).Episodes.ToList<PoddiEpisode>();
        }

        public async Task<bool> AddItemAsync(PoddiEpisode item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(PoddiEpisode item)
        {
            var oldItem = items.Where((PoddiEpisode arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((PoddiEpisode arg) => arg.Id.ToString() == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<PoddiEpisode> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public async Task<IEnumerable<PoddiEpisode>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}