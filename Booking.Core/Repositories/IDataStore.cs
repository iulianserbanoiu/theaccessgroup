namespace Booking.Core.Repositories
{
    public interface IDataStore<T>
    {
        IEnumerable<T>? Data { get; set; }
    }
}
