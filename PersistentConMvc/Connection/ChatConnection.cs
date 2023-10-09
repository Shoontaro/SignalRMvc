using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNet.SignalR;
using PersistentConMvc.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PersistentConMvc.Connection
{
    public class ChatConnection : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            Data chatData = new Data() { Name = "Сообщение сервера", Message = "Пользователь " + connectionId + " присоединился к чату" };
            return Connection.Broadcast(chatData);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            Data chatData = JsonConvert.DeserializeObject<Data>(data);
            return Connection.Broadcast(chatData);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            Data chatData = new Data() { Name = "Сообщение сервера", Message = "Пользователь " + connectionId + " покинул чат" };
            return Connection.Broadcast(chatData);
        }
    }
}