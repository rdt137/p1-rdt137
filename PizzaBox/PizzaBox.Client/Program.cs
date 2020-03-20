using System;
using System.Collections.Generic;
using System.Linq;
using PizzaBox.Client.Singletons;
using PizzaBox.Client.UserInterface;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Databases;
using PizzaBox.Storing.Repositories;

namespace PizzaBox.Client
{
  class Program : RepositorySingleton
  {
    private static PizzaBoxDbContext db = new PizzaBoxDbContext();
    private static PizzaBoxDbContext _db = db.Instance;
    public static int[] pizzaSelect = new int[] {0, 0, 0};
    public static string[] pizzaType = new string[] {"Pep", "Cheese", "PinBac"};
    
    static void Main(string[] args)
    {      
      string userType = "";
      string user = "";
      userType = Login(out user, userType);

      if(userType == "Customer")
        UI.CustomerUI(user);
      if(userType == "Admin")
        UI.AdminUI();   

    }

    public static string Login(out string user, string userType)
    {
      string username = null;
      string password = null;
      while(_us.Get(username) is null)
      {
        Console.WriteLine("Enter username:");
        Console.WriteLine("(Ctrl + C) to exit");
        username = Console.ReadLine();        
      }
      while(_us.Check(username, password) is null)
      {
        Console.WriteLine("\nEnter password:");
        Console.WriteLine("(Ctrl + C) to exit");
        password = Console.ReadLine();
      }

      user = username;
     
      userType = (from u in _db.User
                   where u.UserId == username
                   select u.UserType).FirstOrDefault();
      return userType;
    }    
  }
}