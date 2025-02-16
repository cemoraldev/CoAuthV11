namespace SharedLibrary.Dtos;

public class ErrorDto
{
    public List<string> Errors { get; private set; } = new List<string>();
    
    public bool IsShown { get; private set; }

    public ErrorDto()
    {
        Errors = new List<string>();
    }

    public ErrorDto(string error,bool isShown)
    {
        Errors.Add(error);
        IsShown = isShown;
    }

    public ErrorDto(List<string> errors,bool isShown)
    {
        Errors = errors;
        IsShown = isShown;
    }
}