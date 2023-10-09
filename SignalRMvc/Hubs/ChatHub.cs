using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRMvc.Models;

namespace SignalRMvc.Hubs
{
    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();

        // Отправка сообщений
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message); //Вызов метода на всех клиентах

            /*Вызов метода только на текущем клиенте, который обратился к серверу: Clients.Caller.addMessage(name, message);
             * Вызов метода на всех клиентах, кроме того, который обратился к серверу: Clients.Others.addMessage(name, message);
             * Вызов метода только у клиента с определенным id: Clients.Client(Context.ConnectionId).addMessage(name, message);
             * Вызов метода на всех клиентах, кроме клиента с определенным id: Clients.AllExcept(connectionId).addMessage(name, message);
             * Вызов метода на всех клиентах указанной группы: Clients.Group(groupName).addMessage(name, message);
             * Вызов метода на всех клиентах указанной группы, за исключением клиента, у которого id - connectionId: Clients.Group(groupName, connectionId).addMessage(name, message);
             * Вызов метода на всех клиентах указанной группы, за исключением обратившегося к серверу клиента: Clients.OthersInGroup(groupName).addMessage(name, message);*/
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}

