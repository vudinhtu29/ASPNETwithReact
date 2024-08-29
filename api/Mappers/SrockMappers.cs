using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class SrockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel){
            return new StockDto{
                Id = stockModel.Id,
                Sympol = stockModel.Sympol,
                CompanyName = stockModel.CompanyName,
                Purcahase = stockModel.Purcahase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(s => s.ToCommentDto()).ToList()
            };
        }
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDto stockDto){
            return new Stock{
                Sympol = stockDto.Sympol,
                CompanyName = stockDto.CompanyName,
                Purcahase = stockDto.Purcahase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap    
            };
        }
       
    }
}