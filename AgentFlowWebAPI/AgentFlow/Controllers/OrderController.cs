namespace AgentFlow.Controllers;

[ApiController]
[Route("orders")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
    {
        var order = _orderService.CreateOrder(request.TotalAmount);
        return Ok(order);
    }
}

public record CreateOrderRequest(
    [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
    decimal TotalAmount
);
