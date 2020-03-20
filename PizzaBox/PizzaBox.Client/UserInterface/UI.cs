using System;
using System.Collections;
using System.Collections.Generic;
using PizzaBox.Client.Singletons;
using PizzaBox.Domain.Models;
using PizzaBox.Storing.Databases;
using PizzaBox.Storing.Repositories;
using System.Linq;

namespace PizzaBox.Client.UserInterface
{
  public class UI : RepositorySingleton
  {
    private static readonly PizzaBoxDbContext db = new PizzaBoxDbContext();
    public static PizzaBoxDbContext _db = db.Instance;   
    public static void CustomerUI(string username)
    {
      StoreUI stUI = new StoreUI();      
      stUI.Get();
      PizzaTypeUI ptUI = new PizzaTypeUI();      
      ptUI.Get();
      SizeUI szUI = new SizeUI();
      szUI.Get();     
      

      var pztype = PizzaTypeUI.pizzatypes;
      var size = SizeUI.sizes;
      var loc = StoreUI.locations;
      
      Console.WriteLine("\n\nWelcome to Papa's Pizzas");
      
      var userLoc = 0;
      var userD = _us.Get(username);
      var oDate = _o.Get(userD);
      TimeSpan span = new TimeSpan();
      try
      {        
        span = DateTime.Now.Subtract(oDate.GetDate());
      }
      catch(Exception) {}
      
      while(userLoc != 1 && userLoc != 2 && userLoc != 3)
      {             
        try
        {
          Console.WriteLine("You can only order from {0} in the next {1} hours", oDate.GetLocation(), 24 - span.Hours);
        }
        catch (Exception) {}
        
        for(int i = 0; i < loc.Count; i++)
        {
          Console.Write("\n({0}) {1} \t", i + 1, loc[i]);
        }
        Console.WriteLine("\n");
        int.TryParse(Console.ReadLine(), out userLoc );
      }
        
      // set store location based on answer
      var typeId = new SortedList();
      var sizeId = new SortedList();
      var pizzaCount = 0;

      var ans2 = 0;      
      while(ans2 != 3)
      {
        try
        {
          if(span.Hours < 24 && !(oDate.GetLocation() == loc[userLoc - 1]))
          {
            Console.WriteLine("Can't order from this store.");
            break;
          } 
        }
        catch(Exception) {}

        Console.WriteLine("\n(1) Add Pizza");
        Console.WriteLine("(2) Remove Pizza");
        Console.WriteLine("(3) Checkout");
        Console.WriteLine("(Ctrl + C) Exit\n");
        int.TryParse(Console.ReadLine(), out ans2 );

        // add pizza
        if(ans2 == 1)
        {
          Console.WriteLine("\nWhich pizza would you like?");
          for(int i = 0; i < pztype.Count; i++)
          {
            Console.Write("({0}) {1} \t", i + 1, pztype[i]);
          }
          Console.WriteLine("\n");

          var ans3 = 0;
          var ansArray = new List<int>() {1, 2, 3};
          int.TryParse(Console.ReadLine(), out ans3 );
          if(ansArray.Contains(ans3))
          {
            Console.WriteLine("\nWhat size would you like?");
            for(int i = 0; i < size.Count; i++)
            {
              Console.Write("({0}) {1} \t", i + 1, size[i]);
            }
            Console.WriteLine("\n");

            var ans4 = 0;
            
            var ansArray2 = new List<int>() {1, 2, 3};
            int.TryParse(Console.ReadLine(), out ans4 );
            if(ansArray2.Contains(ans4))
            {
              typeId.Add(pizzaCount, ans3);
              sizeId.Add(pizzaCount, ans4);  
              pizzaCount++;       
            }
            else
              Console.WriteLine("Enter a valid size");                       
          }
          else
            Console.WriteLine("Enter a valid pizza");
        }

        // remove pizza
        else if(ans2 == 2)
        {
          Console.WriteLine("\nWhich pizza would you like to remove?");

          Console.WriteLine("\nChoose number to remove:");
          for (int i = 0; i < typeId.Count; i++)
          {
            Console.WriteLine("({2}) {0} {1}", size[(int) sizeId.GetByIndex(i) - 1], pztype[(int) typeId.GetByIndex(i) - 1], i + 1);
          }
          Console.WriteLine("\n");

          var removeAns = 0;
          int.TryParse(Console.ReadLine(), out removeAns );
          if(removeAns >= 0 && removeAns <= typeId.Count)
          {
            typeId.RemoveAt(removeAns - 1);
            sizeId.RemoveAt(removeAns - 1);
            //x--;
          }
          else
            Console.WriteLine("Enter a valid pizza number");
        }

        // checkout
        else if(ans2 == 3)
        {
          Console.WriteLine("\nFinal Order List:");
          for (int i = 0; i < typeId.Count; i++)
          {
            Console.WriteLine("{0} {1}", size[(int) sizeId.GetByIndex(i) - 1], pztype[(int) typeId.GetByIndex(i) - 1]);
          }        
                    
          // stUI = new StoreUI();
          // stUI.GetLocation();
          // var locModel = StoreUI.locModel;

          var pt = _pt.Get();
          var s = _sz.Get();          
          var l = _st.Get();

          var o = new Order();
          var st = new Store();

          //o.Location = locModel[userLoc - 1];
          List<PizzaType> ptCost = new List<PizzaType>();
          List<Size> sCost = new List<Size>();
          for(int i = 0; i < typeId.Count; i++)
          {
            ptCost.Add(pt[(int) typeId.GetByIndex(i) - 1]);
            sCost.Add(s[(int) sizeId.GetByIndex(i) - 1]);
          }
          
          // get total cost of order
          decimal ptOrderCost = 0;
          decimal sOrderCost = 0;
          for(int i = 0; i < typeId.Count; i++)
          {
            decimal ptAdd = 0;
            decimal.TryParse(ptCost[i].GetCost(), out ptAdd );
            decimal sAdd = 0;
            decimal.TryParse(sCost[i].GetCost(), out sAdd );
            ptOrderCost += ptAdd;
            sOrderCost += sAdd;
          }
          var oCost = ptOrderCost + sOrderCost;
          if(oCost > 250)
          {
            Console.WriteLine("Limit of $250 reached");
            break;
          }
          
          o.Cost = oCost;
          

          var user = _us.Get(username);
          o.User = user;
          o.OrderDate = DateTime.Now;
          o.Location = l[userLoc - 1];
          
          _o.Update(o);

          for(int i = 0; i < typeId.Count; i++)
          {
            var p = new Pizza();

            var pizzaType = pt[(int) typeId.GetByIndex(i) - 1];
            var siz = s[(int) sizeId.GetByIndex(i) - 1];
            

            pizzaType.Pizzas = new List<Pizza> { p }; // p.crust = *crustId
            siz.Pizzas = new List<Pizza> { p };

            p.Cost = pizzaType.Cost + siz.Cost;

            p.Order = o;

            var worked = _pr.Update(p);

            
          }

          break;
        }

        // handling exception
        else
          continue;
  
        // display what customers currently ordered
        Console.WriteLine("\nCurrent Order List:");
        for (int i = 0; i < typeId.Count; i++)
        {
          Console.WriteLine("{0} {1}", size[(int) sizeId.GetByIndex(i) - 1], pztype[(int) typeId.GetByIndex(i) - 1]);
        }      
      }      
    }

