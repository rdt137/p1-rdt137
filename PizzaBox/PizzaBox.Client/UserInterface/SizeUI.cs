using System.Collections.Generic;
using PizzaBox.Client.Singletons;
using PizzaBox.Domain.Interfaces;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.UserInterface
{
  public class SizeUI : RepositorySingleton, IUserInterface
  {
    public static List<string> sizes = new List<string>();
    public static List<Size> sizeModel = new List<Size>();
    
    public void Get()
    {
      foreach (var s in _sz.Get())
      {
        sizes.Add(s.ToString());
      }
    }

    public void GetModel()
    {
      foreach (var s in _sz.Get())
      {
        sizeModel.Add(s);
      }
    }
  }
}