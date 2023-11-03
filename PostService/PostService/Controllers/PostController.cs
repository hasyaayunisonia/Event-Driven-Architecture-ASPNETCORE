﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PostService.Data;
using PostService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostServiceContext _context;

        public PostController(PostServiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost()
        {
            return await _context.Post.Include(x => x.User).ToListAsync();
        }

        /*[HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.PostId }, post);
        }*/

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _context.Post
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PostId == id);

            if (post == null)
            {
                return NotFound(); // Mengembalikan respons 404 jika tidak ditemukan
            }

            return post;
        }



        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
        {
            var existingUser = await _context.User.FindAsync(post.UserId);

            if (existingUser == null)
            {
                // Handle jika pengguna tidak ditemukan
                return BadRequest("User not found");
            }

            post.User = existingUser; // Setel pengguna untuk posting
            _context.Post.Add(post);
            await _context.SaveChangesAsync();

            // Muat ulang posting termasuk informasi pengguna
            var postedWithUser = await _context.Post
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.PostId == post.PostId);

            return CreatedAtAction("GetPost", new { id = post.PostId }, postedWithUser);
        }

    }
}
