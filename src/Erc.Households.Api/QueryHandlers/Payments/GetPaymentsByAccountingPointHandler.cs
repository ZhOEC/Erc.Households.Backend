﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Erc.Households.Api.Queries.Payments;
using Erc.Households.Api.Responses;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using X.PagedList;

namespace Erc.Households.Api.QueryHandlers.Payments
{
    public class GetPaymentsByAccountingPointHandler : IRequestHandler<GetPaymentsByAccountingPoint, IPagedList<AccountingPointPaymentListItem>>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetPaymentsByAccountingPointHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<IPagedList<Payment>> Handle(GetPaymentsByPart request, CancellationToken cancellationToken)
        {
            return await _ercContext.Payments
                .Include(t => t.AccountingPoint)
                .Include(t => t.Batch.BranchOffice)
                .Where(x => x.Batch.Id == request.PaymentsBatchId && (request.ShowProcessed || x.Status != Domain.Payments.PaymentStatus.Processed))
                .ProjectTo<Payment>(_mapper.ConfigurationProvider)
                .OrderByDescending(x => x.Id)
                    .ThenBy(x => x.AccountingPointName)
                    .ThenBy(x => x.PayerInfo)
                    .ThenBy(x => x.Status)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }

        public async Task<IPagedList<AccountingPointPaymentListItem>> Handle(GetPaymentsByAccountingPoint request, CancellationToken cancellationToken)
        {
            return await _ercContext.Payments
                .Include(p => p.Batch.PaymentChannel)
                .Where(p => p.AccountingPointId == request.AccountingPointId && p.Status == Domain.Payments.PaymentStatus.Processed)
                .OrderByDescending(p => p.EnterDate)
                .ThenByDescending(p => p.Id)
                .ProjectTo<AccountingPointPaymentListItem>(_mapper.ConfigurationProvider)
                .ToPagedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
