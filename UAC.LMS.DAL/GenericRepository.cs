using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using UAC.LMS.Common.Constants;
using UAC.LMS.Common.Utilities;
using UAC.LMS.Models;

namespace UAC.LMS.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // comment here 

        /// <summary>
        /// 
        /// </summary>
        internal LMSDBContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(LMSDBContext context)
        {
            // comment here             
            //sets the dbset
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            // comment here 

            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            // comment here 
            // return the entity from the database by - id

            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            // comment here 
            // Adds the Entity in the database table

            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            // comment here 
            // finds the entity by id and deletes from the database table

            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            // comment here 
            // deletes the Entity from the database table

            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            // comment here
            // inserts the modified entity
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Register a new user login
        /// </summary>
        public virtual void Register(LMSLogin model)
        {            
            string strCurrentDate = DateTime.Now.ToString();
            byte[] passwordSalt = Encryptor.EncryptText(strCurrentDate, model.UserName);
            string se = Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = Encryptor.GenerateHash(model.Password, se.ToString());
            var data = new LMSLogin
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //commented on 11/14/2016
               // EmployeeNo = model.EmployeeNo,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PermissionLevel = model.PermissionLevel,
                UserType = model.UserType,
                IssueDate = model.IssueDate,
                CreatedBy = model.CreatedBy,
                CreatedOn = model.CreatedOn,
                StatusCode = model.StatusCode,
            };
            this.context.LMSLogins.Add(data);
        }

        //comment here 
        /// <summary>
        /// Store the changed passwrod in encrypted format
        /// </summary>
        /// <param name="model"></param>
        public virtual void ChangePassword(LMSLogin model)
        {
            string strCurrentDate = DateTime.Now.ToString();
            byte[] passwordSalt = Encryptor.EncryptText(strCurrentDate, model.UserName);
            string se = Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = Encryptor.GenerateHash(model.Password, se.ToString());
            var login = context.LMSLogins.SingleOrDefault(x => x.UserId == model.UserId);
            if (login != null)
            {
                login.IsSecurityApplied = true;
                login.PasswordHash = passwordHash;
                login.PasswordSalt = passwordSalt;
                login.LastModifiedBy = model.UserId;
                login.LastModifiedOn = DateTime.Now;
            }
        }

        /// <summary>
        /// Checks if the password matches the stored encrypted password 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public LMSLogin CheckPassword(LMSLogin model)
        {
            LMSLogin lMSLogin = null;
            if (model != null && !string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
            {
                var dbUser = this.context.LMSLogins.SingleOrDefault(x => x.UserName == model.UserName);
                byte[] strSalt = dbUser.PasswordSalt;
                string salt = Convert.ToBase64String(strSalt);
                byte[] dbPasswordHash = dbUser.PasswordHash;
                byte[] userPasswordHash = Encryptor.GenerateHash(model.Password, salt);
                bool chkPassword = Encryptor.CompareByteArray(dbPasswordHash, userPasswordHash);
                if (chkPassword)
                    lMSLogin = dbUser;
            }
            return lMSLogin;
        }

        /// <summary>
        /// Checks and returns if an user exits 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckIfUserExists(string userName)
        {
            bool isExists = false;
            var userCount = this.context.LMSLogins.Count(x => x.UserName == userName);
            if (userCount > 0)
                isExists = true;
            return isExists;
        }
    }
}
