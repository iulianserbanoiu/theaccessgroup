using Booking.Core.Repositories;
using Booking.Infra.Converters;
using System.Text.Json;

namespace Booking.Infra.Persistance
{
    public class DataStore<T> : IDataStore<T> where T : class
    {
        public IEnumerable<T>? Data { get; set; }

        public DataStore(string filePath)
        {
            LoadData(filePath);
        }

        private void LoadData(string filePath)
        {
            string json = File.ReadAllText(filePath);
            var opt = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            opt.Converters.Add(new CustomDateJsonConverter());
            Data = JsonSerializer.Deserialize<IList<T>>(json, opt);
        }
    }
}
