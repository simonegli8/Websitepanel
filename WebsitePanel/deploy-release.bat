mode con:cols=140 lines=10000
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe" build.xml /target:Deploy /p:BuildConfiguration=Release /p:Version="2.1.0" /p:FileVersion="2.1.0.1" /p:VersionLabel="2.1.0.1" /v:n /fileLogger /m
