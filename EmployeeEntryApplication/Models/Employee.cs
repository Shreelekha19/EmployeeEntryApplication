using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeEntryApplication.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("City Name")]
        public string? CityName { get; set; }

        [DisplayName("Joining Date")]
        [DataType(DataType.Date)]
        public DateTime YearOfJoining { get; set; }
    }
}
