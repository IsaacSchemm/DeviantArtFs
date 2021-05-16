using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExampleWebApp.Data;
using DeviantArtFs.Extensions;
using Microsoft.AspNetCore.Mvc;
using DeviantArtFs;
using System.Threading;
using DeviantArtFs.ParameterTypes;

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
            IDeviantArtResultPage<PagingOffset, Deviation> resp;
            if (folderId is Guid f) {
                resp = await DeviantArtFs.Api.Gallery.AsyncPageGallery(
                    token,
                    ObjectExpansion.None,
                    new DeviantArtFs.Api.Gallery.GalleryRequest { Folderid = f, Username = username },
                    limit_param,
                    offset_param).StartAsTask(cancellationToken: cancellationToken);
            } else {
                resp = await DeviantArtFs.Api.Gallery.AsyncPageAllView(
                    token,
                    new DeviantArtFs.Api.Gallery.GalleryAllViewRequest { Username = username },
                    limit_param,
                    offset_param).StartAsTask(cancellationToken: cancellationToken);
            }

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
                new DeviantArtFs.Api.Gallery.GalleryFoldersRequest { CalculateSize = true, Username = username },
                PagingLimit.MaximumPagingLimit,
                PagingOffset.FromStart).ThenToList().StartAsTask(cancellationToken: cancellationToken);

            ViewBag.Username = username;
            return View(list);
        }
    }
}