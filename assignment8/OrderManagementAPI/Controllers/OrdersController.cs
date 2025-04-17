using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data;
using OrderManagementAPI.Models;

namespace OrderManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrdersController : ControllerBase
	{
		private readonly OrderContext _context;

		public OrdersController(OrderContext context)
		{
			_context = context;
		}

		// ��ȡ���ж���
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
		{
			return await _context.Orders
				.Include(o => o.Customer)
				.Include(o => o.Details)
				.ThenInclude(d => d.Product)
				.ToListAsync();
		}

		// ���ݶ��� ID ��ȡ����
		[HttpGet("{id}")]
		public async Task<ActionResult<Order>> GetOrder(int id)
		{
			var order = await _context.Orders
				.Include(o => o.Customer)
				.Include(o => o.Details)
				.ThenInclude(d => d.Product)
				.FirstOrDefaultAsync(o => o.OrderId == id);

			if (order == null)
			{
				return NotFound();
			}

			return order;
		}

		// �����¶���
		[HttpPost]
		public async Task<ActionResult<Order>> CreateOrder(Order order)
		{
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
		}

		// ���¶���
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrder(int id, Order order)
		{
			if (id != order.OrderId)
			{
				return BadRequest();
			}

			_context.Entry(order).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// ɾ������
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteOrder(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				return NotFound();
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.OrderId == id);
		}
	}
}