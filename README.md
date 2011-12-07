This project is an effort to build a multi-node demo to prove out a working architecture for a REALLY BIG company.  :)

Notes:
1. Make sure IIS service is enabled in your windows 7 setup (Programs and Features)
 - probably should have a local sql 2008 MSSQLSERVER instance with SQL Agent Service running
   locally, unless you want to use a remote SQL server.
2. Install the inetmgr_amd64.msi to get the IIS 7 manager tool.
3. Install Appfabric msi
- make sure select cache server and client options as well as the management options.

SEAD-GBYD1R1-X2 (Has AppFabricCache - which has the cache cluster configuration)

Note:
During Appfabric setup you will be asked for monitoring and persistence providers,
I setup an "AppFabric" database on my local SQL 2008 instance.

During Cache setup - setup as a cluster host (if this is first instance), you can
let the setup create the SQL database for the cache DB.

Libraries:

To use caching - the client dlls are in C:\Windows\System32\Appfabric
*Microsoft.ApplicationServer.Caching.Core.dll
*Microsoft.ApplicationServer.Caching.Client.dll
Microsoft.WindowsFabric.Common.dll
Microsoft.WindowsFabric.Data.Common.dll

*These are the public APIs

Note: Product Version of Microsoft.ApplicationServer.Caching.Client.dll
 must be identical on all machines (hosts and clients)


When configuring cache for notifications:

Cache configuration must have notifications enabled
Get-CacheConfig - to show
Set-CacheConfig - to change
(BTW: I ended up having to shut down both hosts Stop-CachHost - then restart the cluster
Start-CacheCluster)

I got an error message when running program - which cleared up after restarting everything:
ERRCA0017, sub ES0001 - "there was a temporary problem..." not much info there, and web search
revealed very little. Looks like the cluster got confused about it's state.












