﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TAM_DBEntities : DbContext
    {
        public TAM_DBEntities()
            : base("name=TAM_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<FACULTY> FACULTY { get; set; }
        public virtual DbSet<FACULTY_ASSIGNMENT> FACULTY_ASSIGNMENT { get; set; }
        public virtual DbSet<MAJOR> MAJOR { get; set; }
        public virtual DbSet<NOTIFICATION_STATUS> NOTIFICATION_STATUS { get; set; }
        public virtual DbSet<SPECIALITY> SPECIALITY { get; set; }
        public virtual DbSet<SPECIALITY_MAJOR_ASSIGNMENT> SPECIALITY_MAJOR_ASSIGNMENT { get; set; }
        public virtual DbSet<STUDENT_GROUP> STUDENT_GROUP { get; set; }
        public virtual DbSet<STUDIES_TYPE_DICT> STUDIES_TYPE_DICT { get; set; }
        public virtual DbSet<SUBJECT> SUBJECT { get; set; }
        public virtual DbSet<SUBJECT_ASSIGNMENT> SUBJECT_ASSIGNMENT { get; set; }
        public virtual DbSet<SUBJECT_ASSIGNMENT_TEMP> SUBJECT_ASSIGNMENT_TEMP { get; set; }
        public virtual DbSet<SUBJECT_TYPE_DICT> SUBJECT_TYPE_DICT { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TEACHER> TEACHER { get; set; }
        public virtual DbSet<USER> USER { get; set; }
        public virtual DbSet<USER_NOTIFICATION> USER_NOTIFICATION { get; set; }
        public virtual DbSet<USER_TYPE> USER_TYPE { get; set; }
    }
}