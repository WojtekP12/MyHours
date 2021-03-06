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
    
    public partial class TEACHER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TEACHER()
        {
            this.FACULTY_ASSIGNMENT = new HashSet<FACULTY_ASSIGNMENT>();
            this.SUBJECT_ASSIGNMENT = new HashSet<SUBJECT_ASSIGNMENT>();
            this.SUBJECT_ASSIGNMENT_TEMP = new HashSet<SUBJECT_ASSIGNMENT_TEMP>();
            this.USER = new HashSet<USER>();
        }
    
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string TeacherStatus { get; set; }
        public int AssignedHours { get; set; }
        public string FullName { get; set; }
        public Nullable<int> FacultyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FACULTY_ASSIGNMENT> FACULTY_ASSIGNMENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_ASSIGNMENT> SUBJECT_ASSIGNMENT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUBJECT_ASSIGNMENT_TEMP> SUBJECT_ASSIGNMENT_TEMP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER> USER { get; set; }
        public virtual FACULTY FACULTY { get; set; }
    }
}
