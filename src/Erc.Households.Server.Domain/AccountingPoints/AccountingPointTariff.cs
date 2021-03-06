﻿using Erc.Households.Domain.Shared.Tariffs;
using Erc.Households.ModelLogs;
using System;

namespace Erc.Households.Domain.AccountingPoints
{
    public class AccountingPointTariff: LogableObjectBase
    {
        protected AccountingPointTariff()
        {
            
        }

        public AccountingPointTariff(int tariffId, DateTime date, string user)
        {
            TariffId = tariffId;
            StartDate = date;
            AddLog("create record", user);
        }

        public int Id { get; private set; }
        public DateTime StartDate { get; private set; }
        public int AccountingPointId { get; private set; }
        public int TariffId { get; private set; }
        public Tariff Tariff { get; private set; }
    }
}
