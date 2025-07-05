using System.Reflection;
using RentCarServer.Application.Behaviors;

namespace RentCarServer.Application.Services;
public sealed class PermissionService
{
    public List<string> GetAll()
    {
        var permissions = new HashSet<string>();

        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes();

        foreach (var type in types)
        {
            var permissinAttr = type.GetCustomAttribute<PermissionAttribute>();

            if (permissinAttr is not null && !string.IsNullOrEmpty(permissinAttr.Permission))
            {
                permissions.Add(permissinAttr.Permission);
            }
        }

        return permissions.ToList();
    }
}
