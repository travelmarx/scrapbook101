---
layout: default
---
# Code Discussion

## Categories

{{site.sn}} items are described by a key-value pairs at the top level of JSON such as **title**, **id**, and **category** as described in [item document][item]. Category values can be one of a fixed number of string values ("Books", "Events", etc.). Each category has a fixed number of associated fields as described in [category document][cat]. The category fields are tracked not at the top level of JSON, but at a block with fields nested one layer deep.

The advantages of the nested structure is that is makes the structure easier to visually read and allows for more targeted searching because of the controlled category field taxonomy. The disadvantage is that it introduces more complexity in code with the need for a `Category.cs` object that tracks the nested category fields and more complexity in the models (`CombinedModel.cs` as discussed below) in the MVC architecture.

An obvious questions is if there another way to handle categories?  Can the JSON structure be flattened? Consider an implementation of {{site.sn}} where the category document is not used, and **category** and **categoryFields** are not controlled by a schema but are allowed to be added or not. In this case, **category** values might be "Book", "Books", "BooksRead", etc., that is, values are not governed by a fixed set of choices. This is certainly easier implementation-wise with less complicated class structures in MVC, but its main disadvantage is that it becomes harder to have consistent search results. 

Another consideration in the current implementation is determining which data (fields) belong at the item-level (first-level) or as category fields (second-level). Here are some rough guidelines for determining what data fields belong at the category level and which belong at the item level.

* Category data fields are specific to one or more categories but are not general enough to be be useful for all categories and therefore should *not* be promoted to [item level][item]. Use the same field names when possible between categories level data fields. 

    - For example, **synopsis**, **type**, **location**, and **year** are used in several categories. This is useful because the object representation of the categories in `Categories.cs` can be more compact requiring less property definitions.

* Item data fields are common to all (or nearly all) categories. For example, the **rating** field is an item-level field, and even if it may not necessarily apply to say the People category. It still is better suited at the item-level than duplicated at the category-level. (It's not a problem to leave the **rating** field blank if it doesn't apply.)

## Models

There are three model files: 

* `Item.cs` is the object representing the [item document][item].

* `Category.cs` is the object representing the [category document][cat].

* `CombinedModel.cs` is needed because each ASP.NET MVC view (`.cshtml` page) needs to bind to one model, however in some views both the item and category models are needed. Therefore, the combined model represents a combination of the two models plus some helper classes for dealing with how categories are displayed and how assets are tracked.

In the `Item.cs` and `Category.cs` classes, note the use of the [Newtonsoft Json.NET][newton] JsonProperty annotations that allow the use of lowercase names for data fields in JSON (e.g., "assetPath") and camelcase for data fields in code (e.g., "AssetPath").

Finally, recall how our {{site.sn}} categories can contain data fields with the same name. For example, a book and film item both have a **synopsis** data field. Instead of defining multiple properties in code that have the same name "Synopsis", we instead define one property that can be used for any category that includes a **synopsis** field.  In `Category.cs` we define a `CategoryFields` class with all possible fields, and allow indexing by name (`this`) so that just the fields for a given category can be used.

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

Authentication is verifying the identify of a person or device (an account in general). {{site.sn}} does not have authentication, but it can be added. This is especially important when running live. For example, you can add code to allow users to sign in with [Google][auth-goog], [Facebook][auth-fb], [Microsoft][auth-msft] accounts as well as with other means. Even if your {{site.sn}} implementation is open, tracking who makes and edits entries is useful. This can be done by capturing the associated email and using that in the **updatedBy** field in the [item-document][item].

If the intent with your {{site.sn}} implementation is to allow only certain accounts access, then you need to additionally set up authorization, that is, associating privileges to specific accounts. In fact, you should never store personal information (be it with {{site.sn}} or any other means) without authentication and authorization in place. For an overview of security in ASP.NET Core, see [Introduction to authorization in ASP.NET Core][auth-core]. The approaches here could might be useful when running locally. When running as a web service such as in Microsoft Azure, see [Advanced usage of authentication and authorization in Azure App Service][auth-adv].

## Import Data

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

The create, delete, update, and edit pages (under the `Views\Item` folder) are currently minimally styled. They can be improved and rearranged as needed. For example in the edit page, the **description** field can be customized to the long or short descriptions and fields that can't or shouldn't be changed (like **id** and **type**) can be removed from the the form.  

In contrast, the {{site.sn}} main page (`Views\Item\Index.cshtml`) is styled with [Bootstrap][boot] to demonstrate a visual representation of the items in {{site.sn}}. The Bootstrap [cards][bootcard] structure is used that shows one asset image if it exists or else show a default image as specified in the `web.config` file.  

All the scripts for styling are injected in the `Views\Shared\__Layout.cshtml`. A more typical approach is to bundle
the scripts together and include them in the `App_Start\BundleConfig.cs`.

## Paging

Paging is not currently implemented in {{site.sn}}. Paging features can be added by modifying the search results in the `ItemController.cs` file where results are returned in the `SearchAsync` method.

[blog]: http://blog.travelmarx.com/2017/12/a-personal-information-management-system-introducing-scrapbook.html
[boot]: https://getbootstrap.com
[bootcard]: https://getbootstrap.com/docs/4.0/components/card/
[item]: /item-document
[cat]: /category-document
[auth-fb]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-facebook
[auth-goog]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-google
[auth-msft]: https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-provider-microsoft
[auth-adv]: https://docs.microsoft.com/en-us/azure/app-service/app-service-authentication-how-to
[auth-core]: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/introduction?view=aspnetcore-2.2
[migrate]: https://docs.microsoft.com/en-us/azure/cosmos-db/import-data
[newton]: https://www.newtonsoft.com/json
[migration]: https://docs.microsoft.com/en-us/azure/cosmos-db/import-data
[linq]: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/working-with-linq