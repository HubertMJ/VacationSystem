namespace VacationSystem.Models
{
    public class Worker
    {
        public Worker(){}
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int VacationDays { get; set; }
        public int PermissionLevel { get; set; }
    }
}