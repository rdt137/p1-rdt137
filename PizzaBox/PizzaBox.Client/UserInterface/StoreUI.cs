using System.Collections.Generic;
using PizzaBox.Client.Singletons;
using PizzaBox.Domain.Interfaces;
using PizzaBox.Domain.Models;

namespace PizzaBox.Client.UserInterface
{
  public class StoreUI : RepositorySingleton, IUserInterface
  {
    public static List<string> locations = new List<string>();

    public static List<Store> locModel = new List<Store>();
    
    public void Get()
    {
      foreach (var s in _st.Get())
      {
        locations.Add(s.ToString());
      }
    }

    public void GetLocation()
    {
      foreach (var s in _st.Get())
      {
        locModel.Add(s);
      }
    }
  }
}