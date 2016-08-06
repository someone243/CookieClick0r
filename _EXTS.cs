using JetBrains.Annotations;
namespace CookieClicker
{
    public static class _EXTS
    {
        [UsedImplicitly]
        public static string FormatNumber(this double number)
        {
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