using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz.Xml.JobSchedulingData20;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Telerik.Web.Mvc.UI;
using AARTWeb.Models;

namespace AARTWeb.Controllers
{
    public class ImageBrowserController : Controller
    {
        private const string contentFolderRoot = "~/Themes/Upload-Images/ImageBrowser";
        private const string prettyName = "Images/";
        private static readonly string[] foldersToCopy = new[] { "~/Themes/Upload-Images/ImageBrowser/Read" };
        private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

        private const int ThumbnailHeight = 80;
        private const int ThumbnailWidth = 80;

        private readonly Models.DirectoryBrowser directoryBrowser;
        private readonly ContentInitializer contentInitializer;
        private readonly Models.ThumbnailCreator thumbnailCreator;

        public ImageBrowserController()
        {
            directoryBrowser = new Models.DirectoryBrowser();
            contentInitializer = new ContentInitializer(contentFolderRoot, foldersToCopy, prettyName);
            thumbnailCreator = new Models.ThumbnailCreator();
        }

        public string ContentPath
        {
            get
            {
                return contentInitializer.CreateUserFolder(Server);
            }
        }

        private string ToAbsolute(string virtualPath)
        {
            return VirtualPathUtility.ToAbsolute(virtualPath);
        }

        private string CombinePaths(string basePath, string relativePath)
        {
            return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
        }

        public virtual bool AuthorizeRead(string path)
        {
            return CanAccess(path);
        }

        protected virtual bool CanAccess(string path)
        {
            return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return ToAbsolute(ContentPath);
            }

            return CombinePaths(ToAbsolute(ContentPath), path);
        }

