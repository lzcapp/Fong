using System.Security.Cryptography;
using System.Text;

namespace Fong.Helpers;

public static class AppHelper {
    public static bool IsInDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

    public static string GetDataPath() {
        /*
        var mountedPath = Environment.GetEnvironmentVariable("DATA_PATH");

        if (string.IsNullOrEmpty(mountedPath)) {
            mountedPath = AppContext.BaseDirectory;
        }
        */
        return AppContext.BaseDirectory;
    }
    
    public static long GenerateHash(string str = "")
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        var hash = SHA256.HashData(bytes);
        return BitConverter.ToInt64(hash, 0);
    }
}