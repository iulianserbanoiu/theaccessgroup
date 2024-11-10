namespace Booking.Application.Dtos
{
    public class ResponseDto<T>
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }

        public T Result { get; set; }

        public static ResponseDto<T> Success(T result)
        {
            return new ResponseDto<T>
            {
                Succeeded = true,
                Result = result
            };
        }

        public static ResponseDto<T> Error(string message)
        {
            return new ResponseDto<T>
            {
                Succeeded = false,
                Message = message
            };
        }
    }
}
