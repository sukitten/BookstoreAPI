using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using YourNamespace.Models;
using YourNamespace.Data;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public OrdersController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return _dataContext.Orders.ToList();
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal error has occurred."); 
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(int id)
        {
            try
            {
                var order = _dataContext.Orders.Find(id);

                if (order == null)
                {
                    return NotFound();
                }

                return order;
            }
            catch (Exception)
            {
                return StatusCode(500, "An internal server error has occurred.");
            }
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(Order order)
        {
            try
            {
                _dataContext.Orders.Add(order);
                _dataContext.SaveChanges();

                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _data.isDirectory(order).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                _dataContext.SaveChanges();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!_dataContext.Orders.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the order.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error has occurred.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Order> DeleteOrderById(int id)
        {
            try
            {
                var orderToDelete = _dataContext.Orders.Find(id);
                if (orderToDelete == null)
                {
                    return NotFound();
                }

                _dataContext.Orders.Remove(orderToDelete);
                _dataContext.SaveChanges();

                return orderToDelete;
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }
    }
}