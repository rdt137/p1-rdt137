using System.ComponentModel.DataAnnotations;
using PizzaBox.Client2.Attributes;

namespace PizzaBox.Client2.Models
{
  public class LoginModel
  {
    [Required(ErrorMessage = "Please enter a username")]
    [DataType(DataType.Text)]
    [UsernameAttribute(ErrorMessage = "Username not available")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Please enter a password")]
    [DataType(DataType.Text)]
    [PasswordAttribute(ErrorMessage = "Incorrect Password")]
    public string Password { get; set; }
  }
}