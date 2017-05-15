using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy.Engine
{
    class Methods
    {
        public static void Subscribe(string tag, string id)
        {
            
        }

        public static void Unsubscribe(string tag, string id)
        {
            
        }

        public static void RequestModule(string ip, int port)
        {
            // TODO: Сгенерировать уникальный идентификатор и отправить модулю с запросом на подтверждение
        }

        public static void RegisterModule()
        {
            // TODO: Зарегистрировать модуль во внутренней базе данных
            // Добавить автоподписку на сообщения по своему Uid
            // Добавить название модуля 
            // Добавить IP адрес и порт 
            // Записать данные в локальную базу
        }

        public static string[] GetModulesList()
        {
            // TODO: return массив uid
            return new string[10];
        }

        public static string[] GetAppList()
        {    
            return new string[10];
        }
    }
}
