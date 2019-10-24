using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdId : Value<ClassifiedAdId>
    {
        public ClassifiedAdId(Guid value) => Value = value;
        
        public static implicit operator Guid(ClassifiedAdId self) => self.Value;
        public Guid Value { get; }
    }
}