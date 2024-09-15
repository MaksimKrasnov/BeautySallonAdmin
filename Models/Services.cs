namespace BeautySaloon.Models
{
    public class Services
    {
        public Services(int id, string name)
        {
            Id = id;
            Name = name;
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public Service Service { get; set; }
    }
}

