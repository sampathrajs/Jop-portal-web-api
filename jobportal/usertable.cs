//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace jobportal
{
    using System;
    using System.Collections.Generic;
    
    public partial class usertable
    {
        public System.Guid PK_UserID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string emailid { get; set; }
        public string phonenumber { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<System.DateTime> lastlogin { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<bool> isadmin { get; set; }
    }
}
