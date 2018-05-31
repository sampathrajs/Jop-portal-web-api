using jobportal.Utilities;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace jobportal.Controllers
{
    public class DefaultController : ApiController
    {
        SankeerthiEntities _jobdetails = new SankeerthiEntities();
        [HttpGet]
        public bool data()
        {
            WebApiApplication._log.LogInfoFormat("MyPortal");

            return true;
        }
        [HttpGet]
        public Jobsearch getjobsearchdetails()
        {
            Jobsearch _objJobsearch = new Jobsearch();
            try
            {
                //Jobsearch _objJobsearch = new Jobsearch();
                List<categoryclass> _objcategoryclass = new List<categoryclass>();
                var category = (from s in _jobdetails.lkpcategories
                                where s.isactive == true
                                select s).ToList();
                foreach (var categorylist in category)
                {
                    categoryclass _objcategory = new categoryclass();
                    _objcategory.categoryname = categorylist.Categoryname;
                    _objcategoryclass.Add(_objcategory);
                }
                //_objJobsearch.Add(_objcategoryclass);
                List<locationclass> _objlocationclass = new List<locationclass>();
                var location = (from s in _jobdetails.lkpStates
                                where s.isactive == true
                                select s).ToList();
                foreach (var locationlist in location)
                {
                    locationclass _objlocation = new locationclass();
                    _objlocation.statename = locationlist.statename;
                    _objlocationclass.Add(_objlocation);
                }
                List<experienceclass> _objexperienceclass = new List<experienceclass>();
                var experience = (from s in _jobdetails.lkpexperiencetypes
                                  where s.isactive == true
                                  select s).ToList();
                foreach (var experiencelist in experience)
                {
                    experienceclass _objexperience = new experienceclass();
                    _objexperience.experiencetype = experiencelist.experiencetype;
                    _objexperienceclass.Add(_objexperience);
                }
                List<employementclass> _objemployementclass = new List<employementclass>();
                var employement = (from s in _jobdetails.lkpemployement_type
                                   where s.isactive == true
                                   select s).ToList();
                foreach (var employementlist in employement)
                {
                    employementclass _objemployement = new employementclass();
                    _objemployement.employementtype = employementlist.employementtype;
                    _objemployementclass.Add(_objemployement);
                }
                List<companyclass> _objcompanyclass = new List<companyclass>();
                var company = (from s in _jobdetails.lkpcompanies
                               where s.isactive == true
                               select s).ToList();
                foreach (var companylist in company)
                {
                    companyclass _objcompany = new companyclass();
                    _objcompany.companyname = companylist.companyname;
                    _objcompanyclass.Add(_objcompany);
                }

                _objJobsearch.category = _objcategoryclass;
                _objJobsearch.location = _objlocationclass;
                _objJobsearch.experience = _objexperienceclass;
                _objJobsearch.employement = _objemployementclass;
                _objJobsearch.company = _objcompanyclass;
            }
            catch (Exception ex)
            {
                return null;
            }
            return _objJobsearch;

        }
        [HttpGet]
        public List<Jobposteddetails> getJobposteddetails()
        {
            WebApiApplication._log.LogInfoFormat("DefaultController/getJobposteddetails");
            List<Jobposteddetails> obj_jobposteddetails = new List<Jobposteddetails>();
            var posteddetails = (from t_jobdetail in _jobdetails.jobdetails
                                 join t_category in _jobdetails.lkpcategories on t_jobdetail.FK_CategoryID equals t_category.PK_CategoryID
                                 join t_comapny in _jobdetails.lkpcompanies on t_jobdetail.FK_CompanyID equals t_comapny.PK_CompanyID
                                 join t_location in _jobdetails.lkpcountries on t_jobdetail.FK_CountryID equals t_location.PK_CountryID
                                 join t_state in _jobdetails.lkpStates on t_jobdetail.FK_StateID equals t_state.PK_StateID
                                 join t_exper in _jobdetails.lkpexperiencetypes on t_jobdetail.FK_ExpericeID equals t_exper.PK_ExpericeID
                                 join t_employement in _jobdetails.lkpemployement_type on t_jobdetail.FK_EmployementID equals t_employement.PK_EmployementID
                                 where t_jobdetail.isactive == true && t_comapny.isactive == true && t_state.isactive == true && t_location.isactive == true && t_employement.isactive == true
                                 && t_exper.isactive == true
                                 select new
                                 {
                                     t_category.Categoryname,
                                     t_comapny.companyname,
                                     t_employement.employementtype,
                                     t_exper.experiencetype,
                                     t_state.statename,
                                     t_jobdetail.image,
                                     t_jobdetail.minsalary,
                                     t_jobdetail.maxsalary,
                                     t_jobdetail.jobdesgination,
                                     t_jobdetail.jobposteddate
                                 }).ToList();
            foreach (var postjob in posteddetails)
            {
                Jobposteddetails _Jobposteddetails = new Jobposteddetails();
                _Jobposteddetails.companyname = postjob.companyname;
                _Jobposteddetails.categoryname = postjob.Categoryname;
                _Jobposteddetails.desgination = postjob.jobdesgination;
                _Jobposteddetails.minsalary = postjob.minsalary;
                _Jobposteddetails.maxsalary = postjob.maxsalary;
                _Jobposteddetails.postedon = Convert.ToDateTime(postjob.jobposteddate);
                _Jobposteddetails.location = postjob.statename;
                _Jobposteddetails.experiencelevel = postjob.experiencetype;
                _Jobposteddetails.employementtype = postjob.employementtype;
                _Jobposteddetails.image = postjob.image.ToString();
                obj_jobposteddetails.Add(_Jobposteddetails);
            }
            return obj_jobposteddetails;
        }

        [HttpGet]
        public List<Jobposteddetails> getJobsearchbuttondetails(string loc, string cat, [FromUri]string[] day, [FromUri]string[] etype, [FromUri]string[] elevel, [FromUri]string[] ecompany, [FromUri]string[] elocation, string esalary, string ecat)
        {
            List<Jobposteddetails> obj_jobposteddetails = new List<Jobposteddetails>();
            var posteddetails = (from t_jobdetail in _jobdetails.jobdetails
                                 join t_category in _jobdetails.lkpcategories on t_jobdetail.FK_CategoryID equals t_category.PK_CategoryID
                                 join t_comapny in _jobdetails.lkpcompanies on t_jobdetail.FK_CompanyID equals t_comapny.PK_CompanyID
                                 join t_location in _jobdetails.lkpcountries on t_jobdetail.FK_CountryID equals t_location.PK_CountryID
                                 join t_state in _jobdetails.lkpStates on t_jobdetail.FK_StateID equals t_state.PK_StateID
                                 join t_exper in _jobdetails.lkpexperiencetypes on t_jobdetail.FK_ExpericeID equals t_exper.PK_ExpericeID
                                 join t_employement in _jobdetails.lkpemployement_type on t_jobdetail.FK_EmployementID equals t_employement.PK_EmployementID
                                 where t_jobdetail.isactive == true && t_comapny.isactive == true && t_state.isactive == true && t_location.isactive == true && t_employement.isactive == true
                                 && t_exper.isactive == true
                                 select new
                                 {
                                     t_category.Categoryname,
                                     t_comapny.companyname,
                                     t_employement.employementtype,
                                     t_exper.experiencetype,
                                     t_state.statename,
                                     t_jobdetail.image,
                                     t_jobdetail.minsalary,
                                     t_jobdetail.maxsalary,
                                     t_jobdetail.jobdesgination,
                                     t_jobdetail.jobposteddate
                                 }).ToList();
            foreach (var postjob in posteddetails)
            {
                Jobposteddetails _Jobposteddetails = new Jobposteddetails();
                _Jobposteddetails.companyname = postjob.companyname;
                _Jobposteddetails.categoryname = postjob.Categoryname;
                _Jobposteddetails.desgination = postjob.jobdesgination;
                _Jobposteddetails.minsalary = postjob.minsalary;
                _Jobposteddetails.maxsalary = postjob.maxsalary;
                _Jobposteddetails.postedon = Convert.ToDateTime(postjob.jobposteddate);
                _Jobposteddetails.location = postjob.statename;
                _Jobposteddetails.experiencelevel = postjob.experiencetype;
                _Jobposteddetails.employementtype = postjob.employementtype;
                _Jobposteddetails.image = postjob.image.ToString();
                _Jobposteddetails.statename = postjob.statename;
                obj_jobposteddetails.Add(_Jobposteddetails);
            }
            if (elocation[0] == "all")
            {
                if (loc != null && loc != "" && loc != "all")
                {
                    obj_jobposteddetails = obj_jobposteddetails.Where(x => loc.Contains(x.statename)).ToList();
                }
            }
            else
            {
                string location = string.Empty;
                for (int i = 0; i < elocation.Length; i++)
                {
                    location += elocation[i] + ",";
                }
                if (loc != null && loc != "" && loc != "all")
                {
                    if (!location.Contains(loc))
                    {
                        location += loc;
                    }

                }
                if (location != null && location != "")
                {
                    obj_jobposteddetails = obj_jobposteddetails.Where(x => location.Contains(x.statename)).ToList();
                }
            }


            if (ecompany[0] == "all")
            {

            }
            else
            {
                string company = string.Empty;
                for (int i = 0; i < ecompany.Length; i++)
                {
                    company += ecompany[i] + ",";
                }
                if (company != null && company != "")
                {
                    obj_jobposteddetails = obj_jobposteddetails.Where(x => company.Contains(x.companyname)).ToList();
                }
            }

            string category = string.Empty;
            if (cat != null && cat != "" && cat != "all")
            {
                category = cat;
            }
            if (category != null && category != "")
            {
                obj_jobposteddetails = obj_jobposteddetails.Where(x => category.Contains(x.categoryname)).ToList();
            }

            if (etype[0] == "all")
            {

            }
            else
            {
                string employementtype = string.Empty;
                for (int i = 0; i < etype.Length; i++)
                {
                    employementtype += etype[i] + ",";
                }
                if (employementtype != null && employementtype != "")
                {
                    obj_jobposteddetails = obj_jobposteddetails.Where(x => employementtype.Contains(x.employementtype)).ToList();
                }
            }
            if (elevel[0] == "all")
            {

            }
            else
            {
                string experience = string.Empty;
                for (int i = 0; i < elevel.Length; i++)
                {
                    experience += elevel[i] + ",";
                }
                if (experience != null && experience != "")
                {
                    obj_jobposteddetails = obj_jobposteddetails.Where(x => experience.Contains(x.experiencelevel)).ToList();
                }
            }
            int salary = 0;
            if (esalary != null && esalary != "")
            {
                salary = int.Parse(esalary);
            }
            obj_jobposteddetails = obj_jobposteddetails.Where(x => x.minsalary >= salary).ToList();
            DateTime posteddate = DateTime.Now;
            if (day[0] == "all")
            {
                posteddate = DateTime.Now;
            }
            else
            {
                string date = string.Empty;

                for (int i = 0; i < day.Length; i++)
                {
                    date += day[i] + ",";
                }
                if (date.Contains("30 days"))
                {
                    posteddate = posteddate.AddMonths(-1);
                }
                else if (date.Contains("7 days"))
                {
                    posteddate = posteddate.AddDays(-7);
                }
                obj_jobposteddetails = obj_jobposteddetails.Where(x => x.postedon >= posteddate).ToList();
            }
           
            return obj_jobposteddetails;
        }

        [HttpPost]
        public async Task<bool> SignInAccess(signinclass login)
        {
            bool isvalid = false;
            try
            {
                var login_credentials = _jobdetails.usertables.Where(c => c.username == login.UserName && c.password == login.Password).FirstOrDefault();
                if (login_credentials != null)
                {
                    isvalid = true;
                }
                else
                {
                    isvalid = false;
                }

            }
            catch (Exception ex)
            {
                isvalid = false;
            }
            return isvalid;
        }
        [HttpPost]
        public async Task<bool> MyPortal()
        {
            string Email = string.Empty;
            string FullName = string.Empty;
            bool isvalid = false;
            try
            {
              
                WebApiApplication._log.LogInfoFormat("MyPortal");

                var context = HttpContext.Current.Request.Files;
                var root = HttpContext.Current.Server.MapPath("~/App_Data/Temp/FileUploads");
                Directory.CreateDirectory(root);
                var provider = new MultipartFormDataStreamProvider(root);
                var result = await Request.Content.ReadAsMultipartAsync(provider);

                WebApiApplication._log.LogDebugFormat("MyPortal: {0}", result.FormData["postresumedata"]);
                WebApiApplication._log.LogInfoFormat("MyPortal after debug print");


                if (result.FormData["postresumedata"] != null)
                {
                    var modelStr = result.FormData["postresumedata"];
                    JavaScriptSerializer JS = new JavaScriptSerializer();
                    var data = JS.Deserialize<JobPortal>(modelStr);




                    careerdetail _objcareerdetails = new careerdetail();

                    Guid ID = Guid.NewGuid();
                    _objcareerdetails.PK_MemberID = ID;
                    _objcareerdetails.FullName = data.FullName;
                    _objcareerdetails.AdditionalInformation = data.AdditionalInfo;
                    if (result.FileData != null)
                    {
                        var files = result.FileData;
                        string filename = files[0].LocalFileName;


                        byte[] imageData; var imagestring="";

                        using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            BinaryReader br = new BinaryReader(file);
                            imageData = br.ReadBytes((int)file.Length);
                            imagestring= Convert.ToBase64String(imageData.ToArray());
                           // bytes = new byte[file.Length];
                           // file.Read(bytes, 0, (int)file.Length);
                        }
                        _objcareerdetails.Resume = imagestring;
                    }                  

                    _objcareerdetails.CareerObjective = data.CarrierObjective;
                    _objcareerdetails.SpecialQualification = data.SpecialQualification;
                    _objcareerdetails.Declaration = data.declaration;
                    _objcareerdetails.createdon = DateTime.Now;
                    _objcareerdetails.EmailAlert = data.emailAlert;
                    _jobdetails.careerdetails.Add(_objcareerdetails);
                    foreach (var workhistory in data.workHistry)
                    {
                        workhistory _objworkhistory = new workhistory();
                        _objworkhistory.PK_WorkHistoryID = Guid.NewGuid();
                        _objworkhistory.FK_MemberID = ID;
                        _objworkhistory.CompanyName = workhistory.companyName;
                        _objworkhistory.Desgination = workhistory.designation;
                        _objworkhistory.JobDescription = workhistory.jobDescription;
                        //foreach (var timeperiod in workhistory.timePeriod)
                        //{
                        if(workhistory.timePeriod.fromDate !="" && workhistory.timePeriod.fromDate !=null)
                        _objworkhistory.TimePeriod_From = Convert.ToDateTime(workhistory.timePeriod.fromDate);
                        if (workhistory.timePeriod.toDate!= "" && workhistory.timePeriod.toDate!= null)
                            _objworkhistory.TimePeriod_To = Convert.ToDateTime(workhistory.timePeriod.toDate);

                        //}
                        _jobdetails.workhistories.Add(_objworkhistory);
                    }
                    foreach (var education in data.educationBackground)
                    {
                        educationdetail _objeducationdetails = new educationdetail();
                        _objeducationdetails.PK_EducationDetailsID = Guid.NewGuid();
                        _objeducationdetails.FK_MemberID = ID;
                        _objeducationdetails.InstituteName = education.instituteName;
                        _objeducationdetails.Degree = education.degree;
                        _objeducationdetails.Ed_Description = education.designation;
                        //foreach (var timeperiod in education.timePeriod)
                        //{
                        if (education.timePeriod.fromDate != "" && education.timePeriod.fromDate != null)
                            _objeducationdetails.TimePeriod_From = Convert.ToDateTime(education.timePeriod.fromDate);
                        if (education.timePeriod.toDate != "" && education.timePeriod.toDate != null)
                            _objeducationdetails.TimePeriod_To = Convert.ToDateTime(education.timePeriod.toDate);

                        //}
                        _jobdetails.educationdetails.Add(_objeducationdetails);
                    }
                    foreach (var Language in data.language)
                    {
                        languageproficiency _objLanguage = new languageproficiency();
                        _objLanguage.PK_LanguageProficiencyID = Guid.NewGuid();
                        _objLanguage.FK_MemberID = ID;
                        _objLanguage.LanguageName = Language.languageName;
                        if(Language.rating!=null && Language.rating!="")
                        _objLanguage.Rating = Convert.ToDecimal(Language.rating);

                        _jobdetails.languageproficiencies.Add(_objLanguage);
                    }
                    foreach (var personaldetail in data.personalDetails)
                    {
                        personaldetail _objpersonal = new personaldetail();
                        _objpersonal.PK_MemberUserID = Guid.NewGuid();
                        _objpersonal.FK_MemberID = ID;
                        FullName= personaldetail.LastName;
                        _objpersonal.LastName = personaldetail.LastName;
                        _objpersonal.MotherName = personaldetail.motherName;
                        _objpersonal.FatherName = personaldetail.fatherName;
                        _objpersonal.DOB = Convert.ToDateTime(personaldetail.dateOfBirth);
                        _objpersonal.BirthPlace = personaldetail.birthPlace;
                        _objpersonal.Nationality = personaldetail.nationality;
                        Email = data.email;
                        _objpersonal.email = data.email;
                        _objpersonal.PhoneNumber = data.phoneNumber;
                        _objpersonal.Sex = personaldetail.sex;
                        _objpersonal.Address = personaldetail.address;
                        _jobdetails.personaldetails.Add(_objpersonal);
                    }
                    _jobdetails.SaveChanges();                  
                    isvalid = true;
                   // SendChangePasswordMail(Email, FullName);
                    Thread MailThread = new Thread(() => SendChangePasswordMail(Email, FullName));
                    MailThread.IsBackground = true;
                    MailThread.Start();



                }

            }
            catch (Exception ex)
            {
                WebApiApplication._log.LogErrorFormat("MyPortal : {0}", ex);
                isvalid = false;
            }
            return isvalid;           
        }

        public void SendChangePasswordMail(string MailID, string Firstname)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                byte[] imageData = null;
                string imageLocation = System.Web.Hosting.HostingEnvironment.MapPath("~/images/logo.png");
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);


                var imagestring = Convert.ToBase64String(imageData.ToArray());
                string username = ConfigurationManager.AppSettings["username"].ToString();
                string password = ConfigurationManager.AppSettings["password"].ToString();
                string MessageContent = ConfigurationManager.AppSettings["Messagecontent"].ToString();
                smtp.Host = ConfigurationManager.AppSettings["Host"].ToString();
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
                smtp.Credentials = new NetworkCredential(username, password);
                smtp.EnableSsl = false;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(ConfigurationManager.AppSettings["From"].ToString());
                mail.To.Add(MailID);
                mail.Subject = "Job Applied";


                string TemplatePathForgetPassword = System.Web.Hosting.HostingEnvironment.MapPath("~/Views/MailTemplate.htm");
                string htmlForgetPassword = System.IO.File.ReadAllText(TemplatePathForgetPassword);
                htmlForgetPassword = htmlForgetPassword.Replace("#senderFirstName", Firstname);
                htmlForgetPassword = htmlForgetPassword.Replace("#content", MessageContent);
                htmlForgetPassword = htmlForgetPassword.Replace("#CompanyName", "Sankeerthi Staffing Solutions");
                htmlForgetPassword = htmlForgetPassword.Replace("#imageid", imagestring);
                mail.Body = htmlForgetPassword;
                mail.IsBodyHtml = true;

                smtp.Send(mail);

             // return "Success";
            }
            catch (Exception ex)
            {

               // return null;
            }
        }


    }
    public class search
    {
        public string Jobcategory { get; set; }
        public string JobLocation { get; set; }
    }
    public class apiData
    {
        public JobPortal Job { get; set; }
        public byte[] UserImage { get; set; }
    }
    public class JobPortal
    {
        public string FullName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string AdditionalInfo { get; set; }

        public FileAccess userImage;
        public string CarrierObjective { get; set; }
        public string declaration { get; set; }

        public string SpecialQualification { get; set; }
        public  bool emailAlert { get; set; }
        public List<workHistry> workHistry { get; set; }
        public List<educationBackground> educationBackground { get; set; }
        public List<personal> personalDetails { get; set; }
        public List<language> language { get; set; }

    }

    public class workHistry
    {
        public string companyName { get; set; }
        public string designation { get; set; }
        public string jobDescription { get; set; }
        public timePeriod timePeriod { get; set; }


    }

    public class timePeriod
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class educationBackground
    {

        public string instituteName { get; set; }
        public string degree { get; set; }
        public string designation { get; set; }
        public timePeriod timePeriod { get; set; }

    }
    public class personal
    {
        public string LastName { get; set; }
        public string fatherName { get; set; }
        public string motherName { get; set; }
        public string birthPlace { get; set; }
        public string dateOfBirth { get; set; }      
        public string nationality { get; set; }
        public string sex { get; set; }
        public string address { get; set; }
      

    }
    public class language
    {
        public string languageName { get; set; }
        public string rating { get; set; }

    }
    public class signinclass
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Jobposteddetails
    {

        public string categoryname { get; set; }
        public string companyname { get; set; }
        public string desgination { get; set; }
        public DateTime postedon { get; set; }
        public int? minsalary { get; set; }
        public int? maxsalary { get; set; }
        public string location { get; set; }
        public string experiencelevel { get; set; }
        public string employementtype { get; set; }
        public string statename { get; set; }
        public string image { get; set; }
    }
    public class Jobsearch
    {
        public List<categoryclass> category { get; set; }
        public List<locationclass> location { get; set; }
        public List<experienceclass> experience { get; set; }
        public List<employementclass> employement { get; set; }
        public List<companyclass> company { get; set; }
    }


    public class categoryclass
    {
        public string categoryname { get; set; }
    }
    public class locationclass
    {
        public string statename { get; set; }
    }
    public class experienceclass
    {
        public string experiencetype { get; set; }
    }
    public class employementclass
    {
        public string employementtype { get; set; }
    }
    public class companyclass
    {
        public string companyname { get; set; }
    }
    public class filterapplied
    {
        public string category { get; set; }
        public List<locationclass> location { get; set; }
        public posteddate postdate { get; set; }
        //public string salary { get; set; }
        public List<employementclass> employeement_type { get; set; }
        public List<experienceclass> experirence_type { get; set; }
        public List<companyclass> company { get; set; }
    }
    public class posteddate
    {
        public DateTime today { get; set; }
        public DateTime oneweek { get; set; }
        public DateTime onemonth { get; set; }

    }
}

