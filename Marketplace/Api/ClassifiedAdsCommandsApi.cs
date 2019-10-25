using System.Threading.Tasks;
using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api
{
    [Route(("/ad"))]
    public class ClassifiedAdsCommandsApi : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
        {
            return Ok();
        }
    }
}