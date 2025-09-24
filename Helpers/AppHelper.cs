namespace Fong.Helpers;

public static class AppHelper {
    public static bool IsInDocker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

    public static string GetDataPath() {
        var mountedPath = Environment.GetEnvironmentVariable("DATA_PATH");

        if (string.IsNullOrEmpty(mountedPath)) {
            mountedPath = AppContext.BaseDirectory;
        }

        return mountedPath;
    }
}