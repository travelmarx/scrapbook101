namespace Scrapbook101.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Models;

    public class ItemController : Controller
    {
        // GET: Create the search page 
        [ActionName("Search")]
        public async Task<ActionResult> SearchAsync(string searchString)
        {
            searchString = string.IsNullOrEmpty(searchString) ? "" : searchString;
            var items = await DocumentDBRepository<Item>.GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType
                && item.Title.ToLower().Contains(searchString.ToLower()));
            ViewBag.imagePath = BuildPathList(items);
            return View("Index", items);
        }

        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(item => item.Type == AppVariables.ItemDocumentType);
            ViewBag.imagePath = BuildPathList(items);
            return View(items);
        }

        private List<string> BuildPathList(IEnumerable<Item> items)
        {
            List<string> imagePath = new List<string>();
            foreach (var item in items)
            {
                string imageToDisplay = AppVariables.NoAssetImage;
                if (!System.String.IsNullOrEmpty(item.AssetPath))
                {

                    if (item.Assets != null && item.Assets.Count != 0)
                    {
                        // Show first image found if one exists
                        foreach (var asset in item.Assets)
                        {
                            if (MimeMapping.GetMimeMapping(asset.Name).StartsWith("image/"))
                            {
                                imageToDisplay = $"{item.AssetPath}" + "/" + asset.Name;
                                break;
                            }
                        }
                    }
                    imagePath.Add("/" + imageToDisplay);
                }
                else
                {
                    imagePath.Add("/" + imageToDisplay);
                }
            }
            return imagePath;
        }

#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            CombinedModel combinedModel = new CombinedModel
            {
                Item = new Item
                {
                    Id = "value auto-generated on insert",
                    Type = AppVariables.ItemDocumentType,
                },
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList
            };
            return View(combinedModel);
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CombinedModel combinedModel)
        {
            // The items not bound are specifed
            combinedModel.Item.DateAdded = System.DateTime.UtcNow;
            combinedModel.Item.DateUpdated = System.DateTime.UtcNow;
            combinedModel.Item.UpdatedBy = "travelmarx"; // If user is signed in, use name or email.
            combinedModel.Item.GeoLocation = null;
            combinedModel.Item.Assets = null;
            combinedModel.Item.Id = null; // Set to null so on insert CosmosDB sets GUID.

            // Get geocode from Bing if applicable - see web.config
            if (AppVariables.BingMapKey.Length > 0)
            {
                double[] coord = await GeoCodeHelper.GetGeocode(combinedModel.Item.Location);
                if (coord[0] != 0)
                {
                    combinedModel.Item.GeoLocation = new Microsoft.Azure.Documents.Spatial.Point(coord[1], coord[0]);
                }
            }

            // Check for local upoads - the LocalHttpPostedFileBaseList object contains the streams
            if (combinedModel.LocalHttpPostedFileList?[0] != null && combinedModel.LocalHttpPostedFileList.Count() > 0)
            {
                int indx = 0;
                foreach (HttpPostedFileBase file in combinedModel.LocalHttpPostedFileList)
                {
                    FileItem currFile = combinedModel.LocalUploadFileList[indx];

                    if (combinedModel.Item.Assets == null || combinedModel.Item.Assets.Count() == 0)
                    {
                        combinedModel.Item.Assets = new List<AssetItem>();
                    }

                    combinedModel.Item.Assets.Add(
                        new AssetItem
                        {
                            Name = currFile.Name,
                            Size = currFile.Size
                        });
                    indx++;
                }
                // Add sorted items
                combinedModel.Item.Assets = combinedModel.Item.Assets.OrderBy(a => a.Name).ToList();
            }

            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(combinedModel.Item);
                return RedirectToAction("Index");
            }

            return View(combinedModel);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CombinedModel combinedModel)
        {
            // Deal with any files checked for removal.
            List<string> filesToRemove = Request["FilesToRemove"]?.Split(',').ToList();
            if (Request["FilesToRemove"] != null)
            {
                foreach (var file in filesToRemove)
                {
                    combinedModel.Item.Assets.Remove(new AssetItem() { Name = file, Size = "n/a" });
                }
            }
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(combinedModel.Item.Id, combinedModel.Item);
                return RedirectToAction("Index");
            }

            return View(combinedModel);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            CombinedModel combinedModel = new CombinedModel
            {
                Item = item,
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList,
                CategoryFieldMappingList = AppVariables.CategoryFieldMappingList
            };


            return View(combinedModel);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id")] string id)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id)
        {
            Item item = await DocumentDBRepository<Item>.GetItemAsync(id);
            CombinedModel combinedModel = new CombinedModel
            {
                Item = item,
                CategoryItemsForDisplay = AppVariables.CategoryDisplayList,
                CategoryFieldMappingList = AppVariables.CategoryFieldMappingList
            };
            return View(combinedModel);
        }
    }
}