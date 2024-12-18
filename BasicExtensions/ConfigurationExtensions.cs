using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace BasicExtensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsRunningUnderIde() => Debugger.IsAttached;

        public static string GetAppPath() => Directory.GetCurrentDirectory();

        public static string GetMethodName(this MethodBase currentMethod) => currentMethod.Name;

        public static string GetMethodFullName(this MethodBase currentMethod) => currentMethod.DeclaringType.FullName + "." + currentMethod.Name;
    }
}