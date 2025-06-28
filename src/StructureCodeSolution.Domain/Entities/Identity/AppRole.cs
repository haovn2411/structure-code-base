using Microsoft.AspNetCore.Identity;
using StructureCodeSolution.Domain.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>, IAuditable
    {
        public string Description { get; set; }
        public string RoleCode { get; set; }
        public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }
        public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
