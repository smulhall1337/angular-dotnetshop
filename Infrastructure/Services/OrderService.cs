using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepo;
    private readonly IUnitOfWork _uow;

    public OrderService(IBasketRepository basketRepo, IUnitOfWork uow)
    {
        _basketRepo = basketRepo;
        _uow = uow;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // get basket from repo 
        CustomerBasket basket = await _basketRepo.GetBasketAsync(basketId);
        
        // get items from product repo
        List<OrderItem> items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            Product productItem = await _uow.Repository<Product>().GetByIdAsync(item.Id);
            ProductItemOrdered itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            OrderItem orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }
        
        // get delivery method
        DeliveryMethod deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        
        // calculate subtotal
        decimal subtotal = items.Sum(item => item.Price * item.Quantity);
        
        // create order 
        Order order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);
        _uow.Repository<Order>().Add(order);
        order.PaymentIntentId = "0";
        
        // save to DB
        int result = await _uow.Complete();

        if (result <= 0)
        {
            // nothing was saved
            return null;
        }
        
        // delete basket 
        await _basketRepo.DeleteBasketAsync(basketId);
        
        // return order
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _uow.Repository<Order>().ListAsync(spec);
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _uow.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
    {
        return await _uow.Repository<DeliveryMethod>().ListAllAsync();
    }
}