using System;
using UAC.LMS.Models;

namespace UAC.LMS.DAL
{
    // comment here 

    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<LMSLogin> LMSLoginRepository { get; }
        IGenericRepository<LMSDepartment> LMSDepartmentRepository { get; }
        IGenericRepository<LMSJobTitle> LMSJobTitleRepository { get; }
        IGenericRepository<LMSCourse> LMSCourseRepository { get; }
        IGenericRepository<LMSBusinessUnit> LMSBusinessUnitRepository { get; }

        IGenericRepository<LMSEmployee> LMSEmployeeRepository { get; }
        IGenericRepository<LMSEmployeeCourse> LMSEmployeeCourseRepository { get; }
        IGenericRepository<LMSSecurityQuestion> LMSSecurityQuestionRepository { get; }
        IGenericRepository<LMSUserSecurityAnswer> LMSUserSecurityAnswerRepository { get; }
        IGenericRepository<LMSJobTitleCourse> LMSJobTitleCourseRepository { get; }
        //11/11/2016
        IGenericRepository<LMSAudit> LMSAuditRepository { get; }
        IGenericRepository<LMSCourseHistory> LMSCourseHistoryRepository { get; }
        void Save();
    }

    public partial class UnitOfWork : IUnitOfWork
    {
        private IGenericRepository<LMSDepartment> _lmsDepartmentRepository;
        private IGenericRepository<LMSLogin> _lmsLoginRepository;
        private IGenericRepository<LMSJobTitle> _lmsJobTitleRepository;
        private IGenericRepository<LMSCourse> _lmsCourseRepository;
        private IGenericRepository<LMSBusinessUnit> _lmsBusinessUnitRepository;
        private IGenericRepository<LMSEmployee> _lmsEmployeeRepository;
        private IGenericRepository<LMSEmployeeCourse> _lmsEmployeeCourseRepository;
        private IGenericRepository<LMSSecurityQuestion> _lmsSecurityQuestionRepository;
        private IGenericRepository<LMSUserSecurityAnswer> _lmsSecurityAnswerRepository;
        //11/11/2016
        private IGenericRepository<LMSAudit> _lmsAuditRepository;
        private IGenericRepository<LMSJobTitleCourse> _lmsJobTitleCourseRepository;
        private IGenericRepository<LMSCourseHistory> _lmsCourseHistoryRepository;

        private LMSDBContext _context;

        //11/11/2016
        public IGenericRepository<LMSAudit> LMSAuditRepository
        {
            get
            {
                if (_lmsAuditRepository == null)
                    _lmsAuditRepository = new GenericRepository<LMSAudit>(_context);

                return _lmsAuditRepository;
            }
        }
        public IGenericRepository<LMSDepartment> LMSDepartmentRepository
        {
            get
            {
                if (_lmsDepartmentRepository == null)
                    _lmsDepartmentRepository = new GenericRepository<LMSDepartment>(_context);

                return _lmsDepartmentRepository;
            }
        }

        public IGenericRepository<LMSLogin> LMSLoginRepository
        {
            get
            {
                if (_lmsLoginRepository == null)
                    _lmsLoginRepository = new GenericRepository<LMSLogin>(_context);

                return _lmsLoginRepository;
            }
        }

        public IGenericRepository<LMSJobTitle> LMSJobTitleRepository
        {
            get
            {
                if (_lmsJobTitleRepository == null)
                    _lmsJobTitleRepository = new GenericRepository<LMSJobTitle>(_context);

                return _lmsJobTitleRepository;
            }
        }

        public IGenericRepository<LMSJobTitleCourse> LMSJobTitleCourseRepository
        {
            get
            {
                if (_lmsJobTitleCourseRepository == null)
                    _lmsJobTitleCourseRepository = new GenericRepository<LMSJobTitleCourse>(_context);

                return _lmsJobTitleCourseRepository;
            }
        }

        public IGenericRepository<LMSCourse> LMSCourseRepository
        {
            get
            {
                if (_lmsCourseRepository == null)
                    _lmsCourseRepository = new GenericRepository<LMSCourse>(_context);

                return _lmsCourseRepository;
            }
        }

        public IGenericRepository<LMSBusinessUnit> LMSBusinessUnitRepository
        {
            get
            {
                if (_lmsBusinessUnitRepository == null)
                    _lmsBusinessUnitRepository = new GenericRepository<LMSBusinessUnit>(_context);

                return _lmsBusinessUnitRepository;
            }
        }

        public IGenericRepository<LMSEmployee> LMSEmployeeRepository
        {
            get
            {
                if (_lmsEmployeeRepository == null)
                    _lmsEmployeeRepository = new GenericRepository<LMSEmployee>(_context);

                return _lmsEmployeeRepository;
            }
        }

        public IGenericRepository<LMSEmployeeCourse> LMSEmployeeCourseRepository
        {
            get
            {
                if (_lmsEmployeeCourseRepository == null)
                    _lmsEmployeeCourseRepository = new GenericRepository<LMSEmployeeCourse>(_context);

                return _lmsEmployeeCourseRepository;
            }
        }

        public IGenericRepository<LMSSecurityQuestion> LMSSecurityQuestionRepository
        {
            get
            {
                if (_lmsSecurityQuestionRepository == null)
                    _lmsSecurityQuestionRepository = new GenericRepository<LMSSecurityQuestion>(_context);

                return _lmsSecurityQuestionRepository;
            }
        }

        public IGenericRepository<LMSUserSecurityAnswer> LMSUserSecurityAnswerRepository
        {
            get
            {
                if (_lmsSecurityAnswerRepository == null)
                    _lmsSecurityAnswerRepository = new GenericRepository<LMSUserSecurityAnswer>(_context);

                return _lmsSecurityAnswerRepository;
            }
        }

        public IGenericRepository<LMSCourseHistory> LMSCourseHistoryRepository
        {
            get
            {
                if (_lmsCourseHistoryRepository == null)
                    _lmsCourseHistoryRepository = new GenericRepository<LMSCourseHistory>(_context);

                return _lmsCourseHistoryRepository;
            }
        }

        #region Common Methods
        public UnitOfWork()
        {
            _context = new LMSDBContext();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
