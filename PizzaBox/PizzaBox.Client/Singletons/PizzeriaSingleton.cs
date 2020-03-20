using System.Collections.Generic;
using System.Linq;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client.Singletons
{
  public class PizzeriaSingleton
  {
    private static readonly PizzaRepository _pr = new PizzaRepository();
    private static readonly PizzeriaSingleton _ps = new PizzeriaSingleton();
    public static PizzeriaSingleton Instance { get { return _ps; } }
    private PizzeriaSingleton() {}

    public List<Pizza> Get()
    {
      return _pr.Get();
    }

    public bool Update(PizzaType pizzaType, Size size, Order order)
    {
      var p = new Pizza();

      pizzaType.Pizzas = new List<Pizza> { p }; // p.crust = *crustId
      size.Pizzas = new List<Pizza> { p };
      //p.Cost = pizzaType.Cost + size.Cost;
      p.Order = order;

      return _pr.Update(p);
    }
  }
}