using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing
{
    // Класс OrderResult представляет результат обработки заказа
    public class OrderResult
    {
        // Свойство, указывающее на успешность обработки заказа
        public bool Success { get; }

        // Свойство, содержащее сообщение о результате обработки заказа
        public string Message { get; }

        /// Конструктор класса, который инициализирует свойства Success и Message
        /// </summary>
        /// <param name="success">Указывает, был ли заказ успешно обработан (true) или нет (false)</param>
        /// <param name="message">Сообщение, содержащее дополнительную информацию о результате обработки заказа</param>
        public OrderResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
