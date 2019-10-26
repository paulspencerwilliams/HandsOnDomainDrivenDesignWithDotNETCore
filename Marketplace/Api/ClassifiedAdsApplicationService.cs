using System;
using System.Threading.Tasks;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.Api
{
    public partial class ClassifiedAdsApplicationService
    {
        private readonly IClassifiedAdRepository _store;
        private readonly ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService(IClassifiedAdRepository store, ICurrencyLookup currencyLookup)
        {
            _store = store;
            _currencyLookup = currencyLookup;
        }

        public Task Handle(object command) =>
            command switch
            {
                ClassifiedAds.V1.Create cmd => HandleCreate(cmd),

                ClassifiedAds.V1.SetTitle cmd => HandleUpdate(
                    cmd.Id,
                    c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))
                ),

                ClassifiedAds.V1.UpdateText cmd => HandleUpdate(
                    cmd.Id,
                    c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text))
                ),

                ClassifiedAds.V1.UpdatePrice cmd => HandleUpdate(
                    cmd.Id,
                    c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))
                ),
                
                ClassifiedAds.V1.RequestToPublish cmd => HandleUpdate(
                    cmd.Id,
                    c => c.RequestToPublish()
                )
            };

        private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
        {
            if (await _store.Exists(cmd.Id.ToString()))
            {
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            }

            var classifiedAd = new ClassifiedAd(
                new ClassifiedAdId(cmd.Id),
                new UserId(cmd.OwnerId));

            await _store.Save(classifiedAd);
        }

        private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await _store.Load(classifiedAdId.ToString());
            if (classifiedAd == null)
            {
                throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
            }
            
            operation(classifiedAd);

            await _store.Save(classifiedAd);
        }
    }
}