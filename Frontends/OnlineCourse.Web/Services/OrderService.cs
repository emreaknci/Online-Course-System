using NuGet.ContentModel;
using OnlineCourse.Shared.Dtos;
using OnlineCourse.Shared.Services;
using OnlineCourse.Web.Models.Basket;
using OnlineCourse.Web.Models.Order;
using OnlineCourse.Web.Models.Payment;
using OnlineCourse.Web.Services.Interfaces;

namespace OnlineCourse.Web.Services;
public class OrderService : IOrderService
{
    private readonly IPaymentService _paymentService;
    private readonly HttpClient _httpClient;
    private readonly IBasketService _basketService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
    {
        _paymentService = paymentService;
        _httpClient = httpClient;
        _basketService = basketService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();

        var responsePayment = await GetPaymentResponse(basket, checkoutInfoInput);

        if (!responsePayment)
        {
            return new OrderCreatedViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
        }

        var orderCreateInput = new OrderCreateInput()
        {
            BuyerId = _sharedIdentityService.GetUserId,
            Address = new AddressCreateInput { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
        };                              

        basket.BasketItems.ForEach(x =>
        {
            var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
            orderCreateInput.OrderItems.Add(orderItem);
        });

        var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

        if (!response.IsSuccessStatusCode)
        {
            return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı", IsSuccessful = false };
        }

        var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

        orderCreatedViewModel.Data.IsSuccessful = true;
        await _basketService.Delete();
        return orderCreatedViewModel.Data;
    }

    public async Task<List<OrderViewModel>> GetOrder()
    {
        var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

        return response.Data;
    }

    public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();
        var orderCreateInput = new OrderCreateInput()
        {
            BuyerId = _sharedIdentityService.GetUserId,
            Address = new AddressCreateInput { Province = checkoutInfoInput.Province, District = checkoutInfoInput.District, Street = checkoutInfoInput.Street, Line = checkoutInfoInput.Line, ZipCode = checkoutInfoInput.ZipCode },
        };

        basket.BasketItems.ForEach(x =>
        {
            var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
            orderCreateInput.OrderItems.Add(orderItem);
        });


        var responsePayment = await GetPaymentResponse(basket, checkoutInfoInput, orderCreateInput);

        if (!responsePayment)
        {
            return new OrderSuspendViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
        }

        await _basketService.Delete();
        return new OrderSuspendViewModel() { IsSuccessful = true };
    }

    private async Task<bool> GetPaymentResponse(BasketViewModel basket, CheckoutInfoInput checkoutInfoInput, OrderCreateInput? orderCreateInput = null)
    {
        var paymentInfoInput = new PaymentInfoInput()
        {
            CardName = checkoutInfoInput.CardName,
            CardNumber = checkoutInfoInput.CardNumber,
            Expiration = checkoutInfoInput.Expiration,
            CVV = checkoutInfoInput.CVV,
            TotalPrice = basket.TotalPrice,
        };

        if (orderCreateInput != null)
            paymentInfoInput.Order = orderCreateInput;

        var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
        return responsePayment;
    }
}
