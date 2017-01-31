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
    
    public partial class SUBJECT_ASSIGNMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUBJECT_ASSIGNMENT()
        {
            this.USER_NOTIFICATION = new HashSet<USER_NOTIFICATION>();
        }
    
        public int ID { get; set; }
        public int Hours { get; set; }
        public int Semester { get; set; }
        public int TeacherID { get; set; }
        public bool IsSubstitute { get; set; }
        public string IsSubstituteDescription { get; set; }
        public int StudentGroupID { get; set; }
        public int SubjectID { get; set; }
        public int SubjectTypeID { get; set; }
        public int StudiesTypeID { get; set; }
        public Nullable<int> ProxyID { get; set; }
    
        public virtual STUDENT_GROUP STUDENT_GROUP { get; set; }
        public virtual STUDIES_TYPE_DICT STUDIES_TYPE_DICT { get; set; }
        public virtual SUBJECT SUBJECT { get; set; }
        public virtual SUBJECT_TYPE_DICT SUBJECT_TYPE_DICT { get; set; }
        public virtual TEACHER TEACHER { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_NOTIFICATION> USER_NOTIFICATION { get; set; }
    }
}
