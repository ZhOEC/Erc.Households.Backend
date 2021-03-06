﻿using Erc.Households.Domain.Helpers;

namespace Erc.Households.Domain.Payments
{
    public class PaymentChannel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RecordpointFieldName { get; set; }
        public string SumFieldName { get; set; }
        public string DateFieldName { get; set; }
        public string TextDateFormat { get; set; }
        public string PersonFieldName { get; set; }
        public FileTotalRow TotalRecord { get; set; } = FileTotalRow.None;
        public PaymentType PaymentsType { get; set; }
    }
}
