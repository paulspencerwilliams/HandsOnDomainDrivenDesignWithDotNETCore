using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class PictureId : Value<PictureId>
    {
        public Guid Value { get; }

        protected PictureId()
        {
        }

        public PictureId(Guid value) => Value = value;
    }
}