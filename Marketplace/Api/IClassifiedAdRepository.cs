using System.Threading.Tasks;
using Marketplace.Domain;

namespace Marketplace.Api
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(ClassifiedAdId id);

        Task<ClassifiedAd> Load(ClassifiedAdId id);

        Task Save(ClassifiedAd entity);
    }
}