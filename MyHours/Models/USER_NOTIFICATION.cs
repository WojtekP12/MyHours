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
    
    public partial class USER_NOTIFICATION
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StatusID { get; set; }
        public System.DateTime Date { get; set; }
        public int SubjectAssignmentID { get; set; }
    
        public virtual NOTIFICATION_STATUS NOTIFICATION_STATUS { get; set; }
        public virtual SUBJECT_ASSIGNMENT_TEMP SUBJECT_ASSIGNMENT_TEMP { get; set; }
        public virtual USER USER { get; set; }
    }
}
