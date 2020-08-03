﻿using AutoMapper;
using Erc.Households.Domain;
using Erc.Households.Domain.AccountingPoints;
using Erc.Households.Domain.Addresses;
using Erc.Households.Domain.Payments;
using System.Linq;

namespace Erc.Households.Api.MapperProfiles
{
	public class MainProfile : Profile
	{
		public MainProfile()
		{
			CreateMap<AccountingPoint, Responses.AccountingPoint>()
				.ForMember(x => x.TariffId, x => x.MapFrom(y => y.CurrentTariff.Id))
				.ForMember(x => x.City, x => x.MapFrom(y => y.Address.Street.City))
				.ForMember(x => x.ContractStartDate, x => x.MapFrom(y => y.CurrentContract.StartDate));
			CreateMap<Address, Responses.Address>()
				.ForMember(x => x.CityId, x => x.MapFrom(y => y.Street.City.Id));
			CreateMap<Person, Responses.Person>();
			CreateMap<BranchOffice, Responses.BranchOffice>();
			CreateMap<Domain.Payments.PaymentsBatch, Responses.PaymentsBatch>()
				.ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.Payments.Sum(t => t.Amount)))
				.ForMember(x => x.TotalCount, x => x.MapFrom(y => y.Payments.Count()));
			CreateMap<Domain.Payments.Payment, Responses.Payment>()
				.ForMember(x => x.AccountingPointName, x => x.MapFrom(y => y.AccountingPoint.Name));
			CreateMap<UsageCategory, Responses.UsageCategory>();

			CreateMap<Domain.Billing.Invoice, Responses.Invoice>()
				.ForMember(i => i.Type, s => s.MapFrom(_ => "Звичайний"));
			CreateMap<Domain.AccountingPoints.AccountingPointExemption, Responses.AccountingPointExemption>()
				.ForMember(r=>r.CategoryName, mo=>mo.MapFrom(s=>$"{s.Category.Name} ({s.Category.Coeff:#.#}%)"));
		}
	}
}
