Unix Installers for Mono versions of WebsitePanel
====================================

Currently only WebsitePanel.Server runs on Mono, it's located in WebsitePanel.Server.Mono.

This directory contains the deployed file system in the folder ServerDeployed, and packages in the folders Debian and RPM.
The server uses xsp4 as server, as it needs to run as root, what is not straightforward on apache.
The files in ServerDeployed are directly derived from the mono-xsp4 files for running xsp4 as a server.
Currently they don't support https, and the current version of WebsitePanel.Server.Mono doesn't encrypt SOAP messages,
so currently things are not secure and should be used in production only behind a firewall. It should be very easy to add
support for https tough, as xsp4 supports it, I only don't know how to get the parameters into the shell script, I'm not much of
a unix shell programmer.