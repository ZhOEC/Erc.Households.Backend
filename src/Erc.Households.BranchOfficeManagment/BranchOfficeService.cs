﻿using Erc.Households.BranchOfficeManagment.Core;
using Erc.Households.Commands;
using Erc.Households.Domain;
using Erc.Households.Domain.Billing;
using Erc.Households.EF.PostgreSQL;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erc.Households.BranchOfficeManagment
{
    public class BranchOfficeService : IBranchOfficeService
    {
        readonly ErcContext _dbContext;
        private readonly IMediator _mediator; 
        static readonly object _sync = new object();

        public BranchOfficeService(ErcContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public IEnumerable<BranchOffice> GetList(params int[] branchOfficeIds)
        {
            lock (_sync)
            {
                return _dbContext.BranchOffices
                    .Include(b=>b.CurrentPeriod)
                    .Where(b => branchOfficeIds.Contains(b.Id))
                    .ToArray();
            }
        }

        public IEnumerable<BranchOffice> GetList(IEnumerable<string> branchOfficeIds)
        {
            lock (_sync)
            {
                return _dbContext.BranchOffices
                    .Include(b => b.CurrentPeriod)
                    .Where(b => branchOfficeIds.Contains(b.StringId))
                    .ToArray();
            }
        }

        public BranchOffice GetOne(int id)
        {
            lock (_sync)
            {
                return _dbContext.BranchOffices
                    .Include(b => b.CurrentPeriod)
                    .First(b => b.Id == id);
            }
        }

        public void StartNewPeriod(int branchOfficeId)
        {
            lock (_sync)
            {
                var branchOffice = _dbContext.BranchOffices
                    .Include(b => b.CurrentPeriod)
                    .First(b => b.Id == branchOfficeId);
                
                var period = _dbContext.Periods.FirstOrDefault(p => p.StartDate == branchOffice.CurrentPeriod.EndDate.AddDays(1));

                using var transaction = _dbContext.Database.BeginTransaction();

                if (period is null)
                {
                    period = new Period(branchOffice.CurrentPeriod.EndDate.AddDays(1), branchOffice.CurrentPeriod.EndDate.AddMonths(1));
                    _dbContext.Entry(period).State = EntityState.Added;
                }

                branchOffice.StartNewPeriod(period);
                _mediator.Send(new CreateTaxInvoiceCommand(branchOffice.CurrentPeriod)); // The current period before switching to a new one

                _dbContext.SaveChanges();
                _dbContext.Database.ExecuteSqlInterpolated($"insert into accounting_point_debt_history(accounting_point_id, period_id, debt_value) select id, {period.Id}, debt from accounting_points where branch_office_Id={branchOfficeId}");

                transaction.Commit();
            }
        }
    }
}
