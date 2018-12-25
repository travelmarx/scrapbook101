using Scrapbook101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scrapbook101.Controllers
{
    public class CategoryController : Controller
    {
        // GET for partial view on create page to get category fields for a chosen category
        [HttpGet]
        [ActionName("GetCategoryFields")]
        public string GetCategoryFieldsAsync(string category)
        {
            System.Text.StringBuilder buildString = new System.Text.StringBuilder();
            var categories = AppVariables.CategoryFieldMappingList;
            var foundCategory = categories.First(x => string.Compare(x.Name, category, true) == 0) as CategoryFieldMapping;

            foreach (string field in foundCategory.ActiveFields)
            {
                var builder1 = new TagBuilder("label");
                builder1.MergeAttribute("for", "Item_CategoryFields_" + field + "_");
                builder1.MergeAttribute("style", "color: blue");
                builder1.SetInnerText(field);
                var builder2 = new TagBuilder("input");
                builder2.MergeAttribute("name", "Item.CategoryFields[" + field + "]");
                builder2.MergeAttribute("id", "Item_CategoryFields_" + field + "_");
                builder2.MergeAttribute("type", "text");
                builder2.MergeAttribute("value", "");
                builder2.MergeAttribute("class", "form-control");
                buildString.Append(builder1.ToString() + builder2.ToString() + "<br/>");
            }
            return buildString.ToString();
        }

    }
}