    public static void AdminUI()
    {
      StoreUI stUI = new StoreUI();      
      stUI.Get();   

      var loc2 = StoreUI.locations;
      
      var adminLoc = 0;
      for(int i = 0; i < loc2.Count; i++)
      {
        Console.Write("\n({0}) {1} \t", i + 1, loc2[i]);
      }
      Console.WriteLine("\n");
      int.TryParse(Console.ReadLine(), out adminLoc );
      
      var adminAns = 0;
      while(adminAns != 4)
      {  
        Console.WriteLine("\n(1) Order History");
        Console.WriteLine("(2) Pizzas for specified order (regardless of store)");
        Console.WriteLine("(3) Cost per order (regardless of store)");
        Console.WriteLine("(4) Exit\n");
        int.TryParse(Console.ReadLine(), out adminAns );
        
        //order history
        if(adminAns == 1)
        {          
          var store = _st.Get();
          var orders = from o in _db.Order
                        where o.Location == store[adminLoc - 1]
                        select new {o.OrderId, o.Cost, o.OrderDate, o.User, o.Location};
          foreach (var order in orders)
          {
            Console.WriteLine(order);
          }
          Console.ReadKey();
        }

        //pizzas for specified order
        if(adminAns == 2)
        {
          var spOrder = 0;
          Console.WriteLine("Enter a specific order: ");
          int.TryParse(Console.ReadLine(), out spOrder );
          Order oId;
          try
          {
            oId = _o.Get(spOrder);
          }
          catch(Exception)
          {
            Console.WriteLine("Wrong input");
            break;
          }
          
          var pizzas = from p in _db.Pizza
                      where p.Order == oId
                      select new {p.PizzaType, p.Size, p.Cost, p.Order};
          foreach (var pizza in pizzas)
          {
            Console.WriteLine(pizza);
          }
          Console.ReadKey();
        }

        // pizza cost per order
        if(adminAns == 3)
        {          
          var orderCost = new decimal[_o.Get().Count];
          for(int i = 0; i < _o.Get().Count; i++)
          {
            var id = _o.Get(i + 1);
            var pizzaCosts = from p in _db.Pizza
                            where p.Order == id
                            select p.Cost;
            foreach (var pizza in pizzaCosts)
            {
              orderCost[i] += pizza;
            }
          }
          for(int i = 0; i < _o.Get().Count; i++)
          {
            Console.WriteLine("{0}\t\tOrderCost: {1}", _o.Get()[i], orderCost[i]);
          }
          Console.ReadKey();
        }
      }
    }
  }
}