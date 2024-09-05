namespace AccountSystem.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public List<Incident> Incidents { get; set; } = new List<Incident>();
    }
}
