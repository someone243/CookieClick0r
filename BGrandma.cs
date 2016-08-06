using System;

namespace CookieClicker
{
    public static class BGrandma
    {

        public static int quantity = 0;
        public static double initCps = 1;
        public static int initCost = 100;
        public static double cps = 0;
        public static int cost = 100;
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
            double perSingle = 1;
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