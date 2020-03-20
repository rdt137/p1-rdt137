using System.Collections.Generic;
using PizzaBox.Client.Singletons;
using PizzaBox.Domain.Interfaces;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.UserInterface
{
  public class PizzaTypeUI : RepositorySingleton, IUserInterface
  {
    public static List<string> pizzatypes = new List<string>();
    public static List<PizzaType> ptModel = new List<PizzaType>();
    
    public void Get()
    {
      foreach (var pt in _pt.Get())
      {
        pizzatypes.Add(pt.ToString());
      }
    }

    public void GetPizzaType()
    {
      foreach (var pt in _pt.Get())
      {
        ptModel.Add(pt);
      }
    }
  }
}