dotnet build -c release
Compress-Archive -Path build/x64/Release/net6.0-windows/* -DestinationPath ./build/mp4Utl_Release.zip