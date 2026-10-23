namespace AcademicHybridApp.Services;

public static class ApiConfig
{
#if ANDROID
    // Emulator Android mengakses localhost komputer host lewat 10.0.2.2.
    public const string BaseUrl = "http://10.0.2.2:5294/";
#else
    public const string BaseUrl = "http://localhost:5294/";
#endif
}
