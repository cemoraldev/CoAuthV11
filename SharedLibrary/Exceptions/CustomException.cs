using System.Runtime.Serialization;

namespace SharedLibrary.Exceptions;

public class CustomException:Exception
{
    public CustomException()
    {
    }

    protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public CustomException(string? message) : base(message)
    {
    }

    public CustomException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}