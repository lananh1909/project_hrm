using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Core.Entities
{
    public class UserInfo : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
