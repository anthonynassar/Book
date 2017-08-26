using System;
using System.Collections.Generic;
using System.Text;

namespace PeopleApp.Models.ViewsRelated
{
    public class SharingSpaceView : SharingSpace
    {
        public SharingSpaceView(SharingSpace sharingSpace, bool isVisible)
        {
            base.Id = sharingSpace.Id;
            base.Descriptor = sharingSpace.Descriptor;
            base.CreationLocation = sharingSpace.CreationLocation;
            base.CreationDate = sharingSpace.CreationDate;
            base.CreatedAt = sharingSpace.CreatedAt;
            base.UpdatedAt = sharingSpace.UpdatedAt;
            base.UserId = sharingSpace.UserId;
            base.Version = sharingSpace.Version;
            IsVisible = isVisible;
        }

        public bool IsVisible { get; set; }
    }
}
