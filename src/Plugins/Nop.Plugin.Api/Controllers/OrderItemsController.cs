﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.DTOs.OrderItems;
using Nop.Plugin.Api.JSON.ActionResults;
using Nop.Plugin.Api.MappingExtensions;
using Nop.Plugin.Api.Models.OrderItemsParameters;
using Nop.Plugin.Api.Serializers;
using Nop.Plugin.Api.Services;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Plugin.Api.Controllers
{
    public class OrderItemsController : BaseApiController
    {
        private readonly IOrderItemApiService _orderItemApiService;
        private readonly IOrderApiService _orderApiService;

        public OrderItemsController(IJsonFieldsSerializer jsonFieldsSerializer, 
            IAclService aclService, 
            ICustomerService customerService, 
            IStoreMappingService storeMappingService, 
            IStoreService storeService, 
            IDiscountService discountService, 
            ICustomerActivityService customerActivityService, 
            ILocalizationService localizationService, 
            IOrderItemApiService orderItemApiService, 
            IOrderApiService orderApiService) 
            : base(jsonFieldsSerializer, 
                  aclService, 
                  customerService, 
                  storeMappingService, 
                  storeService, 
                  discountService, 
                  customerActivityService, 
                  localizationService)
        {
            _orderItemApiService = orderItemApiService;
            _orderApiService = orderApiService;
        }

        [HttpGet]
        [ResponseType(typeof(OrderItemsRootObject))]
        public IHttpActionResult GetOrderItems(int orderId, OrderItemsParametersModel parameters)
        {
            if (parameters.Limit < Configurations.MinLimit || parameters.Limit > Configurations.MaxLimit)
            {
                return Error(HttpStatusCode.BadRequest, "limit", "Invalid limit parameter");
            }

            if (parameters.Page < Configurations.DefaultPageValue)
            {
                return Error(HttpStatusCode.BadRequest, "page", "Invalid request parameters");
            }

            Order order = _orderApiService.GetOrderById(orderId);

            if (order == null)
            {
                return Error(HttpStatusCode.NotFound, "order", "not found");
            }

            IList<OrderItem> allOrderItemsForOrder = _orderItemApiService.GetOrderItemsForOrder(order, parameters.Limit, parameters.Page, parameters.SinceId);

            var orderItemsRootObject = new OrderItemsRootObject()
            {
                OrderItems = allOrderItemsForOrder.Select(item => item.ToDto()).ToList()
            };

            var json = _jsonFieldsSerializer.Serialize(orderItemsRootObject, parameters.Fields);

            return new RawJsonActionResult(json);
        }
    }
}