namespace AgentFlow_Infrastructure;

public class OrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _store = new();

    public void Add(Order order) => _store[order.Id] = order;

    public Order? GetById(Guid id) => _store.TryGetValue(id, out var order) ? order : null;
}
