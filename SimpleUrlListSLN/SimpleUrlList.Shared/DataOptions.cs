using System.ComponentModel.DataAnnotations;

namespace SimpleUrlList.Shared;

public class DataOptions
{
    [Required(ErrorMessage = "The connection string is required.")]
    public string ConnectionString { get; set; }
}