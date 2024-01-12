using System.ComponentModel.DataAnnotations;

namespace SimpleUrlList.Web.Options;

public class AppOptions
{
    [Required(ErrorMessage = "ApiUrl is required")]
    public string ApiUrl { get; set; }
}