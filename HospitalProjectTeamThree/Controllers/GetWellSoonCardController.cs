using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospitalProjectTeamThree.Data;
using HospitalProjectTeamThree.Models;
using HospitalProjectTeamThree.Models.ViewModels;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
//need this for pagination
using PagedList;
using Microsoft.AspNet.Identity;

namespace HospitalProjectTeamThree.Controllers
{
    public class GetWellSoonCardController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public GetWellSoonCardController() { }
        // GET: GetWellSoonCard
        public ActionResult Index()
        {
            //if user logged in to the get well card page AND they are admins or editors
            //redirect them to list
            //if they are logged in but is not admin, editor
            //redirect to their personal page
            //else if they are not logged in or wrong logged in infor
            //reject them
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Editor"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("Add");//To do: redirect action to personal list
                }
                
            }
            else
            {
                return View();
            }       
        }        

        [Authorize(Roles = "Admin, Editor")]
        //authorize function only allow a certain ppl role to see the full list of card
        //To Do: personal list of card, normal user can only see the card they create
        public ActionResult List(int? page)
        {
            //string showDesignQuery = "Select * from CardDesigns inner join GetWellSoonCards on GetWellSoonCards.CardDesignId = CardDesigns.CardDesignId where CardId = @id";
            //string showCardQuery = "Select * from GetWellSoonCards";
            /*List<GetWellSoonCard> allCards = db.GetWellSoonCards.SqlQuery(showCardQuery).ToList();
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[1] = new SqlParameter("@id", id);*/
            Debug.WriteLine("Iam trying to list all the cards");

            //ListGetWell ListGetWellViewModel = new ListGetWell();
            //ListGetWellViewModel.GetWellSoonCard = allCards;
            //ListGetWellViewModel.CardDesign = cardDesign;

            //return View();           
            string query = "Select * from GetWellSoonCards";
            List<GetWellSoonCard> GetWellSoonCards = db.GetWellSoonCards.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the cards");
            //return View(GetWellSoonCards);
            //Reference link: 
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
            //allow 3 items per page
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            //passing the pagelist to the view
            return View(GetWellSoonCards.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PersonalList()
        {
            //string userId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            //return View(currentUser);
            //string query = "Select * from GetWellSoonCards where userId = userId";
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            string query = "select * from GetWellSoonCards where User_Id=@userId";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@userId", userId);

            List<GetWellSoonCard> Cards = db.GetWellSoonCards.SqlQuery(query, sqlparams).ToList();
            //SqlParameter[] sqlparams = new SqlParameter[1];
            //sqlparams[0] = new SqlParameter("@userId", userId);

            PersonalListGetWell PersonalListGetWellViewModel = new PersonalListGetWell();
            PersonalListGetWellViewModel.GetWellSoonCard = Cards;
            PersonalListGetWellViewModel.User = currentUser;
           
            return View(PersonalListGetWellViewModel);
        }
        public ActionResult Add()
        {
            List<CardDesign> addingDesign = db.CardDesigns.SqlQuery("select * from CardDesigns").ToList();
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            AddGetWellCard AddGetWellCardViewModel = new AddGetWellCard();
            AddGetWellCardViewModel.CardDesigns = addingDesign;
            AddGetWellCardViewModel.User = currentUser;

            return View(AddGetWellCardViewModel);
        }
        [HttpPost]
        public ActionResult Add(string message, string PatientName, string PatientEmail, string RoomNumber, string CardDesignId, string User_Id)
        {
            string query = "Insert into GetWellSoonCards (message, PatientName, PatientEmail , RoomNumber, CardDesignId, User_Id) values (@message, @PatientName, @PatientEmail, @RoomNumber, @CardDesignId, @User_Id)";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@message", message);
            sqlparams[1] = new SqlParameter("@CardDesignId", CardDesignId);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);
            sqlparams[5] = new SqlParameter("@User_Id", User_Id);
            //string userId = User.Identity.GetUserId();
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            Debug.WriteLine("I am tryting to add the card with the message " + message);
            return RedirectToAction("List");
        }
        public ActionResult Show(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns inner join GetWellSoonCards on GetWellSoonCards.CardDesignId = CardDesigns.CardDesignId where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card id" + id);
            ShowGetWell ShowGetWellViewModel = new ShowGetWell();
            ShowGetWellViewModel.GetWellSoonCard = Card;
            ShowGetWellViewModel.CardDesign = Design;
            return View(ShowGetWellViewModel);
        }
        public ActionResult Update(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            List<CardDesign> Designs = db.CardDesigns.SqlQuery("Select * from CardDesigns").ToList();
            Debug.WriteLine("I am trying to show card id" + id);
            UpdateGetWell UpdateGetWellViewModel = new UpdateGetWell();
            UpdateGetWellViewModel.GetWellSoonCard = Card;
            UpdateGetWellViewModel.CardDesigns = Designs;
            return View(UpdateGetWellViewModel);
        }
        [HttpPost]
        public ActionResult Update(string Message, string PatientName, string PatientEmail, string RoomNumber, int CardId, int CardDesignId)
        {
            string query = "Update GetWellSoonCards set Message=@Message, PatientName=@PatientName, PatientEmail=@PatientEmail, CardDesignId=@CardDesignId, RoomNumber=@RoomNumber where CardId=@CardId";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@Message", Message);
            sqlparams[1] = new SqlParameter("@CardDesignId", CardDesignId);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);
            sqlparams[5] = new SqlParameter("@CardId", CardId);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            Debug.WriteLine("I am tryting to edit the card with the message " + Message);
            return RedirectToAction("/Show/" + CardId);
        }
        public ActionResult Delete(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns inner join GetWellSoonCards on GetWellSoonCards.CardDesignId = CardDesigns.CardDesignId where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card id" + id);
            ShowGetWell ShowGetWellViewModel = new ShowGetWell();
            ShowGetWellViewModel.GetWellSoonCard = Card;
            ShowGetWellViewModel.CardDesign = Design;
            return View(ShowGetWellViewModel);
        }
        [Authorize(Roles = "Admin, Registered User")]
        //authorize function only allow a certain ppl role to see the full list of card
        //Admin has the full right to delete, User can delete their own cards.
        //Editor has to ask for the permission from admin to delete.
        [HttpPost]
        public ActionResult Delete(int id, int CardId)
        {
            string query = "Delete from GetWellSoonCards where CardId = @CardID";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CardId", CardId);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        public GetWellSoonCardController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

    }
}