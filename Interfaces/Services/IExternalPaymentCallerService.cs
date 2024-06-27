﻿using EventSeller.DataLayer.ExternalDTO.PaymentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSeller.DataLayer.Entities
{
    public interface IExternalPaymentService
    {
        public Task<ProcessPaymentResponse> ProcessPaymentAsync(long cardId, decimal amount, string currency, decimal unreturnableFee = 0);
        public Task ConfirmPaymentAsync(long transactionId, string confirmationCode);
        public Task CancelPaymentAsync(long transactionId);
        public Task ReturnPaymentAsync(long transactionId);
    }
}
