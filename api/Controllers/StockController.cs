using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private const string V = "{id}";
        private readonly ApplicationDBContext _context;   
        public StockController(ApplicationDBContext context){
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll(){
            var stocks = _context.Stocks.ToList()
                .Select(s => s.ToStockDto());
            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id){
            var stock = _context.Stocks.Find(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
            var stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById),new {id = stockModel.Id},stockModel.ToStockDto());
        }
        [HttpPut]
        public IActionResult Update([FromRoute] int id,[FromBody] UpdateStockRequestDto updateStock){
            var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);
            if(stockModel == null){
                return NotFound();
            }

            stockModel.Sympol = updateStock.Sympol;
            stockModel.CompanyName = updateStock.CompanyName;
            stockModel.Purcahase = updateStock.Purcahase;
            stockModel.Industry = updateStock.Industry;
            stockModel.LastDiv = updateStock.LastDiv;
            stockModel.MarketCap = updateStock.MarketCap;

            _context.SaveChanges();
            return Ok(stockModel.ToStockDto());
        }
    }
}