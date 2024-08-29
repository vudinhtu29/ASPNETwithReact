using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {   
            _context = context;    
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockId = await _context.Stocks.FirstOrDefaultAsync(i => i.Id == id);
            if(stockId == null){
                return null;
            }
            _context.Stocks.Remove(stockId);
            await _context.SaveChangesAsync();
            return stockId;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(s => s.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(l => l.Id == id);
        }

        public Task<bool> StockExist(int id)
        {
            return _context.Stocks.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateStockRequestDto)
        {
            var exsitingStock = await _context.Stocks.FirstOrDefaultAsync(e => e.Id == id);
            if(exsitingStock == null){
                return null;
            }
             exsitingStock.Sympol = updateStockRequestDto.Sympol;
             exsitingStock.CompanyName = updateStockRequestDto.CompanyName;
             exsitingStock.Purcahase = updateStockRequestDto.Purcahase;
             exsitingStock.Industry = updateStockRequestDto.Industry;
             exsitingStock.LastDiv = updateStockRequestDto.LastDiv;
             exsitingStock.MarketCap = updateStockRequestDto.MarketCap;

             await _context.SaveChangesAsync();
             return exsitingStock;
        }
    }
}