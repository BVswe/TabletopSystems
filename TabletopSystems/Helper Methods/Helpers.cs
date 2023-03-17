using System.Text.RegularExpressions;

namespace TabletopSystems.Helper_Methods;

public static class Helpers
{
    public static bool validSystemNickname(string s)
    {
        if (!Regex.Match(s, "^[A-Za-z _0-9]+$").Success)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
