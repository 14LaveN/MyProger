using Microsoft.AspNetCore.SignalR;
using MyProger.Core.Entity.Job;

namespace MyProger.Micro.JobListAPI.SignalR.Hubs;

public class JobListHub : Hub
{
    //? public async Task AddItemToCart(string productId) --> SignalR 
    //? { --> SignalR 
    //?     // Добавить товар в корзину пользователя --> SignalR 
    //?     var cart = await GetCurrentCart(); --> SignalR 
    //?     cart.Items.Add(new CartItem { ProductId = productId }); --> SignalR 
//?  --> SignalR 
    //?     // Отправить сообщение всем пользователям, просматривающим страницу корзины покупок --> SignalR 
    //?     await Clients.All.SendAsync("CartUpdated", cart); --> SignalR 
    //? } --> SignalR 
//?  --> SignalR 
    //? public async Task RemoveItemFromCart(string productId) --> SignalR 
    //? { --> SignalR 
    //?     // Удалить товар из корзины пользователя --> SignalR 
    //?     var cart = await GetCurrentCart(); --> SignalR 
    //?     cart.Items.Remove(cart.Items.FirstOrDefault(x => x.ProductId == productId)); --> SignalR 
//?  --> SignalR 
    //?     // Отправить сообщение всем пользователям, просматривающим страницу корзины покупок --> SignalR 
    //?     await Clients.All.SendAsync("CartUpdated", cart);
    //? }
} 