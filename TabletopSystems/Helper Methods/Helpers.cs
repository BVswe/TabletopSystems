using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;

namespace TabletopSystems.Helper_Methods;

public static class Helpers
{
    public static void AddGenericFactory<TClass>(this IServiceCollection services)
        where TClass : class
    {
        services.AddTransient<TClass>();
        services.AddSingleton<Func<TClass>>(x => () => x.GetService<TClass>());
        services.AddSingleton<IAbstractFactory<TClass>, AbstractFactory<TClass>>();
    }
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
