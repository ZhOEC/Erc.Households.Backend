﻿using Erc.Households.Api.Queries.Payments;
using Erc.Households.Domain.Payments;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class GetPaymentsChannelByIdHandler : IRequestHandler<GetPaymentChannelById, PaymentChannel>
    {
        private readonly ErcContext _ercContext;

        public GetPaymentsChannelByIdHandler(ErcContext ercContext)
        {
            _ercContext = ercContext;
        }

        public async Task<PaymentChannel> Handle(GetPaymentChannelById request, CancellationToken cancellationToken)
        {
            return await _ercContext.PaymentChannels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.PaymentChannelId);
        }
    }
}
