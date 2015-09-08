Import-Module WebAdministration

cd ..
$path = pwd
cd Tools

New-Website -Name "WebsitePanel Server" -Port 9003 -PhysicalPath "$path\Sources\WebsitePanel.Linux.Server" -ErrorAction SilentlyContinue
	
	
	