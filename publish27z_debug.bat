dotnet publish -c Debug -r win-x64 --self-contained
7z a build/mp4Utl_Debug.7z build/x64/Debug/*/win-x64/publish/*