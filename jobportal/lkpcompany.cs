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
    
    public partial class lkpcompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lkpcompany()
        {
            this.jobdetails = new HashSet<jobdetail>();
        }
    
        public System.Guid PK_CompanyID { get; set; }
        public string companyname { get; set; }
        public string companytype { get; set; }
        public string emailid { get; set; }
        public string phonenumber { get; set; }
        public string address { get; set; }
        public Nullable<bool> isactive { get; set; }
        public string createdby { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<jobdetail> jobdetails { get; set; }
    }
}