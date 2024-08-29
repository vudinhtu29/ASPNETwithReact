using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Mappers
{
    public static class CommentMapper
    {
          public static CommentDto ToCommentDto(this Comment commentModel){
            return new CommentDto{
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
          }
    }
}