using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        internal PictureSize Size { get; set; }
        internal Uri Uri { get; set; }
        internal int Order { get; set; }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    Id = new PictureId(e.PictureId);
                    Uri = new Uri(e.Uri);
                    Size = new PictureSize(e.Width, e.Height);
                    Order = e.Order;
                    break;
                case Events.ClassifiedAdPictureResized e:
                    Size = new PictureSize{Height = e.Height, Width = e.Width};
                    break;
            }       
        }

        public Picture(Action<object> applier) : base(applier)
        {
        }

        public void Resize(PictureSize newSize) => Apply(new Events.ClassifiedAdPictureResized
        {
            PictureId = Id.Value,
            Width = newSize.Width,
            Height = newSize.Height
        });
    }
}