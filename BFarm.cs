using System;

namespace CookieClicker
{
    static public class BFarm
    {
        public static int quantity = 0;
        public static double initCps = 8;
        public static int initCost = 1100;
        public static double cps = 0;
        public static int cost = 1100;
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