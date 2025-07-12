using System.Collections.Generic;

namespace Money.Domain;

public static class RmdTables
{
    // IRS Uniform Lifetime Table divisors (2023). Keyed by age.
    public static readonly Dictionary<int, decimal> UniformLifetimeDivisors = new()
    {
        {73, 26.5m},
        {74, 25.5m},
        {75, 24.6m},
        {76, 23.7m},
        {77, 22.9m},
        {78, 22.0m},
        {79, 21.1m},
        {80, 20.2m},
        {81, 19.4m},
        {82, 18.5m},
        {83, 17.7m},
        {84, 16.8m},
        {85, 16.0m},
        {86, 15.2m},
        {87, 14.4m},
        {88, 13.7m},
        {89, 12.9m},
        {90, 12.2m},
        {91, 11.5m},
        {92, 10.8m},
        {93, 10.1m},
        {94, 9.5m},
        {95, 8.9m},
        {96, 8.4m},
        {97, 7.8m},
        {98, 7.3m},
        {99, 6.8m},
        {100, 6.4m},
        {101, 6.0m},
        {102, 5.6m},
        {103, 5.2m},
        {104, 4.9m},
        {105, 4.6m},
        {106, 4.3m},
        {107, 4.1m},
        {108, 3.9m},
        {109, 3.7m},
        {110, 3.5m},
        {111, 3.4m},
        {112, 3.3m},
        {113, 3.1m},
        {114, 3.0m},
        {115, 2.9m},
        {116, 2.8m},
        {117, 2.7m},
        {118, 2.5m},
        {119, 2.3m},
        {120, 2.0m}
    };
}
