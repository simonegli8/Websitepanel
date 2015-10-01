SET WSDL="C:\Program Files (x86)\Microsoft WSE\v3.0\Tools\WseWsdl3.exe"
SET WSE_CLEAN=..\Tools\WseClean.exe
SET SERVER_URL=http://localhost:8080

REM %WSDL% %SERVER_URL%/AutoDiscovery.asmx /out:.\WebsitePanel.Linux.Server.Client\AutoDiscoveryProxy.cs /namespace:WebsitePanel.AutoDiscovery /type:webClient /fields
REM %WSE_CLEAN% .\WebsitePanel.Linux.Server.Client\AutoDiscoveryProxy.cs

%WSDL% %SERVER_URL%/OperationSystem.asmx /out:.\WebsitePanel.Linux.Server.Client\OperationSystemProxy.cs /namespace:WebsitePanel.Providers.OperationSystem /type:webClient /fields
%WSE_CLEAN% .\WebsitePanel.Linux.Server.Client\OperationSystemProxy.cs

