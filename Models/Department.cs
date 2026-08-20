namespace EmploymentManagmentSystem.Models
{
    public class Department : IHasId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Department(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}