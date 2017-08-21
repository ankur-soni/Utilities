using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using AutoMapper;
using Service;
using Data;
using PagedList;

namespace HR_Web.Controllers
{
    public class ProfessionalDetailsController : Controller
    {
        int userId = 0;
        string userName = null;

        //service call
        private IEmpSkillsService _IEmpSkillsService;
        private ISkillsetService _ISkillsetService;
        private IProfessionalDetailsService _IProfessionalDetailsService;

        //Data class
        EmployeeSkillDetail _employeeSkillDetail;
        Master_SkillSet _skillsMaster;
        EmployeeProfessionalDetail _professional;

        public ProfessionalDetailsController(IEmpSkillsService IEmpSkillsService, ISkillsetService ISkillsetService, IProfessionalDetailsService IProfessionalDetailsService)
        {
            this._IEmpSkillsService = IEmpSkillsService;
            this._ISkillsetService = ISkillsetService;
            this._IProfessionalDetailsService = IProfessionalDetailsService;

            _employeeSkillDetail = new EmployeeSkillDetail();
            _skillsMaster = new Master_SkillSet();
            _professional = new EmployeeProfessionalDetail();

        }
        
        public ActionResult Index()
        {   
            return View();
        }

        public ActionResult ProfessionalDetails()
        {
           
                return View();
        }

        [HttpPost]
        public ActionResult ProfessionalDetails(ProfessionalDetailsModel model)
        {

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult SkillsDetailsGrid()
        {
            Models.EmpSkillDetailsModel SkillsModel = new EmpSkillDetailsModel();
            userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
            SkillsModel.EmpSkillDetailsList = EmpSkillsPagedList(userId);


            ViewBag.Year_lst = GetYears();
            ViewBag.Month_lst = GetMonths();

            SkillsModel.ProfessionalDetailsModel = new Models.ProfessionalDetailsModel();
            var profobj = _IProfessionalDetailsService.GetAll(null, null, "");
            if (profobj != null)
            {
                var data = profobj.Where(x => x.UserID == userId).FirstOrDefault();
                if (data != null)
                {
                    SkillsModel.ProfessionalDetailsModel.Id = data.EmpProfID;
                    SkillsModel.ProfessionalDetailsModel.UserId = data.UserID;
                    SkillsModel.ProfessionalDetailsModel.ExprInYears = (data.TotalExprInYears +1).ToString();
                    SkillsModel.ProfessionalDetailsModel.ExprInMonths = (data.TotalExprInMonths+1).ToString();
                                      
                }

            }
            return View("_EmployeeSkillList", SkillsModel);
        }

        public IPagedList<EmpSkillDetailsHistory> EmpSkillsPagedList(int userid)
        {
            List<EmployeeSkillDetail> list = _IEmpSkillsService.GetAll(null,null,"").Where(x=>x.UserId==userid).ToList();

            List<Master_SkillSet> skillsList = _ISkillsetService.GetAll(null, null, "").ToList();

            var data = from ES in list
                       join SM in skillsList on ES.SkillId equals SM.ID
                       select new { Skill = SM.Skill, ES.ExprInMonths, ES.ExprInYears ,Id=ES.Id, IsActive=ES.IsActive};


            List<EmpSkillDetailsHistory> ModelList = new List<EmpSkillDetailsHistory>();
            ModelList = data
                .Select(x => new EmpSkillDetailsHistory() {Id = x.Id, IsActive=x.IsActive, SkillName = x.Skill, Skill_Experience = x.ExprInYears+"."+ x.ExprInMonths })
           .Where(x=>x.IsActive!=false)
           .ToList();
            int pageSize = 10;
            int pageIndex = 1;

            return ModelList.ToPagedList(pageIndex, pageSize);
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddEditSkills(int SkillId = 0, bool IsReadOnly = false)
        {
            ViewBag.Skill_lst = GetAllSkills();
            ViewBag.Year_lst = GetYears();
            ViewBag.Month_lst = GetMonths();

            ViewBag.IsReadOnly = IsReadOnly;
            ViewBag.IsEditPage = false;
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

                //var res = user.Where(u => u.Password == details.Password && u.Email.ToLower() == details.Email.ToLower()).SingleOrDefault();
                var empskills_lst = _IEmpSkillsService.GetAll(null, null, "");
                Session["empskills_lst"] = empskills_lst;
                var obj = empskills_lst.Where(u => u.UserId == userId && u.Id == SkillId).FirstOrDefault();
                if (obj != null)
                {
                    _employeeSkillDetail = obj;
                }

            }
            Models.EmpSkillDetailsModel model = new EmpSkillDetailsModel();
            if (SkillId != 0)
            {
                Mapper.CreateMap<Data.EmployeeSkillDetail, Models.EmpSkillDetailsModel>();
                model = Mapper.Map<Data.EmployeeSkillDetail, Models.EmpSkillDetailsModel>(_employeeSkillDetail);

                               
                ViewBag.IsEditPage = true;

                model.ExprInMonths = model.ExprInMonths +1;
                model.ExprInYears = model.ExprInYears +1;
            }

            model.ProfessionalDetailsModel = new Models.ProfessionalDetailsModel();
            var profobj = _IProfessionalDetailsService.GetAll(null, null, "");
            if (profobj != null)
            {
                var data = profobj.Where(x => x.UserID == userId).FirstOrDefault();
                if (data != null)
                {
                     model.ProfessionalDetailsModel.ExprInYears= data.TotalExprInYears.ToString();
                     model.ProfessionalDetailsModel.ExprInMonths = data.TotalExprInMonths.ToString();              
                }

            }
           
           
           
            return View("AddSkills", model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddEditSkills(EmpSkillDetailsModel details)
        {

            details.ExprInMonths = details.ExprInMonths - 1;
            details.ExprInYears = details.ExprInYears - 1;

            bool status = false;
            string userName = null;

            if (ModelState.IsValid)
            {
                details.UserId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];



                //details.UpdatedBy = userName;
                details.SkillId=details.Skill_Id;

                Mapper.CreateMap<Models.EmpSkillDetailsModel, Data.EmployeeSkillDetail>();
                var skillDetail = Mapper.Map<Models.EmpSkillDetailsModel, Data.EmployeeSkillDetail>(details);

                var skillList = (List<EmployeeSkillDetail>)Session["empskills_lst"];

                var obj = skillList.Where(u => u.UserId == details.UserId).ToList();
                if (obj != null)
                {
                    obj = obj.Where(u => u.UserId == details.UserId && u.SkillId == details.SkillId && u.IsActive!=false).ToList();
                }

                //check if user have alredy added record for this category 
                if (obj.Count > 0 && details.Id == 0)
                {
                    return Json(new { result = false, Message = "This skill is already Added" }, JsonRequestBehavior.AllowGet);
                }
               

                if (obj.Count == 0)
                {
                    //eduDetail.CreatedBy = userName;
                    //eduDetail.CreatedDate = DateTime.Now;
                    //eduDetail.Active = true;
                    status = _IEmpSkillsService.Insert(skillDetail, null, "");
                    if (status == true)
                        TempData["EDsucc"] = "Skill added sucesfully";                    
                }
                else
                {

                    status = _IEmpSkillsService.Update(skillDetail, null, "");
                    if (status == true)
                        TempData["EDsucc"] = "Skill updated sucesfully";                   
                }
                return RedirectToAction("SkillsDetailsGrid");
            }


            details.EmpSkillDetailsList = EmpSkillsPagedList(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]));
            return View(details);
        }

