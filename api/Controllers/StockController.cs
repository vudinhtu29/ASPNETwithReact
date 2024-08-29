using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private const string V = "{id}";
        private readonly ApplicationDBContext _context; 
        private readonly IStockRepository _stockRepository;  
        public StockController(ApplicationDBContext context,IStockRepository stockRepository){
            _context = context;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            //var stocks = await _context.Stocks.ToListAsync();
            var stocks = await _stockRepository.GetAllAsync();
            var stockDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var stock = await _context.Stocks.FindAsync(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel.ToStockDto());
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateStock){
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if(stockModel == null){
                return NotFound();
            }

            stockModel.Sympol = updateStock.Sympol;
            stockModel.CompanyName = updateStock.CompanyName;
            stockModel.Purcahase = updateStock.Purcahase;
            stockModel.Industry = updateStock.Industry;
            stockModel.LastDiv = updateStock.LastDiv;
            stockModel.MarketCap = updateStock.MarketCap;

            await _context.SaveChangesAsync();
            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            var deleteStock = await _context.Stocks.FirstOrDefaultAsync(d => d.Id == id);
            if(deleteStock == null){
                return NotFound();
            }
            _context.Stocks.Remove(deleteStock);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}