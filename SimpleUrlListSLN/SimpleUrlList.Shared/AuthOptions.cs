using System.ComponentModel.DataAnnotations;

namespace SimpleUrlList.Shared;

public class AuthOptions
{
    public const string ApiKeyHeaderName = "X-Api-Key";
    [Required(ErrorMessage = "Api key must be defined")] 
    public string ApiKey { get; set; }
    public string BaseUrl { get; set; }
}