using System.Collections.Generic;

namespace CookieClicker
{
    public static class Achiev
    {
        static List<string> achievs = new List<string>();
        private static CookieClicker form;

        public static void Init(CookieClicker f)
        {
            form = f;
        }
        public static void Win(string name, string desc)
        {
            if (!achievs.Contains(name + "|" + desc))
            {
                achievs.Add(name + "|" + desc);
                form.say($"Achievement won! Name: {name}. Description: {desc}");
                form.milk += 4;
            }
        }
    }
}