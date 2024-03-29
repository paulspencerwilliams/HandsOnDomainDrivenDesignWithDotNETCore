using System.Threading.Tasks;
using Marketplace.Domain;

namespace Marketplace.Domain
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(ClassifiedAdId id);

        Task<ClassifiedAd> Load(ClassifiedAdId id);

        Task Add(ClassifiedAd entity);
    }
}