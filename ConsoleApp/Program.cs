using Domain.Models;
using Infrastructure.Repository;

var customersRepository = new CustomersRepository(new Infrastructure.Data.AppDbContext());
var ordersRepository = new OrdersRepository(new Infrastructure.Data.AppDbContext());

while (true)
{
    Console.WriteLine("==============================");
    Console.WriteLine("1. Show all customers");
    Console.WriteLine("2. Show customer by id");
    Console.WriteLine("3. Add new customer");
    Console.WriteLine("4. Update customer");
    Console.WriteLine("5. Delete customer");
    Console.WriteLine("6. Show all orders");
    Console.WriteLine("7. Show order by id");
    Console.WriteLine("8. Add new order");
    Console.WriteLine("9. Update order");
    Console.WriteLine("10. Delete order");
    Console.WriteLine("11. Show orders by customer id");
    Console.WriteLine("12. Show orders with customer names");
    Console.WriteLine("0. Exit");
    Console.WriteLine("==============================");
    Console.Write("Choose an option: ");

    switch (Console.ReadLine())
    {
        case "1":
        {
            var customers = await customersRepository.GetAllCustomersAsync();
            foreach (var customer in customers)
                Console.WriteLine($"Id: {customer.Id}, Full name: {customer.FullName}");
            break;
        }
        case "2":
        {
            Console.Write("Enter customer id: ");
            var customer = await customersRepository.GetCustomerByIdAsync(int.Parse(Console.ReadLine()!));
            Console.WriteLine(customer is null
                ? "Customer not found."
                : $"Id: {customer.Id}, Full name: {customer.FullName}");
            break;
        }
        case "3":
        {
            Console.Write("Enter customer full name: ");
            var fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.WriteLine("Full name cannot be empty.");
                break;
            }

            await customersRepository.AddCustomerAsync(new Customers { FullName = fullName });
            Console.WriteLine("Customer added successfully.");
            break;
        }
        case "4":
        {
            Console.Write("Enter customer id: ");
            var customer = await customersRepository.GetCustomerByIdAsync(int.Parse(Console.ReadLine()!));
            if (customer is null)
            {
                Console.WriteLine("Customer not found.");
                break;
            }

            Console.Write("Enter new full name: ");
            var fullName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(fullName))
            {
                Console.WriteLine("Full name cannot be empty.");
                break;
            }

            customer.FullName = fullName;
            await customersRepository.UpdateCustomerAsync(customer);
            Console.WriteLine("Customer updated successfully.");
            break;
        }
        case "5":
        {
            Console.Write("Enter customer id to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var customer = await customersRepository.GetCustomerByIdAsync(id);
            if (customer is null)
            {
                Console.WriteLine("Customer not found.");
                break;
            }

            await customersRepository.DeleteCustomerAsync(id);
            Console.WriteLine("Customer deleted successfully.");
            break;
        }
        case "6":
        {
            var orders = await ordersRepository.GetOrdersWithCustomerAsync();
            foreach (var order in orders)
                Console.WriteLine($"Id: {order.Id}, Customer: {order.CustomerName}, Amount: {order.Amount}, Date: {order.Created_At}");
            break;
        }
        case "7":
        {
            Console.Write("Enter order id: ");
            var order = await ordersRepository.GetOrderByIdAsync(int.Parse(Console.ReadLine()!));
            Console.WriteLine(order is null
                ? "Order not found."
                : $"Id: {order.Id}, Customer id: {order.Customer_Id}, Amount: {order.Amount}, Date: {order.Created_At}");
            break;
        }
        case "8":
        {
            Console.Write("Enter customer id: ");
            var customerId = int.Parse(Console.ReadLine()!);
            Console.Write("Enter amount: ");
            var amount = double.Parse(Console.ReadLine()!);
            Console.Write("Enter date (yyyy-mm-dd): ");
            var date = DateOnly.Parse(Console.ReadLine()!);

            await ordersRepository.AddOrderAsync(new Orders
            {
                Customer_Id = customerId,
                Amount = amount,
                Created_At = date
            });
            Console.WriteLine("Order added successfully.");
            break;
        }
        case "9":
        {
            Console.Write("Enter order id: ");
            var order = await ordersRepository.GetOrderByIdAsync(int.Parse(Console.ReadLine()!));
            if (order is null)
            {
                Console.WriteLine("Order not found.");
                break;
            }

            Console.Write("Enter new customer id: ");
            order.Customer_Id = int.Parse(Console.ReadLine()!);
            Console.Write("Enter new amount: ");
            order.Amount = double.Parse(Console.ReadLine()!);
            Console.Write("Enter new date (yyyy-mm-dd): ");
            order.Created_At = DateOnly.Parse(Console.ReadLine()!);

            await ordersRepository.UpdateOrderAsync(order);
            Console.WriteLine("Order updated successfully.");
            break;
        }
        case "10":
        {
            Console.Write("Enter order id to delete: ");
            var id = int.Parse(Console.ReadLine()!);
            var order = await ordersRepository.GetOrderByIdAsync(id);
            if (order is null)
            {
                Console.WriteLine("Order not found.");
                break;
            }

            await ordersRepository.DeleteOrderAsync(id);
            Console.WriteLine("Order deleted successfully.");
            break;
        }
        case "11":
        {
            Console.Write("Enter customer id: ");
            var orders = await ordersRepository.GetOrdersByCustomerIdAsync(int.Parse(Console.ReadLine()!));
            foreach (var order in orders)
                Console.WriteLine($"Id: {order.Id}, Amount: {order.Amount}, Date: {order.Created_At}");
            break;
        }
        case "12":
        {
            var orders = await ordersRepository.GetOrdersWithCustomerAsync();
            foreach (var order in orders)
                Console.WriteLine($"Id: {order.Id}, Customer: {order.CustomerName}, Amount: {order.Amount}, Date: {order.Created_At}");
            break;
        }
        case "0":
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option.");
            break;
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
    Console.Clear();
}
