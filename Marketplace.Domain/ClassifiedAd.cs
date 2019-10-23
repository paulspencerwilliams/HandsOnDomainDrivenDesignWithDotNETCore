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
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            _title = title;
            EnsureValidState();
        }

        public void UpdateText(ClassifiedAdText text)
        {
            _text = text;
            EnsureValidState();
        }

        public void UpdatePrice(Price price)
        {
            _price = price;
            EnsureValidState();
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
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