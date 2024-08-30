using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepository,IStockRepository stockRepo)
        {
            _commentRepo = commentRepository;
            _stockRepo = stockRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.GetAllAsync();
            var commentDto = comment.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
             if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.GetByIdAsync(id);
            if(comment == null){
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId,CreateCommentDto commentDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            if(!await _stockRepo.StockExist(stockId)){
                return BadRequest("Stoke does not exist");
            }
            var commentModel = commentDto.ToCommentFromCreate(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById),new {id = commentModel.Id},commentModel.ToCommentDto());
        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,UpdateCommentRequestDto commemtRequest){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var comment = await _commentRepo.UpdateAsync(id,commemtRequest.ToCommentFromUpdate());
            if(comment == null){
                return NotFound("Comment Not Found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var removeComment =  await _commentRepo.DeleteAsync(id);
            if(removeComment == null){
                return NotFound();
            }
            return NoContent();
        }
    }
}