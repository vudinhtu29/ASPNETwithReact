using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class StockDto
    {
         public int Id { get; set; }
        public string Sympol { get; set; } = string.Empty;
        public string CompanyName {get ; set;} = string.Empty;
        public decimal Purcahase {get ; set;}
        public decimal LastDiv {get;set;}
        public string Industry {get;set;} = string.Empty;
        public long MarketCap {get;set;}
    }
}