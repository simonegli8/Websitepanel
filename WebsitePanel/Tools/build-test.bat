mode con:cols=140 lines=10000
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe" ..\test.xml /target:BuildTest /property:BuildConfiguration=Debug /p:Version="2.1.0" /p:FileVersion="2.1.0.1" /p:VersionLabel="2.1.0.1" /fileLogger /m
Pause