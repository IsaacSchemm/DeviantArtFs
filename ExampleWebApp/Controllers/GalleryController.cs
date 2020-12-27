using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExampleWebApp.Data;
using DeviantArtFs.Extensions;
using Microsoft.AspNetCore.Mvc;
using DeviantArtFs;

namespace ExampleWebApp.Controllers
{
    public class GalleryController : ControllerBase
    {
        public GalleryController(ExampleDbContext context, DeviantArtApp appReg) : base(context, appReg) { }

        public async Task<IActionResult> Index(string username = null, Guid? folderId = null, int offset = 0, int? limit = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var paging = new DeviantArtPagingParams(offset, limit);
            IDeviantArtResultPage<DeviantArtPagingParams, Deviation> resp;
            if (folderId is Guid f) {
                resp = await DeviantArtFs.Api.Gallery.GalleryById.ExecuteAsync(
                    token,
                    DeviantArtObjectExpansion.None,
                    new DeviantArtFs.Api.Gallery.GalleryByIdRequest { Folderid = f, Username = username },
                    paging);
            } else {
                resp = await DeviantArtFs.Api.Gallery.GalleryAllView.ExecuteAsync(
                    token,
                    new DeviantArtFs.Api.Gallery.GalleryAllViewRequest { Username = username },
                    paging);
            }

            ViewBag.Username = username;
            ViewBag.FolderId = folderId;
            ViewBag.Limit = limit;
            return View(resp);
        }

        public async Task<IActionResult> List(string username = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var list = await DeviantArtFs.Api.Gallery.GalleryFolders.ToArrayAsync(
                token,
                new DeviantArtFs.Api.Gallery.GalleryFoldersRequest { CalculateSize = true, Username = username },
                0,
                100);

            ViewBag.Username = username;
            return View(list);
        }
    }
}