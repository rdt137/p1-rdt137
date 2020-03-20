using PizzaBox.Client.UserInterface;
using PizzaBox.Storing.Databases;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client.Singletons
{
  
  public class RepositorySingleton
  {
    protected static readonly PizzaRepository _pr = new PizzaRepository();
    protected static readonly PizzaTypeRepository _pt = new PizzaTypeRepository();
    protected static readonly SizeRepository _sz = new SizeRepository();
    protected static readonly OrderRepository _o = new OrderRepository();
    protected static readonly UserRepository _us = new UserRepository();
    protected static readonly StoreRepository _st = new StoreRepository();
    protected static readonly PizzeriaSingleton _ps = PizzeriaSingleton.Instance;
  }
}