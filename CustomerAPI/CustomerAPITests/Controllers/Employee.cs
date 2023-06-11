namespace EcommerceAPI.Tests
{
    internal class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? City { get; set; }

        public string GetName(string firstName, string lastName)
        {
            return string.Concat(firstName, " ", lastName);
        }
    }
}