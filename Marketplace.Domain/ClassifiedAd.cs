using System;

namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; }
        private UserId _ownerId;
        private string _title;
        private string _text;
        private decimal _price;
        
        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            _ownerId = ownerId;
        }

        public void SetTitle(string title) => _title = title;
        public void SetText(string text) => _text = text;
        public void SetPrice(decimal price) => _price = price;
    }
}