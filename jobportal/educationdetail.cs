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
    
    public partial class educationdetail
    {
        public System.Guid PK_EducationDetailsID { get; set; }
        public Nullable<System.Guid> FK_MemberID { get; set; }
        public string InstituteName { get; set; }
        public string Degree { get; set; }
        public Nullable<System.DateTime> TimePeriod_From { get; set; }
        public Nullable<System.DateTime> TimePeriod_To { get; set; }
        public string Ed_Description { get; set; }
    
        public virtual careerdetail careerdetail { get; set; }
    }
}