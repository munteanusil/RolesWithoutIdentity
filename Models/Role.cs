using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RolesWithoutIdentity.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
