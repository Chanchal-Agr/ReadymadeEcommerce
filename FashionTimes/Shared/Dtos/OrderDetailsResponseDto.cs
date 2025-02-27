﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionTimes.Shared.Dtos
{
    public class OrderDetailsResponseDto
    {
        public string OrderNumber {  get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsProductResponseDto> Products { get; set; }
    }
}
