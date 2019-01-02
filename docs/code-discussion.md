---
layout: default
---
# Code Discussion

## Categories

{{site.sn}} items are described by a name/value pairs such as **title**, **id**, and **category**. There are also objects that contain child name/value pairs. For example, **assets** is an object that contains name/value pairs with the names **name** and **size**. See [item document][item] for more information.

Category values (**category** value) can be one of a fixed number of string values ("Books", "Events", etc.). Each category has a fixed number of associated fields as described in [category document][cat]. The category fields (**categoryField**) object contains different name/value pairs that depend on the selected category.

The advantages of a nested structure (top level name/value pairs and nested name/value pairs defined in an object) is that it is easier to read and allows for more targeted searching because of the controlled category field taxonomy. The disadvantage of the nested structure is that it introduces more complexity in code because additional objects such as the `Category.cs` object are needed to track the nested structure. 

An obvious question is if there another way to handle categories?  Can the JSON structure be flattened? Consider an implementation of {{site.sn}} where the category document is not used, and **category** and **categoryFields** are not controlled by a schema but are allowed to be added or not. In this case, **category** values might be "Book", "Books", "BooksRead", etc., that is, values are not governed by a fixed set of choices. This is certainly easier implementation-wise with less complicated class structures in code, but it becomes harder to have reliable search results. 

Another consideration in choosing a JSON structure - choosing a schema - is determining which name/value pairs (fields) belong at the item-level (first-level) or as category fields (nested, second-level). Here are some rough guidelines for determining what fileds belong at the category level and which belong at the item level.

* Category fields are specific to one or more categories but are not general enough to be be useful for all categories and therefore should *not* be promoted to [item level][item]. Use the same field names when possible between category data fields. For example, **synopsis**, **type**, **location**, and **year** are used in several categories. This is useful because the object representation of the categories in `Categories.cs` can be more compact and requires less property definitions.

* Item data fields are common to all (or nearly all) categories. For example, the **rating** field applies to most categories in {{site.sn}} like "Books", "Films", "Museums", and "Performances" easily and to "People" less so, it still is better suited at the item-level than duplicated at the category-level. It is not a problem to leave the **rating** field blank if it doesn't apply.

## Models

There are three model files: 

* `Item.cs` is the object representing the [item document][item].

* `Category.cs` is the object representing the [category document][cat].

* `CombinedModel.cs` is needed because each MVC view (`.cshtml` page) needs to bind to one model, however in some views both the item and category models are needed. Therefore, the combined model represents a combination of the two models plus some helper classes for dealing with how categories are displayed and how assets are tracked.

In the `Item.cs` and `Category.cs` classes, note the use of the [Newtonsoft Json.NET][newton] JsonProperty annotations that allow the use of lowercase names for data fields in JSON (e.g., "assetPath") and camelcase for data fields in code (e.g., "AssetPath").

Finally, recall how our {{site.sn}} categories can contain name/value pairs (fields) with the same name. For example, a book and film item both have a **synopsis**  field. Instead of defining multiple properties in code that have the same name "Synopsis", we instead define one property that can be used for any category that includes a **synopsis** field.  In `Category.cs` we define a `CategoryFields` class with all possible category fields, and allow indexing by name (`this`) so that just the fields for a given category are included with a {{site.sn}} are used.

```C#
public class Category
{
    [JsonProperty(PropertyName = "categories")]
    public List<CategoryItem> Categories { get; set; }
}
public class CategoryItem
{
    [JsonProperty(PropertyName = "categoryFields")]
    public CategoryFields CategoryFields { get; set; }
}
public class CategoryFields
{
    [JsonIgnore]
    public object this[string propertyName]
    {
        get { return this.GetType().GetProperty(propertyName).GetValue(this); }
        set { this.GetType().GetProperty(propertyName).SetValue(this, value); }
    }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "synopsis")]
    public string Synopsis { get; set; }
}
```

## Security

*Authentication* is verifying the identify of a person or device (an account in general). {{site.sn}} does not have authentication implemented, but it can be added. This is especially important when running live. For example, you can add code to allow users to sign in with [Google][auth-goog], [Facebook][auth-fb], [Microsoft][auth-msft] accounts as well as by other means. Even if your {{site.sn}} implementation is open, tracking who makes and edits entries is useful. This can be done by capturing the associated email or user name and using that in the **updatedBy** field in the [item-document][item].

