namespace Kernel.Shared.Entities;

public class ErrorResponse
{
    public ErrorDetails Details { get; set; }
}

public class ErrorDetails
{ 
    public int Code { get; set; }
    public string Message { get; set; }
}
