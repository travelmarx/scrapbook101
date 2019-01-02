---
layout: default
---
# Handling Assets

## Overview

One obvious question is if assets (images, documents, etc.) are needed? They add a level of complexity to the code that is significant. We would argue that the assets, especially images, are important because visualizing {{site.sn}} items is simpler and more intuitive. Agreeing that assets are important, the question become how to upload assets and where to store them.

Running locally, assets can be stored in a folder on the local file system, e.g. `C:\Assets`. In the version of {{site.sn}} we use the `Assets` folder (.NET Core version) or `App_data` (.Framework version) that are part of the Visual Studio solution to make documentation and demonstation easier. However, this approach isn't practical or recommended. 

> The {{site.sn}} application allows basic handling of assets through browser, including adding assets in the
> create operation using the [HTML File Upload Object][htmlfile] and deleting assets in the edit operation. 
> In practice, you will need more renaming and file manipulation code.

You could use any document storage servies or blob storage service to store assets. In this case, you would add connection keys to the `appsettings.json` (.NET Core version) or `web.config` (.NET Framework) and use them in the `ItemController.cs` code when creating or editing items. In the Scrapbook version described in our [blog post][blog], assets can be uploaded locally with the [HTML File Upload Object][htmlfile] or from OneDrive using the [OneDrive REST API][onedriverest]. Regardless of where the assets originate, they are stored permanently in [Microsoft Azure Blob Storage][blob].

Assets that are stored in an online service could be used when running {{site.sn}} both locally or live. Finally, note that assets stored in an online service should not be "open", keys or access to the assets should be an authenticated and authorized account.

## Asset organization

In the {{site.sn}} code, the naming convention show in the `Assets` folder (.NET Core version) or `App_data` (.NET Framework version) is only for demonstration and is not scaleable to a large number of items.

Here's an example of how you might organize your assets in a more scaleable way regardless of whether you do it locally or online. The description below is in fact how we do it as described in the [blog post][blog].

```
\2018\2018-10-01\Folder.png
\2018\2018-10-01\dinner-menu.png
\2018\2018-10-28\Folder.png
\2018\2018-10-28\alternate-movie-poster.png
\2018\2018-10-28\info-about-the-documentary.pdf
\2018\2018-10-28-01\Folder.png
\2018\2018-10-28-01\back-cover-of-book.png
...
```

**How should asset path names be structured?**

We've tried many different approaches and in the end we stuck with using dates (YYYY-MM-DD) because we found it to be intutive as well as having the benefit that when looking at the assets folder structure in any file explorer, the structure makes sense at least chronologically. In consumer-oriented storage services like Microsoft OneDrive or Google Drive, or object storage services like Microsoft Azure Blob Storage, Google Cloud Storage, or Amazon S3,  the asset hierarchy can be searched easily. Consumer-oriented services offer file/folder paradigms for searching. Object storage services can be searched with third party tools like [CloudBerry Explorer][cloudberry].

A second reason for using dates as part of the asset path is that the date an asset is entered can be used to automatically assign the **assetPath** value in the [item document][item].

**What name to use for assets?**

Naming of assets in {{site.sn}} is not currently fixed to any particular convention. Assets can be any legal file name. The first image asset found at the specified **assetPath** is used to represent the item on main page (`Item\Index.cshtml`). 

In our personalized version of Scrapbook and as hinted at above, we use `Folder.png` to represent the main image visual representation of the item. Another approach would be to add another asset JSON data field called **role** for each asset with values such as "display", "main", or "asset" for example.

**How to deal with repeated items with the same date?**

Choosing the date (YYYY-MM-DD) as part of the asset path name leads to the problem of dealing with multiple {{site.sn}} items on the same date. We choose to resolve this issue by appending numbers (01, 02, etc.) at the end of the date so as to still retain the date format but denote multiple entries. Therefore, in the example above there are two {{site.sn}} items for `2018-10-28`.

[item]: item-document
[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[blob]: https://azure.microsoft.com/en-us/services/storage/blobs/
[htmlfile]: https://www.w3schools.com/jsref/dom_obj_fileupload.asp
[onedriverest]: https://docs.microsoft.com/it-it/onedrive/developer/rest-api/?view=odsp-graph-online
[cloudberry]: https://www.cloudberrylab.com/explorer.aspx