//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyHours.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class STUDENT_GROUP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public STUDENT_GROUP()
        {
            this.FACULTY_ASSIGNMENT = new HashSet<FACULTY_ASSIGNMENT>();
            this.SUBJECT_ASSIGNMENT = new HashSet<SUBJECT_ASSIGNMENT>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACULTY_ASSIGNMENT> FACULTY_ASSIGNMENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_ASSIGNMENT> SUBJECT_ASSIGNMENT { get; set; }
    }
}