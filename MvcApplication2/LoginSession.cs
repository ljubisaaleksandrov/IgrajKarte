//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IgrajKarte
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoginSession
    {
        public LoginSession()
        {
            this.Logs = new HashSet<Log>();
        }
    
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
    
        public virtual ICollection<Log> Logs { get; set; }
        public virtual LoginSession LoginSessions1 { get; set; }
        public virtual LoginSession LoginSession1 { get; set; }
        public virtual User User { get; set; }
    }
}
