using System;

namespace CookieClicker
{
    public static class BBank
    {

        public static double quantity = 0;
        public static double initCps = 1400;
        public static double initCost = 1400000;
        public static double cps = 0;
        public static double cost = 1400000;
        public static bool[] upgrades = new bool[1000];
        public static void Buy(double qt)
        {

            quantity += qt;
            quantity = quantity < 0 ? 0 : quantity;
            cost = (double)(initCost * Math.Pow(1.15, quantity));
            RecalculateCpS();
        }

        private static void RecalculateCpS()
        {
            double perSingle = initCps;
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