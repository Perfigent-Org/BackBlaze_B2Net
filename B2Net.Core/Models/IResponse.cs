namespace B2Net.Core.Models
{
    public interface IResponse
    {
        bool IsSuccessful { get; set; }
        string Message { get; set; }
    }
}
