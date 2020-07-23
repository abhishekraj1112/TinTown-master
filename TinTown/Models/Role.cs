using System.Collections.Generic;

namespace TinTown.Models
{
    public class Role
    {
        public int roleId { get; set; }
        public string roleName { get; set; }
        public string userId { get; set; }
        public string flag { get; set; }
    }

    public class RoleReturn
    {
        public string condition { get; set; }
        public string message { get; set; }
        public int roleId { get; set; }
        public string roleName { get; set; }
    }

    public class NewRole
    {
        public int RoleId { get; set; }
        public List<int> PageId { get; set; }
    }
}
