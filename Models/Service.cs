namespace BeautySaloon.Models
{
    public class Service
    {
        public Service() { }
        public Service(int id, string name)
        {
            Id = id;
            Name = name;
           
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public virtual int ServicesId { get; set; }
        public virtual Services Services { get; set; }
		public virtual ICollection<MasterService> MasterServices { get; set; }

    }
}

