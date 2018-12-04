## Run locally

Webuild the Scrapbook 101 app on Winddows with the following tools.

* [Visual Studio Community][vsdown]  
* [Azure Cosmos DB Local Emulator][emul]

After starting the local emulator, there is an option to download code samples you can start from.

![Alt text](images/where-to-get-samples.jpg "Getting samples in Cosmos Local Emulator")

## Run live

Running locally is great to try out your ideas at no cost and little risk. However, you can access your scrapbook outside the context of your local environment. To take the next step and access your Scrapbook 101 more broadly on the web, you need to go live. by publishing your site live with the following services:

- [Azure Cosmos DB Service][cosmos]
  - Works the same as local and you can copy any documents created locally to the live service.
 
* [Azure Application Service][azapp]
  - Publish directly from Visual Studio to the Azure Application service.

Note that you will eventually incur charges from Azure, but be sure to take advantage of any limited, free use offers. 

[vsdown]: https://visualstudio.microsoft.com/downloads/
[emul]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
[cosmos]: https://azure.microsoft.com/en-us/services/cosmos-db/
[azapp]: https://azure.microsoft.com/en-us/services/app-service/
