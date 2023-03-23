using System.ComponentModel.DataAnnotations;

namespace Web.Infrastructure;

public class ContactViewModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "campo requerido")]
    public string Name { get; set; }
    [Required(ErrorMessage = "campo requerido")]
    [EmailAddress(ErrorMessage = "que correo raro...")]
    public string Email { get; set; }
    public string Phone { get; set; }
    [Required(ErrorMessage = "campo requerido")]
    public string Message { get; set; }
    public DateTime CreatedDateTime { get; set; }
}