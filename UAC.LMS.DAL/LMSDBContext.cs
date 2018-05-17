using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using UAC.LMS.Models;

namespace UAC.LMS.DAL
{
    public class LMSDBContext : DbContext
    {
        // comment here 

        public LMSDBContext() : base("LMS")
        {
            // Use the code below During first time database creation
            //Comment after Database creation
            Database.SetInitializer(new LMSDBInitializer());
            
            // Use the code below After first time database creation
            // Comment - During first time database creation

          //  Database.SetInitializer<LMSDBContext>(null);
           //----------------
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<LMSStatusCodeDetail> StatusCodeDetails { get; set; }
        public DbSet<LMSDepartment> LMSDepartments { get; set; }
        public DbSet<LMSAudit> LMSAudits { get; set; }
        public DbSet<LMSBusinessUnit> LMSBusinessUnit { get; set; }
        public DbSet<LMSCourse> LMSCourses { get; set; }
        public DbSet<LMSEmployee> LMSEmployees { get; set; }
        public DbSet<LMSEmployeeCourse> LMSEmployeeCourses { get; set; }
        public DbSet<LMSJobTitle> LMSJobTitles { get; set; }
        public DbSet<LMSLogin> LMSLogins { get; set; }
        public DbSet<LMSSecurityQuestion> LMSSecurityQuestions { get; set; }
        public DbSet<LMSUserSecurityAnswer> LMSUserSecurityAnswers { get; set; }
        //public DbSet<TestModel1> TestModel1 { get; set; }
        public DbSet<LMSJobTitleCourse> LMSJobTitleCourses { get; set; }
        public DbSet<LMSCourseHistory> LMSCourseHistories { get; set; }
        public DbSet<LMSJobTitleCourse> LMSDepartmentCourses { get; set; }
        public override int SaveChanges()
        {
            try
            {
                //this.SetAuditValues();
                return base.SaveChanges();
            }
            catch (DbUpdateException dbex)
            {
                throw dbex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetAuditValues()
        {
            var addedEntries = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            //var currentUser = this.GetCurrentUser();
            var currentUser = 0;

            foreach (var addedEntry in addedEntries)
            {
                var entity = addedEntry.Entity as BaseEntity;
                if (entity != null)
                {
                    entity.CreateDateTime = DateTime.Now;
                    entity.CreateUser = currentUser;
                    entity.ModDateTime = DateTime.Now;
                    entity.ModUser = currentUser;
                    
                }

            }

            var modifiedEntries = this.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var modEntry in modifiedEntries)
            {
                var entity = modEntry.Entity as BaseEntity;
                if (entity != null)
                {
                    entity.ModDateTime = DateTime.Now;
                    entity.ModUser = currentUser;
                }
            }
        }
    }
}
