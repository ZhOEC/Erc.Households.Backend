﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Erc.Households.Api.Authorization;
using Erc.Households.Domain.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Erc.Households.EF.PostgreSQL;

namespace Erc.Households.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentChannelsController : ControllerBase
    {
        private readonly ErcContext _ercContext;

        public PaymentChannelsController(ErcContext ercContext)
        {
            _ercContext = ercContext ?? throw new ArgumentNullException(nameof(ercContext));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _ercContext.PaymentChannels.OrderBy(x => x.Id).ToListAsync());

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Add(PaymentChannel paymentChannel)
        {
            _ercContext.PaymentChannels.Add(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok(paymentChannel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Update(PaymentChannel paymentChannel)
        {
            var pc = await _ercContext.PaymentChannels.FindAsync(paymentChannel.Id);

            if (pc is null)
                return NotFound();

            _ercContext.Entry(pc).CurrentValues.SetValues(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ApplicationRoles.Administrator)]
        public async Task<IActionResult> Delete(int id)
        {
            var paymentChannel = await _ercContext.PaymentChannels.FirstOrDefaultAsync(x => x.Id == id);

            if (paymentChannel is null)
                return NotFound();
                
            _ercContext.Remove(paymentChannel);
            await _ercContext.SaveChangesAsync();

            return Ok();
        }
    }
}
