---
layout: default
---
# Get Started

To build the {{site.sn}} app on Windows use the following:

* [Visual Studio Community][vsdown]  
* [Azure Cosmos DB Local Emulator][emul]
* [Bing Maps Basic Key][bingmap] (optional)

Running {{site.sn}} locally, you can try out your ideas at no cost or risk. After installing the tools described above, first confirm that you can run Cosmos DB Local Emulator. You should see a screen like the following image. 

![Alt text](images/where-to-get-samples.jpg "Getting samples in Cosmos DB Local Emulator")

## Run local

Next, get the {{site.sn}} code and open it as a solution in Visual Studio.  Customize the code as
follows in the `web.config` file.

1. Enter the correct value for **authKey**, which you can get from the local emulator home page as show above.
2. Enter a Bing Maps Key for **bingMapKey** if you have one; if blank, then geocoding is skipped.
3. Set the value for **addTestAssets** to `true` to write test assets or `false` not to write them.

There are other `web.config` you can change, but the the three above are the enough to get started.

At this point you should be able to run the solution (Visual Studio F5) and view {{site.sn}} in a browser, e.g. https://localhost:port#/.

What happened on startup:

* A database named **{{site.sn}}** and collection named **Items** was created in the document store. 
* A [category document][cat] was created and stored in the document store. The category information was read from the file `App_data/categories-document.json`.
* If **addTestAssets** was set to `true` test {{site.sn}} items were added to the document store as well. They were read from the file `App_Data/test-documents.json`.

## Create an item

At this point, you can start working with {{site.sn}} items with CRUD (create, read, update, delete) operations. The home page is https://localhost:port#/ or https://localhost:port#/Item/Index.

To create a new {{site.sn}} item.

1. On the home page, select **Create**.
2. Choose a category in the **Category** dropdown.
3. Fill in the **Title** field.
4. Select **Save** or continue to fill in fields.

## Run live

After running {{site.sn}} locally, you can take optional next step and run {{site.sn}} as a web service. To do this you need to go live, which means publishing your site live with the following services:

- [Azure Cosmos DB Service][cosmos] - This works the same as local emulator and you can copy any documents created locally to the live service. 
 
* [Azure Application Service][azapp] - You can publish your site directly from Visual Studio to the Azure Application service.

Using Azure Cosmos DB Service you will eventually incur charges, but be sure to take advantage of any limited, free use offers. 

There are a couple of other considerations when going live. In the least, you should consider:

* Authentication and authorization.
* Transfering any documents from local emulator to live service.
* How to deal with asset storage.

These and other topics are discussed in [Code Discussion][code-discussion].

[vsdown]: https://visualstudio.microsoft.com/downloads/
[emul]: https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator
[cosmos]: https://azure.microsoft.com/en-us/services/cosmos-db/
[azapp]: https://azure.microsoft.com/en-us/services/app-service/
[bingmap]: https://www.microsoft.com/en-us/maps/create-a-bing-maps-key
[item]: /item-document
[cat]: /category-document
[azblob]: https://azure.microsoft.com/en-us/services/storage/blobs/
[code-discussion]: /code-discussion