If the intent of your {{site.sn}} implementation is to allow only certain accounts access, then you need to additionally set up *Authorization*, that is, associating privileges to specific accounts. In fact, you should never store personal information (be it with {{site.sn}} or any other means) without authentication and authorization in place. For an overview of security in ASP.NET Core, see [Introduction to authorization in ASP.NET Core][auth-core]. The approaches discussed there could be useful when running locally. When running as a web service such as in Microsoft Azure, see [Advanced usage of authentication and authorization in Azure App Service][auth-adv].

## Import data

If you run {{site.sn}} locally and start to add entries and then decide to go-live, you can transfer your local data to your on-line data-store using the [Azure Cosmos DB Data Migration tool][migration].

## Searching

The search functionality implemented in {{site.sn}} allows searching titles for a string fragment. The code to do
this is in the `ItemController.cs` file's `SearchAsync` method, which uses [LINQ][linq]:

```C#
var items = await DocumentDBRepository<Item>.GetItemsAsync(
    item => item.Type == AppVariables.ItemDocumentType
    && item.Title.ToLower().Contains(searchString.ToLower()));
```
The included functionality is basic but can be expanded to include searching the **description**, **location**, and **dateAdded** fields. For example, to search both the **title** and the **description**, you could use

```C#
var items = await DocumentDBRepository<Item>.GetItemsAsync(
    item => item.Type == AppVariables.ItemDocumentType
    && (item.Title.ToLower().Contains(searchString.ToLower())
    || item.Description.ToLower().Contains(searchString.ToLower())));
```

## Styling

The create, delete, update, and edit pages (under the `Views\Item` folder) are currently minimally styled. They can be improved and rearranged as needed. For example in the edit page, the **description** field can be customized for long or short descriptions and fields that can't or shouldn't be changed (like **id** and **type**) can be removed from the form. 

In contrast, the {{site.sn}} main page (`Views\Item\Index.cshtml`) is styled with [Bootstrap][boot] to demonstrate a possible visual representation of the items in {{site.sn}}. The Bootstrap [cards][bootcard] structure is used which shows one asset image if it exists or else show a default image as specified in the configuration file (either `web.config` or `appsettings.json`).  

<ul class="nav nav-tabs" role="tablist">
  <li class="nav-item">
    <a class="nav-link active" href="#styling1" role="tab"
    data-toggle="tab">ASP.NET MVC</a>
  </li>
  <li class="nav-item">
    <a class="nav-link" href="#styling2" role="tab"
    data-toggle="tab">ASP.NET Core</a>
  </li>
</ul>

<div class="tab-content">
  <div role="tabpanel" class="tab-pane aspnetmvc active" id="styling1">
    <p class="single">
    All the scripts for styling are injected in <code>Views\Shared\__Layout.cshtml</code>. 
    A more practical approach is to bundle
    the scripts together and include them in the <code>App_Start\BundleConfig.cs</code>.
    </p>
  </div>
  <div role="tabpanel" class="tab-pane aspnetcore" id="styling2">
    <p class="single">
    All the scripts for styling are injected in <code>Views\Shared\__LayoutScrapbook.cshtml</code>. A more practical approach is to bundle the scripts together and include them using a tool such as 
    <a href="https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier">Bundle & Minifier</a> or <a href="https://www.nuget.org/packages/BuildBundlerMinifier/">BuildBundleMinifier</a>.
    </p>
  </div>
</div>

## Paging

Paging is not currently implemented in {{site.sn}}. Paging features can be added by modifying the search results in the `ItemController.cs` file where results are returned in the `SearchAsync` method.

[item]: item-document
[cat]: category-document
[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[boot]: https://getbootstrap.com
[bootcard]: https://getbootstrap.com/docs/4.0/components/card/
[auth-fb]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-facebook
[auth-goog]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-google
[auth-msft]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-microsoft
[auth-adv]: https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-how-to
[auth-core]: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-2.2
[migrate]: https://docs.microsoft.com/en-us/azure/cosmos-db/import-data
[newton]: https://www.newtonsoft.com/json
[migration]: https://docs.microsoft.com/en-us/azure/cosmos-db/import-data
[linq]: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/working-with-linq
[bundle1]: https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier
[bundle2]: https://www.nuget.org/packages/BuildBundlerMinifier/