namespace SeaEco.Abstractions.ResponseService;

public sealed class Response
{
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    
    public static Response Ok() => new Response { IsError = false };
    public static Response Error(string message) => new Response { IsError = true, ErrorMessage = message };
}

public sealed class Response<T>
{
    public bool IsError { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T Value { get; set; }
    
    public static Response<T> Ok(T value) => new Response<T> { IsError = false, Value = value };
    public static Response<T> Error(string message) => new Response<T> { IsError = true, ErrorMessage = message };
}