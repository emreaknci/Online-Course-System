using OnlineCourse.Web.Models.Payment;

namespace OnlineCourse.Web.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
}

