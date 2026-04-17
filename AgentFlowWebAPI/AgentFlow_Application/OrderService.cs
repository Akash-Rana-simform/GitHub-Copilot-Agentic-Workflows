namespace AgentFlow_Application;

public class OrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public Order CreateOrder(decimal totalAmount)
    {
        if (totalAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalAmount),
                totalAmount,
                "Total amount must be greater than zero.");
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            TotalAmount = totalAmount
        };
        _repository.Add(order);
        return order;
    }
}
