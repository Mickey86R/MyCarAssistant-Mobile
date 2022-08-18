using System;

namespace MyCarAssistant.Models
{
    public class T_O
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Record { get; set; }
        public DateTime Date { get; set; }
        public int Mileage { get; set; }
        public int? Sum { get; set; }

        public int AutoID { get; set; }
        //добавить статус галочку для разделения старых записей и новых + дополнить данные для таблицы
        //+ ДОБАВИТЬ КАТЕГОРИИ РЕМОНТА КАК И В ЗАТРАТАХ. СОРТИРОВКОЙ ПО НАЗВАНИЮ КАТЕГОРИИ (ТО, СЕРВИСНОЕ ОБСЛУЖИВАНИЕ, САМОСТОТЕЛЬНОЕ ОБСЛУЖИВАНИЕ)
    }
}
