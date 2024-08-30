using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace api.Properties
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepo;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

       

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existComment = await _context.Comments.FindAsync(id);
            if(existComment == null){
                return null;
            }
            existComment.Title = commentModel.Title;
            existComment.Content = commentModel.Content;

            await _context.SaveChangesAsync();
            return existComment;

        }
    }
}