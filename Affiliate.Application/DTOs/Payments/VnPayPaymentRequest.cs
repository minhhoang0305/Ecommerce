public record VnPayPaymentRequest(
    int OrderId,
    decimal Amount,
    string OrderInfo,
    string ReturnUrl,
    string IpAddress);
