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
                MarketCap = stockModel.MarketCap
            };
        }
    }
}