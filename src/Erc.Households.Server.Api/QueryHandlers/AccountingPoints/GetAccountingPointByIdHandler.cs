﻿using AutoMapper;
using Erc.Households.Api.Queries.AccountingPoints;
using Erc.Households.Api.Responses;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Erc.Households.Api.QueryHandlers.AccountingPoints
{
    public class GetAccountingPointByIdHandler : IRequestHandler<GetAccountingPointById, AccountingPoint>
    {
        private readonly ErcContext _ercContext;
        readonly IMapper _mapper;

        public GetAccountingPointByIdHandler(ErcContext ercContext, IMapper mapper)
        {
            _ercContext = ercContext;
            _mapper = mapper;
        }

        public async Task<AccountingPoint> Handle(GetAccountingPointById request, CancellationToken cancellationToken)
        {
            var ap = await _ercContext.AccountingPoints
                .Include(a => a.TariffsHistory)
                    .ThenInclude(th => th.Tariff)
                .Include(a => a.ContractsHistory)
                     .ThenInclude(c => c.Customer)
                .Include(a => a.Address.Street.City.District.Region)
                .Include(a => a.DistributionSystemOperator)
                .Include(a => a.BranchOffice)
                .FirstAsync(a => a.Id == request.Id);
            
                return _mapper.Map<AccountingPoint>(ap);
        }
    }
}