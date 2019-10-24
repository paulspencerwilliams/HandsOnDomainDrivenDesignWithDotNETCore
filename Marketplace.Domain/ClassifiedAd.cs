using System;
using Marketplace.Framework;
using Marketplace.Tests;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity<ClassifiedAdId>

    {
        public ClassifiedAdId Id { get; }
        public ClassifiedAdState State { get; private set; }

        private UserId _ownerId;
        private ClassifiedAdTitle _title;
        private ClassifiedAdText _text;
        private Price _price;

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            _ownerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();
            Raise(new Events.ClassifiedAdCreated
            {
                Id = Id,
                OwnerId = _ownerId
            });
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            _title = title;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = _title
            });
        }

        public void UpdateText(ClassifiedAdText text)
        {
            _text = text;
            EnsureValidState();
            Raise(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                Text = _text 
            });
        }

        public void UpdatePrice(Price price)
        {
            _price = price;
            EnsureValidState();
            Raise(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode
            });
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
            Raise(new Events.ClassifiedAdSentForReview{Id = Id});
        }

        protected override void EnsureValidState()
        {
            var valid =
                Id != null &&
                _ownerId != null &&
                (State switch
                {
                    ClassifiedAdState.PendingReview =>
                    _title != null
                    && _text != null
                    && _price?.Amount > 0,
                    ClassifiedAdState.Active =>
                    _title != null
                    && _text != null
                    && _price?.Amount > 0
                    && ApprovedBy != null,
                    _ => true
                });

            if (!valid)
            {
                throw new InvalidEntityStateException(this, $"Post-checks failed in state {State}");
            }
        }

        public UserId ApprovedBy { get; private set; }

        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }
    }
}