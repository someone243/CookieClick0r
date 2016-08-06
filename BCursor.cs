using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms.VisualStyles;
using PlayerIOClient;

namespace CookieClicker
{
    /*
    public class Building
    {
       public static int quantity = 0;
      public  static double initCps = 0;
      public  static int initCost = 0;
       public static int cps = 0;
       public static int cost = 0;
        protected static bool[] upgrades = new bool[1000];
        public virtual void Buy(int qt)
        {
            quantity+= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (int) (initCost*Math.Pow(1.15,quantity));
        }

        public virtual void Sell(int qt)
        {
            quantity -= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (int) (initCost * Math.Pow(1.15, quantity));
        }

        public virtual void Upgrade(int id)
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

        public static int quantity = 0;
        public static double initCps = 0.15;
        public static int initCost = 15;
        public static double cps = 0;
        public static int cost = 15;
        public static bool[] upgrades = new bool[1000];
        public static void Buy(int qt)
        {

            quantity += qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (int)(initCost * Math.Pow(1.15, quantity));
            RecalculateCpS();
        }

        private static void RecalculateCpS()
        {
            double perSingle = 0.15;
            if (upgrades[1])
            {
                perSingle *= 2;
            }
            cps = perSingle * quantity;
        }

        public static void Sell(int qt)
        {
            quantity -= qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (int)(initCost * Math.Pow(1.15, quantity));
            RecalculateCpS();
        }

        public static void Upgrade(int id)
        {
            upgrades[id] = true;
        }

    }
}