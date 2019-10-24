using System;
using Marketplace.Framework;
using Marketplace.Tests;

namespace Marketplace.Domain
{
    public class ClassifiedAd : Entity<ClassifiedAdId>

    {
        public ClassifiedAdId Id { get; private set; }
        public ClassifiedAdState State { get; private set; }

        private UserId _ownerId;
        private ClassifiedAdTitle _title;
        private ClassifiedAdText _text;
        private Price _price;

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Apply(new Events.ClassifiedAdCreated
            {
                Id = id,
                OwnerId = ownerId
            });
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Apply(new Events.ClassifiedAdTitleChanged
            {
                Id = Id,
                Title = title
            });
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Apply(new Events.ClassifiedAdTextUpdated
            {
                Id = Id,
                Text = text
            });
        }

        public void UpdatePrice(Price price)
        {
            Apply(new Events.ClassifiedAdPriceUpdated
            {
                Id = Id,
                Price = price.Amount,
                CurrencyCode = price.Currency.CurrencyCode
            });
        }

        public void RequestToPublish()
        {
            Apply(new Events.ClassifiedAdSentForReview {Id = Id});
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAdCreated e:
                    Id = new ClassifiedAdId(e.Id);
                    _ownerId = new UserId(e.OwnerId);
                    State = ClassifiedAdState.Inactive;
                    break;
                case Events.ClassifiedAdTitleChanged e:
                    _title = new ClassifiedAdTitle(e.Title);
                    break;
                case Events.ClassifiedAdTextUpdated e:
                    _text = new ClassifiedAdText(e.Text);
                    break;
                case Events.ClassifiedAdPriceUpdated e:
                    _price = new Price(e.Price, e.CurrencyCode);
                    break;
                case Events.ClassifiedAdSentForReview e:
                    State = ClassifiedAdState.PendingReview;
                    break;
            }
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