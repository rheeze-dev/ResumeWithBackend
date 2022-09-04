using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoriesController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region CRUD operations

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            List<Category> categories = await _appDbContext.Categories.ToListAsync();

            return Ok(categories);
        }

        // website.com/api/categories/withposts
        [HttpGet("withposts")]
        public async Task<IActionResult> GetWithPosts()
        {
            List<Category> categories = await _appDbContext.Categories
                .Include(category => category.Posts)
                .ToListAsync();

            return Ok(categories);
        }

        // website.com/api/categories/2
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Category category = await GetCategoryByCategoryId(id, false);

            return Ok(category);
        }

        // website.com/api/categories/withposts/2
        [HttpGet("withposts/{id}")]
        public async Task<IActionResult> GetWithPosts(int id)
        {
            Category category = await GetCategoryByCategoryId(id, true);

            return Ok(category);
        }
        #endregion

        #region Utility methods

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> PersistChangesToDatabase() 
        {
            int amountOfChanges = await _appDbContext.SaveChangesAsync();

            return amountOfChanges > 0;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<Category> GetCategoryByCategoryId(int categoryId, bool withPosts)
        {
            Category categoryToGet = null;

            if (withPosts == true)
            {
                categoryToGet = await _appDbContext.Categories
                    .Include(category => category.Posts)
                    .FirstAsync(category => category.CategoryId == categoryId);
            }
            else
            {
                categoryToGet = await _appDbContext.Categories
                    .FirstAsync(category => category.CategoryId == categoryId);
            }

            return categoryToGet;
        }

        #endregion
    }
}
