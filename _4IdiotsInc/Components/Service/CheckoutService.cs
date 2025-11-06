using _4IdiotsInc.Model;
using _4IdiotsInc.Repositories;
using _4IdiotsInc.Components.Service;

namespace _4IdiotsInc.Components.Service
{
    public class CheckoutService
    {
        private readonly OrdersRepository _ordersRepo;
        private readonly CartService _cartService;
        private readonly ShippingAddressRepository _shippingRepo;

        public CheckoutService(
            OrdersRepository ordersRepo,
            CartService cartService,
            ShippingAddressRepository shippingRepo)
        {
            _ordersRepo = ordersRepo;
            _cartService = cartService;
            _shippingRepo = shippingRepo;
        }

        public async Task<(bool Success, string Message, int? OrderId)> ProcessCheckoutAsync(
            string userId,
            UserShippingAddress shippingAddress,
            string paymentMethod, // Added parameter
            bool saveAddress = false)
        {
            try
            {
                // Save shipping address if requested
                int? shippingAddressId = null;
                if (saveAddress)
                {
                    var savedAddress = await _shippingRepo.SaveAddressAsync(shippingAddress);
                    shippingAddressId = savedAddress.Id;
                }
                else if (shippingAddress.Id > 0)
                {
                    // Use existing saved address
                    shippingAddressId = shippingAddress.Id;
                }

                // Get cart items
                var cartItems = _cartService.GetCartItemsForCheckout();

                if (!cartItems.Any())
                {
                    return (false, "Cart is empty", null);
                }

                // Convert CartItems to OrderItems
                var orderItems = cartItems.Select(ci => new OrderItems
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Price
                }).ToList();

                // Create order with "Pending" status and payment method
                var order = await _ordersRepo.CreateOrderWithItemsAsync(
                    userId: userId,
                    items: orderItems,
                    statusId: 1, // Pending status
                    shippingAddressId: shippingAddressId,
                    paymentMethod: paymentMethod // Pass payment method here
                );

                // Clear cart after successful order
                _cartService.CompleteCheckout();

                return (true, $"Order #{order.Id} placed successfully!", order.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Checkout error: {ex.Message}");
                return (false, $"Checkout failed: {ex.Message}", null);
            }
        }
    }
}