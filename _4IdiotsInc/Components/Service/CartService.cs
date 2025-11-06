namespace _4IdiotsInc.Components.Service
{
    public class CartService
    {
        private List<CartItem> _cartItems = new();
        private bool _isCartOpen = false;

        public event Action? OnCartChanged;
        public event Action<bool>? OnCartToggled;
        public event Action? OnCheckoutCompleted; // New event

        public int ItemCount => _cartItems.Sum(i => i.Quantity);
        public decimal TotalPrice => _cartItems.Sum(i => i.Price * i.Quantity);
        public List<CartItem> CartItems => _cartItems;
        public bool IsCartOpen => _isCartOpen;

        public void AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1)
        {
            var existing = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _cartItems.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = quantity
                });
            }

            OnCartChanged?.Invoke();
        }

        public void RemoveFromCart(int productId)
        {
            var item = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                _cartItems.Remove(item);
                OnCartChanged?.Invoke();
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = _cartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveFromCart(productId);
                }
                else
                {
                    item.Quantity = quantity;
                    OnCartChanged?.Invoke();
                }
            }
        }

        public void ToggleCart()
        {
            _isCartOpen = !_isCartOpen;
            OnCartToggled?.Invoke(_isCartOpen);
        }

        public void OpenCart()
        {
            if (!_isCartOpen)
            {
                _isCartOpen = true;
                OnCartToggled?.Invoke(_isCartOpen);
            }
        }

        public void CloseCart()
        {
            if (_isCartOpen)
            {
                _isCartOpen = false;
                OnCartToggled?.Invoke(_isCartOpen);
            }
        }

        public void ClearCart()
        {
            _cartItems.Clear();
            OnCartChanged?.Invoke();
        }

        // New method to get order items for checkout
        public List<CartItem> GetCartItemsForCheckout()
        {
            return _cartItems.ToList(); // Return a copy
        }

        // New method to complete checkout
        public void CompleteCheckout()
        {
            ClearCart();
            CloseCart();
            OnCheckoutCompleted?.Invoke();
        }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}