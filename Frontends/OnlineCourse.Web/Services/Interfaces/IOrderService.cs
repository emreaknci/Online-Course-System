﻿using OnlineCourse.Web.Models.Order;

namespace OnlineCourse.Web.Services.Interfaces;
public interface IOrderService
{
    /// <summary>
    /// Senkron iletişim- direkt order mikroservisine istek yapılacak
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);

    /// <summary>
    /// Asenkron iletişim- sipariş bilgileri rabbitMQ'ya gönderilecek
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);

    Task<List<OrderViewModel>> GetOrder();
}