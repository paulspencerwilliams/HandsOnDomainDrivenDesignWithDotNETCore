using System;

namespace Marketplace.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler where TId : IEquatable<TId>
    {
        public TId Id { get; protected set; }
        private readonly Action<object> _applier;
        protected abstract void When(object @event);

        protected Entity(Action<object> applier) => this._applier = applier;

        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}