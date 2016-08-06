using JetBrains.Annotations;
namespace CookieClicker
{
    public static class _EXTS
    {
        [UsedImplicitly]
        public static string FormatNumber(this double number)
        {
            if (number >= 1e30)
                return "It's time to stop playing";
            if (number >= 1e27)
                return (number / 1e15).ToString() + " octillions";
            if (number >= 1e24)
                return (number / 1e15).ToString() + " septillions";
            if (number >= 1e21)
                return (number / 1e15).ToString() + " sextillions";
            if (number >= 1e18)
                return (number / 1e15).ToString() + " quintillions";
            if (number >= 1e15)
                return (number / 1e15).ToString() + " quadrillions";
            if (number >= 1000000000000)
                return (number/1000000000000).ToString() + " trillions";
            if (number >= 1000000000)
                return (number/1000000000).ToString() + " billions";
            if (number >= 1000000)
                return (number/1000000).ToString() + " millions";
            return number.ToString();
            ;
        }
    }
}