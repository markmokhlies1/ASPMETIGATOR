
namespace M05.RouteConstraints.Constraints;

public class MonthRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if(!values.TryGetValue(routeKey, out var routeValue))
            return false;

        if(int.TryParse(routeValue?.ToString(), out int month))
           return month >=1 && month <=12;

        return false; 
    }
}