﻿using Erc.Households.Server.Domain.AccountingPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Erc.Households.Server.Domain.Billing
{
    public class ZoneCoeff
    {
        public int Id { get; private set; }
        public ZoneNumber ZoneNumber { get; private set; }
        public ZoneRecord ZoneRecord { get; private set; }
        public decimal Value { get; private set; }
        public DateTime StartDate { get; private set; }
    }
}