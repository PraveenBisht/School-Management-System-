
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StudentManagementSystem.DB;
using StudentManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace StudentManagementSystem.Controllers
{
    [RoutePrefix("api/Student")]  //api/Student/MailVarify
    public class StudentController : ApiController
    {
        [HttpGet]
        [Route("SubjectList")]
        public IList<SubjectVM> GetSubjectList()
        {
            List<SubjectVM> SubList = new List<SubjectVM>();

            using (StudentsEntities context = new StudentsEntities())
            {
                var result = context.SubjectList().ToList();

                foreach (var item in result.ToList())
                {
                    SubList.Add(new SubjectVM {
                        SubjectId = item.SubjectId,
                        SubjectName =item.SubjectName
                    });
                }
                return SubList;
            }
        }
        [HttpGet]
        [Route("BloodGroupList")]
        public IList<BloodGroupVM> GetBloodGroupList()
        {
            List<BloodGroupVM> BloodGroupList = new List<BloodGroupVM>();

            using (StudentsEntities context = new StudentsEntities())
            {
                var result = context.BloodGroupList().ToList();

                foreach (var item in result.ToList())
                {
                    BloodGroupList.Add(new BloodGroupVM
                    {
                        BloodGroupId = item.BloodGroupId,
                        BloodGroupName = item.BloodGroupName
                    });
                }
                return BloodGroupList;
            }
        }
        [HttpPost]
        [Route("GetAllStudents")]
        public IList<Allstd> GetAllStudents()
        {
            List<Allstd> StudentList = new List<Allstd>();

            using (StudentsEntities context = new StudentsEntities())
            {
                var result = context.StudentRecord().ToList();

                foreach (var item in result.ToList())
                {
                    StudentList.Add(new Allstd
                    {
                        StudentId = item.StudentId,
                        Name = item.FirstName,
                        LastName = item.LastName,
                        EmailId = item.EmailId,
                        PhoneNo = item.PhoneNo,
                        ImgProfile = item.ProfilePic

                    });
                }
                return StudentList;
            }
        }
        [HttpPost]
        [Route("DeleteUserbyId")]
        public JsonResult<object> DeleteUserbyId([FromBody]JObject json)
        {
            Allstd UserId = JsonConvert.DeserializeObject<Allstd>(json.ToString());
            var obj = new object();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            int DeleteId = Convert.ToInt32(UserId.StudentId);
            try
            {
                using (StudentsEntities context = new StudentsEntities())
                {
                    int Deleteduser = context.DeletebyId(DeleteId);
                    if (Deleteduser >= 1)
                    {
                        dic.Add("Message", "Deleted Successfully.");
                        dic.Add("Status", "1");
                    }
                    else
                    {
                        dic.Add("Message", "No Data available");
                        dic.Add("Status", "2");
                    }
                }
            }
            catch (Exception E)
            {
                dic.Add("Message", E.Message);
                dic.Add("Status", "0");
            }
            obj = dic;
            return Json(obj);

        }
        [HttpPost]
        [Route("MailVarify")]
        public JsonResult<object> MailVarify([FromBody]JObject json)
        {
            StudentVM re = JsonConvert.DeserializeObject<StudentVM>(json.ToString());
            var obj = new object(); 
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                if (!string.IsNullOrWhiteSpace(re.EmailId))
                {
                    using (StudentsEntities context = new StudentsEntities())
                    {
                        var result = context.MailVarify(re.EmailId).ToList();
                        if(result.Count <= 0)
                        { 
                            dic.Add("Status", "1");
                            dic.Add("Message", "Varifyed");
                        }
                        else {
                            dic.Add("Status", "2");
                            dic.Add("Message", "Already Registered!");
                        }

                    }
                }
            }
            catch (Exception E)
            {
                dic.Add("Message", E.Message);
                dic.Add("Status", "0");
            }
            obj = dic;
            return Json(obj);
        }


        [HttpPost]
        [Route("ProfilePicUpdate")]
        public JsonResult<object> ProfilePicUpdate([FromBody]JObject json)
        {
            StudentVM re = JsonConvert.DeserializeObject<StudentVM>(json.ToString());
            var obj = new object();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string Image1 = re.ProfilePic;
            string SavePath1 = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(Image1))
                {
                    var postedFile = Image1;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/"));
                    }
                    string ImagePath1 = HttpContext.Current.Server.MapPath("~/Document/ProfilePic/") + re.FirstName + re.LastName + ".jpg";
                    SavePath1 = "/Document/ProfilePic/" + re.FirstName + re.LastName + ".jpg";
                    if (File.Exists(ImagePath1)) 
                    {
                        File.Delete(ImagePath1);
                    }
                    byte[] Image1D = Convert.FromBase64String(Image1.ToString());

                    Stream stmImage1D = new MemoryStream(Image1D);
                    Bitmap originalBMP1 = new Bitmap(stmImage1D);
                    originalBMP1.Save(ImagePath1);
                }
                using (StudentsEntities context = new StudentsEntities())
                {
                    var result = context.ProfilePicUpdate(re.StudentId, SavePath1);
                    dic.Add("Status", "1");
                    dic.Add("Message", "Success");
                }

            }
            catch (Exception E)
            {
                dic.Add("Message", E.Message);
                dic.Add("Status", "0");
            }
            obj = dic;
            return Json(obj);
        }



        [HttpPost]
        [Route("InsertStudentsDetail")]
        public JsonResult<object> InsertStudentsDetail([FromBody]JObject json)
        {
            StudentVM re = JsonConvert.DeserializeObject<StudentVM>(json.ToString());
            var obj = new object();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string Image1 = re.ProfilePic;
            string Image2 = re.Document1;
            string Image3 = re.Document2;
            string SavePath1 = "";
            string SavePath2 = "";
            string SavePath3 = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(Image1))
                {
                    var postedFile = Image1;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/"));
                    }
                    string ImagePath1 = HttpContext.Current.Server.MapPath("~/Document/ProfilePic/") + re.FirstName + re.LastName + ".jpg";
                    SavePath1 = "/Document/ProfilePic/" + re.FirstName + re.LastName + ".jpg";
                    if (File.Exists(ImagePath1))
                    {
                        File.Delete(ImagePath1);
                    }
                    byte[] Image1D = Convert.FromBase64String(Image1.ToString());

                    Stream stmImage1D = new MemoryStream(Image1D);
                    Bitmap originalBMP1 = new Bitmap(stmImage1D);
                    originalBMP1.Save(ImagePath1);
                }
                if (!string.IsNullOrWhiteSpace(Image2))
                {
                    var postedFile = Image2;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/Documents/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/Documents/"));
                    }
                    string ImagePath2 = HttpContext.Current.Server.MapPath("~/Document/Documents/")+ re.FirstName + re.LastName + "Doc1.jpg";
                    SavePath2 = "/Document/Documents/" + re.FirstName + re.LastName + "Doc1.jpg";
                    if (File.Exists(ImagePath2))
                    {
                        File.Delete(ImagePath2);
                    }
                    byte[] Image2D = Convert.FromBase64String(Image2.ToString());

                    Stream stmImage2D = new MemoryStream(Image2D);
                    Bitmap originalBMP2 = new Bitmap(stmImage2D);
                    originalBMP2.Save(ImagePath2);
                }
                if (!string.IsNullOrWhiteSpace(Image3))
                {
                    var postedFile = Image3;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/Documents/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/Documents/"));
                    }
                    string ImagePath3 = HttpContext.Current.Server.MapPath("~/Document/Documents/") + re.FirstName + re.LastName + "Doc2.jpg";
                    SavePath3 = "/Document/Documents/" + re.FirstName + re.LastName + "Doc2.jpg";
                    if (File.Exists(ImagePath3))
                    {
                        File.Delete(ImagePath3);
                    }
                    byte[] Image3D = Convert.FromBase64String(Image3.ToString());
                    Stream stmImage3D = new MemoryStream(Image3D);
                    Bitmap originalBMP3 = new Bitmap(stmImage3D);
                    originalBMP3.Save(ImagePath3);
                }

                using (StudentsEntities context = new StudentsEntities())
                {
                    var result = context.InsertStudent(re.FirstName, re.LastName,re.DOB,re.EmailId,re.PhoneNo,re.Address,re.BloodGroup,SavePath1,re.FatherName,re.category,re.MainSubject, SavePath2, SavePath3,re.Password, re.Obtional1,re.Obtional2,re.Obtional3,re.Obtional4,re.Role,re.SubjectId,re.BloodGroupId);
                    dic.Add("Status", "1");
                    dic.Add("Message", "Success");
                }

            }
            catch (Exception E)
            {
                dic.Add("Message", E.Message);
                dic.Add("Status", "0");
            }
            obj = dic;
            return Json(obj);
        }


        [HttpPost]
        [Route("UpdateUserDetail")]
        public JsonResult<object> UpdateUserDetail([FromBody]JObject json)
        {
            StudentVM re = JsonConvert.DeserializeObject<StudentVM>(json.ToString());
            var obj = new object();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            string Image1 = re.ProfilePic;
            string Image2 = re.Document1;
            string Image3 = re.Document2;
            string SavePath1 = "";
            string SavePath2 = "";
            string SavePath3 = "";
            try
            {
                if (!string.IsNullOrEmpty(Image1))
                {
                    var postedFile = Image1;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/ProfilePic/"));
                    }
                    string ImagePath1 = HttpContext.Current.Server.MapPath("~/Document/ProfilePic/") + re.FirstName + re.LastName + ".jpg";
                    SavePath1 = "/Document/ProfilePic/" + re.FirstName + re.LastName + ".jpg";
                    if (File.Exists(ImagePath1))
                    {
                        File.Delete(ImagePath1);
                    }
                    byte[] Image1D = Convert.FromBase64String(Image1.ToString());

                    Stream stmImage1D = new MemoryStream(Image1D);
                    Bitmap originalBMP1 = new Bitmap(stmImage1D);
                    originalBMP1.Save(ImagePath1);
                }
                if (!string.IsNullOrEmpty(Image2))
                {
                    var postedFile = Image2;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/Documents/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/Documents/"));
                    }
                    string ImagePath2 = HttpContext.Current.Server.MapPath("~/Document/Documents/") + re.FirstName + re.LastName + "Doc1.jpg";
                    SavePath2 = "/Document/Documents/" + re.FirstName + re.LastName + "Doc1.jpg";
                    if (File.Exists(ImagePath2))
                    {
                        File.Delete(ImagePath2);
                    }
                    byte[] Image2D = Convert.FromBase64String(Image2.ToString());

                    Stream stmImage2D = new MemoryStream(Image2D);
                    Bitmap originalBMP2 = new Bitmap(stmImage2D);
                    originalBMP2.Save(ImagePath2);
                }
                if (!string.IsNullOrEmpty(Image3))
                {
                    var postedFile = Image3;
                    if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Document/Documents/")))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Document/Documents/"));
                    }
                    string ImagePath3 = HttpContext.Current.Server.MapPath("~/Document/Documents/") + re.FirstName + re.LastName + "Doc2.jpg";
                    SavePath3 = "/Document/Documents/" + re.FirstName + re.LastName + "Doc2.jpg";
                    if (File.Exists(ImagePath3))
                    {
                        File.Delete(ImagePath3);
                    }
                    byte[] Image3D = Convert.FromBase64String(Image3.ToString());
                    Stream stmImage3D = new MemoryStream(Image3D);
                    Bitmap originalBMP3 = new Bitmap(stmImage3D);
                    originalBMP3.Save(ImagePath3);
                }

                using (StudentsEntities context = new StudentsEntities())
                {
                    var result = context.StudentUpdate(re.StudentId,re.FirstName, re.LastName, re.DOB, re.PhoneNo, re.Address, re.BloodGroup, SavePath1, re.FatherName, re.category, re.MainSubject, SavePath2, SavePath3, re.Password, re.Obtional1, re.Obtional2, re.Obtional3, re.Obtional4, re.Role, Convert.ToBoolean(0), re.SubjectId, re.BloodGroupId);
                    dic.Add("Status", "1");
                    dic.Add("Message", "Success");
                }

            }
            catch (Exception E)
            {
                dic.Add("Message", E.Message);
                dic.Add("Status", "0");
            }
            obj = dic;
            return Json(obj);
        }
        //var result = context.StudentUpdate(re.StudentId, re.FirstName, re.LastName, re.DOB, re.EmailId, re.PhoneNo, re.Address, re.BloodGroup, SavePath1, re.FatherName, re.category, re.MainSubject, SavePath2, SavePath3, re.Password, re.Obtional1, re.Obtional2, re.Obtional3, re.Obtional4, re.Role, Convert.ToBoolean(0), re.SubjectId, re.BloodGroupId);
        [HttpPost]
        [Route("EditUserbyId")]
        public JsonResult<object> EditUserbyId([FromBody]JObject json)
        {
            StudentVM request = JsonConvert.DeserializeObject<StudentVM>(json.ToString());
            var obj = new object();
            int UserIdEdit = Convert.ToInt32(request.StudentId);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            StudentVM StudentDetail = new StudentVM();

            using (StudentsEntities context = new StudentsEntities())
            {
                Student re = context.UserDetailbyId(UserIdEdit).FirstOrDefault();
                StudentDetail.StudentId = re.StudentId;
                StudentDetail.FirstName = re.FirstName;
                StudentDetail.LastName = re.LastName;
                StudentDetail.DOB = (re.DOB);
                StudentDetail.EmailId = re.EmailId;
                StudentDetail.PhoneNo = re.PhoneNo;
                StudentDetail.Address = re.Address;
                StudentDetail.FatherName = re.FatherName;
                StudentDetail.BloodGroup = re.BloodGroup;
                StudentDetail.MainSubject = re.MainSubject;
                StudentDetail.Obtional1 = re.Obtional1;
                StudentDetail.Obtional2 = re.Obtional2;
                StudentDetail.Obtional3 = re.Obtional3;
                StudentDetail.Obtional4 = re.Obtional4;
                StudentDetail.category = re.category;
                StudentDetail.ProfilePic = re.ProfilePic;
                StudentDetail.Document1 = re.Document1;
                StudentDetail.Document2 = re.Document2;
                StudentDetail.Password = re.Password;
                StudentDetail.SubjectId = Convert.ToInt32(re.SubjectId);
                StudentDetail.BloodGroupId = Convert.ToInt32(re.BloodGroupId);
                if (StudentDetail != null)
                {
                    dic.Add("result", StudentDetail);
                    dic.Add("Message", "UserDetail");
                    dic.Add("Status", "1");
                }
                else
                {
                    dic.Add("Message", "No Data available");
                    dic.Add("Status", "2");
                }
                obj = dic;
                return Json(obj);
            }
        }

    }
}

