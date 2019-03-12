using System;
using System.Collections.Generic;
using System.Text;

namespace WalletComponent.Domains
{
    public class UserGroupRelationship
    {
        public Guid Id { get; set; }

        public Guid GroupId { get; set; }
        public virtual Groups Group { get; set; }

        public Guid UserId { get; set; }
        public virtual Users Users { get; set; }

    }
}
