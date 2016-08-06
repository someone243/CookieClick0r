using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;
using PlayerIOClient;

namespace CookieClicker
{
    /*
    public class Building
    {
       public static double quantity = 0;
      public  static double initCps = 0;
      public  static double initCost = 0;
       public static double cps = 0;
       public static double cost = 0;
        protected static bool[] upgrades = new bool[1000];
        public virtual void Buy(double qt)
        {
            quantity+= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (double) (initCost*Math.Pow(1.15,quantity));
        }

        public virtual void Sell(double qt)
        {
            quantity -= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (double) (initCost * Math.Pow(1.15, quantity));
        }

        public virtual void Upgrade(double id)
        {
            upgrades[id] = true;
        }
        public Building()
        {
            cost = initCost;
        }
    }
    */
    public static class BCursor
    {

        public static double quantity = 0;
        public static double initCps = 0.1;
        public static double initCost = 15;
        public static double cps = 0;
        public static double cost = 15;
        public static bool[] upgrades = new bool[1000];
        public static void Buy(double qt)
        {

            quantity += qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (double)(initCost * Math.Pow(1.15, quantity));
            RecalculateCpS();
            if (quantity > 0) { Achiev.Win("Click", "Have 1 cursor.");}
            if (quantity > 1) { Achiev.Win("Double-click", "Have 2 cursors."); }
            if (quantity > 50) { Achiev.Win("Mouse wheel", "Have 50 cursors."); }
        }

        private static void RecalculateCpS()
        {
            double perSingle = 0.1;
            if (upgrades[1])
            {
                perSingle *= 2;
            }
            cps = perSingle * quantity;
        }

        public static void Sell(double qt)
        {
            quantity -= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (double)(initCost * Math.Pow(1.15, quantity));
            RecalculateCpS();
        }

        public static void Upgrade(int id)
        {
            upgrades[id] = true;
        }

    }
}