using System;

namespace Marketplace.Framework
{
    public abstract class Entity<TId> where TId : IEquatable<TId>
    {
        protected abstract void EnsureValidState();
    }
}