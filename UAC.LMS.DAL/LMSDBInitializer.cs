using System;
using System.Collections.Generic;
using System.Data.Entity;
using UAC.LMS.Common.Constants;
using UAC.LMS.Common.Utilities;
using UAC.LMS.Models;

namespace UAC.LMS.DAL
{
    public class LMSDBInitializer : CreateDatabaseIfNotExists<LMSDBContext>
    {
        // comment here 

        /// <summary>
        ///  // Setting initial admin login 
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(LMSDBContext context)
        {

            //IList<Standard> defaultStandards = new List<Standard>();

            //defaultStandards.Add(new Standard() { StandardName = "Standard 1", Description = "First Standard" });
            //defaultStandards.Add(new Standard() { StandardName = "Standard 2", Description = "Second Standard" });
            //defaultStandards.Add(new Standard() { StandardName = "Standard 3", Description = "Third Standard" });

            //foreach (Standard std in defaultStandards)
            //    context.Standards.Add(std);

            List<LMSStatusCodeDetail> lstSttausCode = new List<LMSStatusCodeDetail>
            {
                new LMSStatusCodeDetail { StatusCodeId=StatusCodeConstants.Active,StatusCodeName="Active",StatusCode=StatusCodeConstants.Active,CreatedOn=DateTime.Now },
                new LMSStatusCodeDetail { StatusCodeId=StatusCodeConstants.InActive,StatusCodeName="InActive",StatusCode=StatusCodeConstants.Active,CreatedOn=DateTime.Now },
                new LMSStatusCodeDetail { StatusCodeId=StatusCodeConstants.OnLeave,StatusCodeName="OnLeave",StatusCode=StatusCodeConstants.Active,CreatedOn=DateTime.Now },
                new LMSStatusCodeDetail { StatusCodeId=StatusCodeConstants.Terminated,StatusCodeName="Terminated",StatusCode=StatusCodeConstants.Active,CreatedOn=DateTime.Now },

                 // Removed Retired 11/21/2016
               // new LMSStatusCodeDetail { StatusCodeId=StatusCodeConstants.Retired,StatusCodeName="Retired",StatusCode=StatusCodeConstants.Active,CreatedOn=DateTime.Now },
              
            };
            context.StatusCodeDetails.AddRange(lstSttausCode);

            //test
            //List<TestModel1> lstTestModel = new List<TestModel1>();
            //for (int i = 0; i < 300; i++)
            //{
            //    lstTestModel.Add(new TestModel1
            //    { MyProperty1 = "MyProperty" + i.ToString(), MyProperty2 = "MyProperty" + i.ToString(), MyProperty3 = "MyProperty3" });
            //}
            //context.TestModel1.AddRange(lstTestModel);                   

            string userName = "admin";
            string password = "admin@123";
            string strCurrentDate = DateTime.Now.ToString();
            byte[] passwordSalt = Encryptor.EncryptText(strCurrentDate, userName);
            string se = Convert.ToBase64String(passwordSalt);
            byte[] passwordHash = Encryptor.GenerateHash(password, se.ToString());
            
            LMSLogin lmsLogin = new LMSLogin
            {
                UserName = userName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                FirstName = "admin",
                LastName = "admin",
              
                CreatedOn = DateTime.Now,
                IsSecurityApplied = true,
                StatusCode = StatusCodeConstants.Active,
                IssueDate = DateTime.Now,
                //PermissionLevel = PermissionConstants.All,              
                PermissionLevel = PermissionConstants.SuperAdmin,
                UserType = UserTypeConstants.SuperAdmin
                
            };
            context.LMSLogins.Add(lmsLogin);
            context.SaveChanges();

            // Storing Security question in LMSSecurityQuestion table

            var lstSecuritQuestions = new List<LMSSecurityQuestion>
                {
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="In what city did you meet your spouse/significant other?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What was your childhood nickname?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What is the name of your favorite childhood friend?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What street did you live on in third grade?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What is your oldest sibling’s birthday month and year? (e.g., January 1900)"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What is the middle name of your oldest child?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What is your oldest sibling’s middle name?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What school did you attend for sixth grade?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What was your childhood phone number including area code? (e.g., 000-000-0000)"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What was the name of your first stuffed animal?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="What is your maternal grandmother’s maiden name?"},
                    new LMSSecurityQuestion{CreatedBy=lmsLogin.UserId,CreatedOn = DateTime.Now,StatusCode = StatusCodeConstants.Active,Question="In what town was your first job?"},
                };
            context.LMSSecurityQuestions.AddRange(lstSecuritQuestions);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
