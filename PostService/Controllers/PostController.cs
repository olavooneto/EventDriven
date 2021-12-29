using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostServiceContext _context;

        public PostController(PostServiceContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()=> await this._context.Posts.Include(x=>x.User).ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            this._context.Posts.Add(post);
            await this._context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new {id = post.PostId}, post);
        }
    }
}