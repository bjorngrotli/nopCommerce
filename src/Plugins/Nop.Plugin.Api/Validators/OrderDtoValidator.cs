﻿using System;
using System.Collections.Generic;
using FluentValidation;
using Nop.Plugin.Api.DTOs.Orders;

namespace Nop.Plugin.Api.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator(string httpMethod, Dictionary<string, object> passedPropertyValuePaires)
        {
            if (string.IsNullOrEmpty(httpMethod) ||
                httpMethod.Equals("post", StringComparison.InvariantCultureIgnoreCase))
            {
                RuleFor(x => x.CustomerId)
                    .NotNull()
                    .Must(id => id > 0)
                    .WithMessage("Invalid customer_id");

                //RuleFor(x => x.PaymentMethodSystemName)
                //    .NotNull()
                //    .NotEmpty()
                //    .WithMessage("Payment method system name is required");
            }
            else if(httpMethod.Equals("put", StringComparison.InvariantCultureIgnoreCase))
            {
                int parsedId = 0;

                RuleFor(x => x.Id)
                        .NotNull()
                        .NotEmpty()
                        .Must(id => int.TryParse(id, out parsedId) && parsedId > 0)
                        .WithMessage("Invalid id");

                if (passedPropertyValuePaires.ContainsKey("customer_id"))
                {
                    RuleFor(x => x.CustomerId)
                       .NotNull()
                       .Must(id => id > 0)
                       .WithMessage("Invalid customer_id");
                }

                //if (passedPropertyValuePaires.ContainsKey("payment_method_system_name"))
                //{
                //    RuleFor(x => x.PaymentMethodSystemName)
                //       .NotNull()
                //       .NotEmpty()
                //       .WithMessage("Payment method system name is required");
                //}
            }
        }
    }
}