namespace AgentFlow_Application;

public interface IOrderRepository
{
    void Add(Order order);
    Order? GetById(Guid id);
}
