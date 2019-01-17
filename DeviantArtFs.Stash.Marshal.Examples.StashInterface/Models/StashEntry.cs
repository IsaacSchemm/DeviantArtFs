using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Stash.Marshal.Examples.StashInterface.Models
{
    public class StashEntry : IStashDelta
    {
        public int StashEntryId { get; set; }

        public Guid UserId { get; set; }

        public long? ItemId { get; set; }

        public long StackId { get; set; }

        [Required]
        public string MetadataJson { get; set; }

        public int Position { get; set; }

        long? IStashDelta.Itemid => ItemId;

        long? IStashDelta.Stackid => StackId;

        string IStashDelta.MetadataJson => MetadataJson;

        int? IStashDelta.Position => Position;
    }
}
