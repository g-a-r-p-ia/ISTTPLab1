using ISSTTP.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ISSTTP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private record CountByCategoryResponseItem(string Category, int Count);
        private readonly DbcarShopContext dbCarShopContext;

        public ChartController(DbcarShopContext dbCarShopContext)
        {
            this.dbCarShopContext = dbCarShopContext;
        }
        [HttpGet("countByCategory")]
        public async Task<JsonResult> GetCountByCategoryAsync(CancellationToken cancellationToken)
        {
            var responseItems = await dbCarShopContext
            .Details
            .GroupBy(purchase => purchase.CategoryId)
            .Select(group => new CountByCategoryResponseItem(group.Key.ToString(), group.Count()))
            .ToListAsync(cancellationToken);

            return new JsonResult(responseItems);
        }

    }
}
