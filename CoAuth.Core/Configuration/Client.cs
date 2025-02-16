namespace CoAuth.Core.Configuration;

public class Client
{
    public string Id { get; set; }
    
    public string Secret { get; set; }
    
    //Nerelere erişebileceğini tutar. Örn : www.myapi1.com , www.myapi2.com
    public List<string> Audiences { get; set; }
}