        public ActionResult AddTotEmpExpr(string TotExprYear, string TotExprMonth, int Id)
        {            
            bool status = false;
            string userName = null;

            if (!string.IsNullOrEmpty(TotExprYear) && !string.IsNullOrEmpty(TotExprYear))
            {
                userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name.Split('|')[1]);
                userName = System.Web.HttpContext.Current.User.Identity.Name.Split('|')[0];

               
                //Mapper.CreateMap<Models.ProfessionalDetailsModel, Data.EmployeeProfessionalDetail>();
                //var ProffessionalDetail = Mapper.Map<Models.ProfessionalDetailsModel, Data.EmployeeProfessionalDetail>(details);

                _professional = new EmployeeProfessionalDetail();
                _professional.TotalExprInYears = Convert.ToInt32(TotExprYear)-1;
                _professional.TotalExprInMonths = Convert.ToInt32(TotExprMonth)-1;
                _professional.UserID = userId;
                _professional.EmpProfID = Id;

                var obj = _IProfessionalDetailsService.GetAll(null, null, "");
                if (obj != null)
                {
                    var data = obj.Where(x => x.UserID == userId).FirstOrDefault();
                    if (data != null && Id!=0)
                    {
                        data.TotalExprInMonths = _professional.TotalExprInMonths;
                        data.TotalExprInYears = _professional.TotalExprInYears;
                        status = _IProfessionalDetailsService.Update(data, null, "");
                    }
                    else
                    {
                        status = _IProfessionalDetailsService.Insert(_professional, null, "");
                    }
                }
              
            }
            return Json(new { result = status }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteSkillsDetails(int SkillId)
        {

            var obj = _IEmpSkillsService.GetById(SkillId);
            if (obj != null)
            {
                obj.IsActive = false;
            }
            var status = _IEmpSkillsService.Update(obj, null, "");

            return Json(new { result = "" }, JsonRequestBehavior.AllowGet);

        }

        public SelectList GetAllSkills()
        {
            List<Master_SkillSet> skilllist = new List<Master_SkillSet>();
            skilllist = _ISkillsetService.GetAll(null, null, "").ToList();
            // List = List.OrderBy(x => x.Class).ToList();
            SelectList selList = new SelectList(skilllist, "ID", "Skill");
            return selList;

        }

        public SelectList GetYears()
        {
            List<Year> yearlist = new List<Models.Year>();

            Year y;
            for (int i = 0; i <= 15; ++i)
            {
                y = new Models.Year();
                y.YearId = i+1;
                y.YearName = i.ToString();
                yearlist.Add(y);
            }

            SelectList selList = new SelectList(yearlist, "YearId", "YearName");
            return selList;
            
        }

        public SelectList GetMonths()
        {
            List<Month> yearlist = new List<Models.Month>();

            Month y;

            int i;

            for (i = 0; i <= 11; ++i)
            {
                y = new Models.Month();
                y.MonthId = i+1;
                y.MonthName = i.ToString();
                yearlist.Add(y);
            }

            //y = new Models.Month();
            //y.MonthId = i+1;
            //y.MonthName = "0";
            //yearlist.Add(y);
                     
            SelectList selList = new SelectList(yearlist, "MonthId", "MonthName");
            return selList;

        }
    }

    
}