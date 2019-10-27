using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class Picture : Entity<PictureId>
    {
        public Guid PictureId
        {
            get => Id.Value;
            set {}
        }

        public ClassifiedAdId ParentId { get; private set; }
        public PictureSize Size { get; private set; }
        internal Uri Uri { get; set; }
        internal int Order { get; set; }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.PictureAddedToAClassifiedAd e:
                    ParentId = new ClassifiedAdId(e.ClassifiedAdId);
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
        
        protected Picture() {}

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