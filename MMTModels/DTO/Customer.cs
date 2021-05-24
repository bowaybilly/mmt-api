using System.ComponentModel.DataAnnotations;

namespace MMTModels
{

    public class Customer
    {
        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }
        public string customerId { get; set; }
        public bool website { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LastLoggedIn { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
        public string PreferredLanguage { get; set; }
    }
    public class CustomerOrderResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
       
    }
    public class CustomerOrderRequestDTO
    {
        
        public string Email { get; set; }
        public string customerId { get; set; }
    }

}