        public virtual JsonResult Read(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeRead(path))
            {
                try
                {
                    directoryBrowser.Server = Server;

                    var result = directoryBrowser
                        .GetContent(path, DefaultFilter)
                        .Select(f => new
                        {
                            name = f.Name,
                            type = f.Type == EntryType.File ? "f" : "d",
                            size = f.Size
                        });

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (DirectoryNotFoundException)
                {
                    throw new HttpException(404, "File Not Found");
                }
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeThumbnail(string path)
        {
            return CanAccess(path);
        }

        [OutputCache(Duration = 3600, VaryByParam = "path")]
        public virtual ActionResult Thumbnail(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeThumbnail(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    Response.AddFileDependency(physicalPath);

                    return CreateThumbnail(physicalPath);
                }
                else
                {
                    throw new HttpException(404, "File Not Found");
                }
            }
            else
            {
                throw new HttpException(403, "Forbidden");
            }
        }

        private FileContentResult CreateThumbnail(string physicalPath)
        {
            using (var fileStream = System.IO.File.OpenRead(physicalPath))
            {
                var desiredSize = new ImageSize
                {
                    Width = ThumbnailWidth,
                    Height = ThumbnailHeight
                };

                const string contentType = "image/png";

                return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Destroy(string path, string name, string type)
        {
            path = NormalizePath(path);

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
            {
                path = CombinePaths(path, name);
                if (type.ToLowerInvariant() == "f")
                {
                    DeleteFile(path);
                }
                else
                {
                    DeleteDirectory(path);
                }

                return Json(new object[0]);
            }
            throw new HttpException(404, "File Not Found");
        }

        public virtual bool AuthorizeDeleteFile(string path)
        {
            return CanAccess(path);
        }

        public virtual bool AuthorizeDeleteDirectory(string path)
        {
            return CanAccess(path);
        }

        protected virtual void DeleteFile(string path)
        {
            if (!AuthorizeDeleteFile(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }

        protected virtual void DeleteDirectory(string path)
        {
            if (!AuthorizeDeleteDirectory(path))
            {
                throw new HttpException(403, "Forbidden");
            }

            var physicalPath = Server.MapPath(path);

            if (Directory.Exists(physicalPath))
            {
                Directory.Delete(physicalPath, true);
            }
        }

        public virtual bool AuthorizeCreateDirectory(string path, string name)
        {
            return CanAccess(path);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Create(string path, FileBrowserEntry entry)
        {
            path = NormalizePath(path);
            var name = entry.Name;

            if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
            {
                var physicalPath = Path.Combine(Server.MapPath(path), name);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                return Json(new
                {
                    name = entry.Name,
                    type = "d",
                    size = entry.Size
                });
            }

            throw new HttpException(403, "Forbidden");
        }


        public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
        {
            return CanAccess(path) && IsValidFile(file.FileName);
        }

        private bool IsValidFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var allowedExtensions = DefaultFilter.Split(',');

            return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Upload(string path, HttpPostedFileBase file)
        {
            path = NormalizePath(path);
            var fileName = Path.GetFileName(file.FileName);

            if (AuthorizeUpload(path, file))
            {
                file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

                return Json(new
                {
                    size = file.ContentLength,
                    name = fileName,
                    type = "f"
                }, "text/plain");
            }

            throw new HttpException(403, "Forbidden");
        }

        [OutputCache(Duration = 360, VaryByParam = "path")]
        public ActionResult Image(string path)
        {
            path = NormalizePath(path);

            if (AuthorizeImage(path))
            {
                var physicalPath = Server.MapPath(path);

                if (System.IO.File.Exists(physicalPath))
                {
                    const string contentType = "image/png";
                    return File(System.IO.File.OpenRead(physicalPath), contentType);
                }
            }

            throw new HttpException(403, "Forbidden");
        }

        public virtual bool AuthorizeImage(string path)
        {
            return CanAccess(path) && IsValidFile(Path.GetExtension(path));
        }
    }
    //private const string contentFolderRoot = "~/";
    //private const string prettyName = "~/Themes/Upload-Images/ImageBrowser/Read";
    //private const string DefaultFilter = "*.png,*.gif,*.jpg,*.jpeg";

    //private const int ThumbnailHeight = 80;
    //private const int ThumbnailWidth = 80;

    //private readonly DirectoryBrowser directoryBrowser;
    //private readonly ThumbnailCreator thumbnailCreator;

    //public ImageBrowserController()
    //{
    //    directoryBrowser = new DirectoryBrowser();
    //    thumbnailCreator = new ThumbnailCreator(new FitImageResizer());
    //}

    //public string ContentPath
    //{
    //    get
    //    {
    //        return Path.Combine(contentFolderRoot, prettyName);
    //    }
    //}

    //private string ToAbsolute(string virtualPath)
    //{
    //    return VirtualPathUtility.ToAbsolute(virtualPath);
    //}

    //private string CombinePaths(string basePath, string relativePath)
    //{
    //    return VirtualPathUtility.Combine(VirtualPathUtility.AppendTrailingSlash(basePath), relativePath);
    //}

    //public virtual bool AuthorizeRead(string path)
    //{
    //    return CanAccess(path);
    //}

    //protected virtual bool CanAccess(string path)
    //{
    //    return path.StartsWith(ToAbsolute(ContentPath), StringComparison.OrdinalIgnoreCase);
    //}

    //private string NormalizePath(string path)
    //{
    //    if (string.IsNullOrEmpty(path))
    //    {
    //        return ToAbsolute(ContentPath);
    //    }

    //    return CombinePaths(ToAbsolute(ContentPath), path);
    //}

    //public virtual JsonResult Read(string path)
    //{
    //    path = NormalizePath(@"D:\AART\Azure download\AARTWeb\AARTWeb\Themes\Upload-Images\ImageBrowser\Read");

    //    if (AuthorizeRead(path))
    //    {
    //        try
    //        {
    //            directoryBrowser.Server = Server;

    //            var result = directoryBrowser.GetFiles
    //                (path, DefaultFilter)
    //                .Select(f => new
    //                {
    //                    name = f.Name,
    //                    //type = f.GetType. == entryType.Equals ? "f" : "d",
    //                    size = f.Size
    //                });

    //            return Json(result, JsonRequestBehavior.AllowGet);
    //        }
    //        catch (DirectoryNotFoundException)
    //        {
    //            throw new HttpException(404, "File Not Found");
    //        }
    //    }

    //    throw new HttpException(403, "Forbidden");
    //}


    //public virtual bool AuthorizeThumbnail(string path)
    //{
    //    return CanAccess(path);
    //}

    //[OutputCache(Duration = 3600, VaryByParam = "path")]
    //public virtual ActionResult Thumbnail(string path)
    //{
    //    path = NormalizePath(@"D:/AART/Azure download/AARTWeb/AARTWeb/Themes/Upload-Images/ImageBrowser/Read/a100.png");

    //    if (AuthorizeThumbnail(path))
    //    {
    //        var physicalPath = Server.MapPath(path);

    //        if (System.IO.File.Exists(physicalPath))
    //        {
    //            Response.AddFileDependency(physicalPath);

    //            return CreateThumbnail(physicalPath);
    //        }
    //        else
    //        {
    //            throw new HttpException(404, "File Not Found");
    //        }
    //    }
    //    else
    //    {
    //        throw new HttpException(403, "Forbidden");
    //    }
    //}

    //private FileContentResult CreateThumbnail(string physicalPath)
    //{
    //    using (var fileStream = System.IO.File.OpenRead(physicalPath))
    //    {
    //        var desiredSize = new ImageSize
    //        {
    //            Width = ThumbnailWidth,
    //            Height = ThumbnailHeight
    //        };

    //        const string contentType = "image/png";

    //        return File(thumbnailCreator.Create(fileStream, desiredSize, contentType), contentType);
    //    }
    //}

    //[AcceptVerbs(HttpVerbs.Post)]
    //public virtual ActionResult Destroy(string path, string name, string type)
    //{
    //    path = NormalizePath(path);

    //    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(type))
    //    {
    //        path = CombinePaths(path, name);
    //        if (type.ToLowerInvariant() == "f")
    //        {
    //            DeleteFile(path);
    //        }
    //        else
    //        {
    //            DeleteDirectory(path);
    //        }

    //        return Json(null);
    //    }
    //    throw new HttpException(404, "File Not Found");
    //}

    //public virtual bool AuthorizeDeleteFile(string path)
    //{
    //    return CanAccess(path);
    //}

    //public virtual bool AuthorizeDeleteDirectory(string path)
    //{
    //    return CanAccess(path);
    //}

    //protected virtual void DeleteFile(string path)
    //{
    //    if (!AuthorizeDeleteFile(path))
    //    {
    //        throw new HttpException(403, "Forbidden");
    //    }

    //    var physicalPath = Server.MapPath(path);

    //    if (System.IO.File.Exists(physicalPath))
    //    {
    //        System.IO.File.Delete(physicalPath);
    //    }
    //}

    //protected virtual void DeleteDirectory(string path)
    //{
    //    if (!AuthorizeDeleteDirectory(path))
    //    {
    //        throw new HttpException(403, "Forbidden");
    //    }

    //    var physicalPath = Server.MapPath(path);

    //    if (Directory.Exists(physicalPath))
    //    {
    //        Directory.Delete(physicalPath, true);
    //    }
    //}

    //public virtual bool AuthorizeCreateDirectory(string path, string name)
    //{
    //    return CanAccess(path);
    //}

    ////[AcceptVerbs(HttpVerbs.Post)]
    ////public virtual ActionResult Create(string path, FileBrowserEntry entry)
    ////{
    ////    path = NormalizePath(path);
    ////    var name = entry.Name;

    ////    if (!string.IsNullOrEmpty(name) && AuthorizeCreateDirectory(path, name))
    ////    {
    ////        var physicalPath = Path.Combine(Server.MapPath(path), name);

    ////        if (!Directory.Exists(physicalPath))
    ////        {
    ////            Directory.CreateDirectory(physicalPath);
    ////        }

    ////        return Json(null);
    ////    }

    ////    throw new HttpException(403, "Forbidden");
    ////}


    //public virtual bool AuthorizeUpload(string path, HttpPostedFileBase file)
    //{
    //    return CanAccess(path) && IsValidFile(file.FileName);
    //}

    //private bool IsValidFile(string fileName)
    //{
    //    var extension = Path.GetExtension(fileName);
    //    var allowedExtensions = DefaultFilter.Split(',');

    //    return allowedExtensions.Any(e => e.EndsWith(extension, StringComparison.InvariantCultureIgnoreCase));
    //}

    //[AcceptVerbs(HttpVerbs.Post)]
    //public virtual ActionResult Upload(string path, HttpPostedFileBase file)
    //{
    //    path = NormalizePath(path);
    //    var fileName = Path.GetFileName(file.FileName);

    //    if (AuthorizeUpload(path, file))
    //    {
    //        file.SaveAs(Path.Combine(Server.MapPath(path), fileName));

    //        return Json(new
    //        {
    //            size = file.ContentLength,
    //            name = fileName,
    //            type = "f"
    //        }, "text/plain");
    //    }

    //    throw new HttpException(403, "Forbidden");
    //}

    //[OutputCache(Duration = 360, VaryByParam = "path")]
    //public ActionResult Image(string path)
    //{
    //    path = NormalizePath(path);

    //    if (AuthorizeImage(path))
    //    {
    //        var physicalPath = Server.MapPath(path);

    //        if (System.IO.File.Exists(physicalPath))
    //        {
    //            const string contentType = "image/png";
    //            return File(System.IO.File.OpenRead(physicalPath), contentType);
    //        }
    //    }

    //    throw new HttpException(403, "Forbidden");
    //}

    //public virtual bool AuthorizeImage(string path)
    //{
    //    return CanAccess(path) && IsValidFile(Path.GetExtension(path));
    //}

    //private const int ThumbnailHeight = 80;
    //private const int ThumbnailWidth = 80;

    ////public ActionResult Image(string path)
    ////{
    ////    var files = new FilesRepository();
    ////    var image = files.ImageByPath(path);
    ////    if (image != null)
    ////    {
    ////        const string contentType = "image/png";
    ////        return File(image.Image1, contentType);
    ////    }
    ////    throw new HttpException(404, "File Not Found");
    ////}

    ////public ActionResult Create(string path, ImageBrowserEntry entry)
    ////{
    ////    var files = new FilesRepository();
    ////    var folder = files.GetFolderByPath(path);
    ////    if (folder != null)
    ////    {
    ////        files.CreateDirectory(folder, entry.Name);
    ////        return Content("");
    ////    }
    ////    throw new HttpException(403, "Forbidden");
    ////}

    ////public ActionResult Destroy(string path, ImageBrowserEntry entry)
    ////{
    ////    var files = new FilesRepository();
    ////    if (entry.EntryType == ImageBrowserEntryType.File)
    ////    {
    ////        var image = files.ImageByPath(Path.Combine(path, entry.Name));
    ////        if (image != null)
    ////        {
    ////            files.Delete(image);
    ////            return Content("");
    ////        }
    ////    }
    ////    else
    ////    {
    ////        var folder = files.GetFolderByPath(Path.Combine(path, entry.Name));
    ////        if (folder != null)
    ////        {
    ////            files.Delete(folder);
    ////            return Content("");
    ////        }
    ////    }
    ////    throw new HttpException(404, "File Not Found");
    ////}

    //public JsonResult Read()
    //{   
    //    DirectoryInfo di = new DirectoryInfo(@"D:\AART\Azure download\AARTWeb\AARTWeb\Themes\Upload-Images\ImageBrowser\Read");

    //    FileInfo[] Images = di.GetFiles();
    //    //var files = new FilesRepository();

    //    //var folders = files.Folders(path);

    //    //var images = files.Images(path);
    //    //object odata = new
    //    //{
    //    //    userid = Images.,
    //    //    pro_name = prdctname,
    //    //    pro_desp = prdctdesc
    //    //};
    //    //var myContent = "";
    //    //object odata="";
    //    //foreach (var a in Images)
    //    //{
    //    //    odata = new
    //    //    {
    //    //        name = a.Name,
    //    //        type = a.Extension==".png"?"f":"f",
    //    //        size = a.Length
    //    //    };
    //    //    myContent += odata + ",";

    //    //}
    //    //var myContent1 = JsonConvert.SerializeObject(myContent);
    //    //object yourOjbect = new JavaScriptSerializer().DeserializeObject(myContent1);
    //    //// JArray b = JArray.Parse(myContent1);
    //    var con = @"[{""name"":""a100.png"",""type"":""f"",""size"":2270},{""name"":""dojo-banner.jpeg"",""type"":""f"",""size"":12864}]";
    //    object yourOjbect = new JavaScriptSerializer().DeserializeObject(con);

    //    //var contat= "[{ name : a100.png, type : f, size : 2270 },{ 'name' : 'dojo-banner.jpeg', 'type' : 'f', 'size' : 12864 },{ 'name' : 'Thumbnail.png', 'type' : 'f', 'size' : 4027 },{ 'name' : 'types-of-data-structure.jpeg', 'type' : 'f', 'size' : 12864 }]";
    //    return Json(yourOjbect, JsonRequestBehavior.AllowGet);
    //}

    //public ActionResult Thumbnail(string path)
    //{
    //    //TODO:Add security checks

    //   // var files = new FilesRepository();
    //   // var image = files.ImageByPath(path);
    //    DirectoryInfo files = new DirectoryInfo(@"D:\AART\Azure download\AARTWeb\AARTWeb\Themes\Upload-Images\ImageBrowser\Read");

    //    FileInfo[] image = files.GetFiles(path);
    //    if (image != null)
    //    {
    //        var desiredSize = new ImageSize { Width = ThumbnailWidth, Height = ThumbnailHeight };

    //        const string contentType = "image/png";

    //        var thumbnailCreator = new ThumbnailCreator(new FitImageResizer());

    //        using (var stream = new MemoryStream())
    //        {
    //            return File(thumbnailCreator.Create(stream, desiredSize, contentType), contentType);
    //        }
    //    }
    //    throw new HttpException(404, "File Not Found");
    //}

    ////public ActionResult Upload(string path, HttpPostedFileBase file)
    ////{
    ////    if (file != null)
    ////    {
    ////        var files = new FilesRepository();
    ////        var parentFolder = files.GetFolderByPath(path);
    ////        if (parentFolder != null)
    ////        {
    ////            files.SaveImage(parentFolder, file);
    ////            return Json(
    ////                new
    ////                {
    ////                    name = Path.GetFileName(file.FileName)
    ////                }
    ////            , "text/plain");
    ////        }
    ////    }
    ////    throw new HttpException(404, "File Not Found");
    ////}

    //public JsonResult Browse(string path)
    //{
    //    throw new NotImplementedException();
    //}

    //public ActionResult DeleteFile(string path)
    //{
    //    throw new NotImplementedException();
    //}

    //public ActionResult DeleteDirectory(string path)
    //{
    //    throw new NotImplementedException();
    //}

    //public ActionResult CreateDirectory(string path, string name)
    //{
    //    throw new NotImplementedException();
    //}

    //public ActionResult Upload(string path, HttpPostedFileBase file)
    //{
    //    throw new NotImplementedException();
    //}

}