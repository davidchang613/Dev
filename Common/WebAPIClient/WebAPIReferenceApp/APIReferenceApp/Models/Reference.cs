using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIReferenceAppPortable.Models
{
    public class Reference
    {
        public System.Guid id { get; set; }
        public Nullable<System.Guid> parent_id { get; set; }
        public System.Guid reference_type_id { get; set; }
        public string short_value { get; set; }
        public Nullable<int> order { get; set; }
        public string default_value { get; set; }
        public string created_by { get; set; }
        public string modified_by { get; set; }
        public Nullable<System.DateTime> modified_date { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<System.Guid> account_id { get; set; }
        public Nullable<System.Guid> account_group_id { get; set; }
        public Nullable<System.Guid> program_type_id { get; set; }
        public Nullable<System.Guid> override_id { get; set; }
    }
}
