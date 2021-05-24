using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMTModels
{

    public class UserDTO
    {
        [MaxLength(75)]
        public string user { get; set; }
        [MaxLength(7)]
        public string customerId { get; set; }
    }

    
}
