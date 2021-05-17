using System;
using System.Threading.Tasks;
using ExampleWebApp.Data;
using DeviantArtFs.Extensions;
using Microsoft.AspNetCore.Mvc;
using DeviantArtFs;
using System.Threading;
using DeviantArtFs.ParameterTypes;
using DeviantArtFs.ResponseTypes;
using DeviantArtFs.Pages;

namespace ExampleWebApp.Controllers
{
    public class GalleryController : ControllerBase
    {
        public GalleryController(ExampleDbContext context, DeviantArtApp appReg) : base(context, appReg) { }

        public async Task<IActionResult> Index(CancellationToken cancellationToken, string username = null, Guid? folderId = null, int offset = 0, int? limit = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var offset_param = PagingOffset.NewPagingOffset(offset);
            var limit_param = limit is int l
                ? PagingLimit.NewPagingLimit(l)
                : PagingLimit.MaximumPagingLimit;
            var resp = await DeviantArtFs.Api.Gallery.AsyncPageGallery(
                token,
                ObjectExpansion.None,
                username != null
                    ? UserScope.NewForUser(username)
                    : UserScope.ForCurrentUser,
                folderId is Guid ff
                    ? GalleryFolderScope.NewSingleGalleryFolder(ff)
                    : GalleryFolderScope.AllGalleryFoldersPopular,
                limit_param,
                offset_param).StartAsTask(cancellationToken: cancellationToken);

            ViewBag.Username = username;
            ViewBag.FolderId = folderId;
            ViewBag.Limit = limit;
            return View(resp);
        }

        public async Task<IActionResult> List(CancellationToken cancellationToken, string username = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var list = await DeviantArtFs.Api.Gallery.AsyncGetFolders(
                token,
                CalculateSize.NewCalculateSize(true),
                FolderPreload.Default,
                username != null
                    ? UserScope.NewForUser(username)
                    : UserScope.ForCurrentUser,
                PagingLimit.MaximumPagingLimit,
                PagingOffset.FromStart).ThenToList().StartAsTask(cancellationToken: cancellationToken);

            ViewBag.Username = username;
            return View(list);
        }
    }
}