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
    
    public partial class careerdetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public careerdetail()
        {
            this.educationdetails = new HashSet<educationdetail>();
            this.languageproficiencies = new HashSet<languageproficiency>();
            this.personaldetails = new HashSet<personaldetail>();
            this.workhistories = new HashSet<workhistory>();
        }
    
        public System.Guid PK_MemberID { get; set; }
        public string FullName { get; set; }
        public string AdditionalInformation { get; set; }
        public string CareerObjective { get; set; }
        public string SpecialQualification { get; set; }
        public string Declaration { get; set; }
        public string Resume { get; set; }
        public string createdby { get; set; }
        public Nullable<System.DateTime> createdon { get; set; }
        public Nullable<bool> EmailAlert { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<educationdetail> educationdetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<languageproficiency> languageproficiencies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<personaldetail> personaldetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<workhistory> workhistories { get; set; }
    }
}