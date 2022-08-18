namespace MyCarAssistant.Models
{
    public class Record
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int AutoID { get; set; }
        //добавить статус галочку для разделения старых записей и новых + дополнить данные для таблицы
    }
}
