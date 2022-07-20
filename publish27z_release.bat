dotnet publish -c Release -r win-x64 --self-contained
7z a build/mp4Utl_Release.7z build/x64/Release/*/win-x64/publish/*