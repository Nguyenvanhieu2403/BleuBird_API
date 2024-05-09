using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueBird.DataConext.Data
{
    public class Users
    {
        public Guid Id { get; set; }
        public virtual string First_Name { get; set; }
        public virtual string Last_Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PassWordHas { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Avata { get; set; }
        public int UsedState { get; set; }
        public string Description { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set;}
        public string Roles { get; set; }
    }

    public class UserShow : Users
    {
        public string FirstName;
        public string LastName;

        public override string First_Name
        {
            get { return FirstName; }
            set { FirstName = value; }
        }

        public override string Last_Name
        {
            get { return LastName; }
            set { LastName = value; }
        }
    }
}
