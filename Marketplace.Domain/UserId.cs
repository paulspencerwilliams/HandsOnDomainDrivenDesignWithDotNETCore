using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class UserId : Value<UserId>
    {
        protected UserId() { }
        public UserId(Guid value) => Value = value;
        public Guid Value { get; internal set; }

        public static implicit operator Guid(UserId self) => self.Value;
    }
}