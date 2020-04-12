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

        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        public GetWellSoonCardController() { }
        // GET: GetWellSoonCard
        public ActionResult Index()
        {
            //if user logged in to the get well card page AND they are admins or editors
            //redirect them to list
            //if they are logged in but is not admin, editor
            //redirect to their personal page
            //else if they are not logged in or having wrong logged in infor
            //reject them
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("Editor"))
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("PersonalList");//To do: redirect action to personal list
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
        public ActionResult List(int? page, string currentFilter, string cardsearch)
        {
            //Linq technique
            if (cardsearch != null)
            {
                //reset the page to 1 if there is a result in the search
                page = 1;
            }
            else
            {
                //maintain the filter setting during paging
                cardsearch = currentFilter;
            }

            ViewBag.CurrentFilter = cardsearch;
            if (cardsearch != null && cardsearch != "")
            {
                List<GetWellSoonCard> GetWellSoonCards = db.GetWellSoonCards
                    .Where(card =>
                        card.Message.Contains(cardsearch) ||
                        card.PatientName.Contains(cardsearch) ||
                        card.User.FirstName.Contains(cardsearch) ||
                        card.PatientEmail.Contains(cardsearch)
                    )
                    .ToList();
                int pageSize = 3;
                int pageNumber = (page ?? 1);                
                return View(GetWellSoonCards.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                List<GetWellSoonCard> GetWellSoonCards = db.GetWellSoonCards.ToList();
                return View(GetWellSoonCards.ToPagedList(pageNumber, pageSize));
            }
            //SQL technique
            /*string query = "Select * from GetWellSoonCards";
            List<GetWellSoonCard> GetWellSoonCards = db.GetWellSoonCards.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the cards");
            //return View(GetWellSoonCards);
            //Reference link: 
            //https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application
            //allow 3 items per page
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            //passing the pagelist to the view
            return View(GetWellSoonCards.ToPagedList(pageNumber, pageSize));*/
        }
        //Problem: Pagination doesn't work with viewmodel using the same technique above 
        public ActionResult PersonalList(/*int? pagenum, string currentFilter,*/ string cardsearch)
        {

            //get the current user id when they logged in
            string userId = User.Identity.GetUserId();

            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            /*if (cardsearch != null)
            {
                //reset the page to 1 if there is a result in the search
                page = 1;
            }
            else
            {
                //maintain the filter setting during paging
                cardsearch = currentFilter;
            }*/

            //ViewBag.CurrentFilter = cardsearch;
            if (cardsearch != null && cardsearch != "")
            {               
                string searchquery = "select * from GetWellSoonCards where User_Id=@userId and (message=@cardsearch " +
                    "or PatientEmail=@cardsearch " +
                    "or PatientName=@cardsearch " +
                    "or RoomNumber=@cardsearch)";
                SqlParameter[] searchparams = new SqlParameter[2];
                searchparams[0] = new SqlParameter("@userId", userId);
                searchparams[1] = new SqlParameter("@cardsearch", cardsearch);
                List<GetWellSoonCard> Cards = db.GetWellSoonCards.SqlQuery(searchquery, searchparams).ToList();
                //allow 3 items per page
                //int pageSize = 3;
                //int pageNumber = (page ?? 1);
                PersonalListGetWell PersonalListGetWellViewModel = new PersonalListGetWell();
                //go to the viewmodel get the paged list and instanciate it here
                
                PersonalListGetWellViewModel.GetWellSoonCard = Cards;//.ToPagedList(pageSize, pageNumber);
                PersonalListGetWellViewModel.User = currentUser;

                return View(PersonalListGetWellViewModel);
            }
            else
            {
            //return his personal list of card based on his id
            string query = "select * from GetWellSoonCards where User_Id=@userId";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@userId", userId);

            
            List<GetWellSoonCard> Cards = db.GetWellSoonCards.SqlQuery(query, sqlparams).ToList();
            //allow 3 items per page
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            PersonalListGetWell PersonalListGetWellViewModel = new PersonalListGetWell();
            //go to the viewmodel get the paged list and instanciate it here
            PersonalListGetWellViewModel.GetWellSoonCard = Cards;
            PersonalListGetWellViewModel.User = currentUser;
            
            //allow 3 items per page
           
            return View(PersonalListGetWellViewModel);
            }

            
        }
        public ActionResult Add()
        {
            List<CardDesign> addingDesign = db.CardDesigns.ToList();
            /*List<CardDesign> addingDesign = db.CardDesigns.SqlQuery("select * from CardDesigns").ToList();*/
            //get the current userId, display it in the view by a intput type="hidden" to add it in the query later
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);

            AddGetWellCard AddGetWellCardViewModel = new AddGetWellCard();
            AddGetWellCardViewModel.CardDesigns = addingDesign;
            AddGetWellCardViewModel.User = currentUser;

            return View(AddGetWellCardViewModel);
        }
        [HttpPost]
        public ActionResult Add(string message, string PatientName, string PatientEmail, string RoomNumber, int CardDesignId, string User_Id)
        {
            /*Linq technique
            //LINQ technique doesn't work with current technique to grab userid and inject it in the Get Well Card table
            //To do: find out why
            GetWellSoonCard newCard = new GetWellSoonCard();  
            //SQL Query: "Insert into ... "
            newCard.Message = message;
            newCard.PatientName = PatientName;
            newCard.CardDesignId = CardDesignId;
            newCard.PatientEmail = PatientEmail;
            newCard.RoomNumber = RoomNumber;       
            newCard.User_Id = User_Id (Error: User_Id is not on the GetWellCard table (but it is))

            db.GetWellSoonCards.Add(newCard);
            db.SaveChanges();*/
            
            //SQL technique
            string query = "Insert into GetWellSoonCards (message, PatientName, PatientEmail , RoomNumber, CardDesignId, User_Id) values (@message, @PatientName, @PatientEmail, @RoomNumber, @CardDesignId, @User_Id)";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@message", message);
            sqlparams[1] = new SqlParameter("@CardDesignId", CardDesignId);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);
            //insert the Id grabbed from above to the get well cards table
            sqlparams[5] = new SqlParameter("@User_Id", User_Id);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);
            Debug.WriteLine("I am tryting to add the card with the message " + message);
            //when adding is done, admin will return to the total list
            //user will return to personal list            
            if (User.IsInRole("Admin") || User.IsInRole("Editor"))
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("PersonalList");
            }
            //return RedirectToAction("List");
        }
        public ActionResult Show(int id)
        {   
            //grab the id of current logged in user to display their names, phone etc...
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == userId);
            //display the cards and the design of cards
            GetWellSoonCard Card = db.GetWellSoonCards.Find(id);
            //GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
           //Look into Inner join
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns inner join GetWellSoonCards on GetWellSoonCards.CardDesignId = CardDesigns.CardDesignId where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card id" + id);
            //instanciate the class
            ShowGetWell ShowGetWellViewModel = new ShowGetWell();
            ShowGetWellViewModel.GetWellSoonCard = Card;
            ShowGetWellViewModel.CardDesign = Design;
            ShowGetWellViewModel.User = currentUser;
            return View(ShowGetWellViewModel);
        }
        public ActionResult Update(int id)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.Find(id);
            List<CardDesign> Designs = db.CardDesigns.ToList();
            //GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            //List<CardDesign> Designs = db.CardDesigns.SqlQuery("Select * from CardDesigns").ToList();
            Debug.WriteLine("I am trying to show card id" + id);
            UpdateGetWell UpdateGetWellViewModel = new UpdateGetWell();
            UpdateGetWellViewModel.GetWellSoonCard = Card;
            UpdateGetWellViewModel.CardDesigns = Designs;
            return View(UpdateGetWellViewModel);
        }
        [HttpPost]
        public ActionResult Update( string Message, string PatientName, string PatientEmail, string RoomNumber, int CardId, int CardDesignId)
        {
            GetWellSoonCard Card = db.GetWellSoonCards.Find(CardId);
            Card.Message = Message;
            Card.CardDesignId = CardDesignId;
            Card.PatientName = PatientName;
            Card.PatientEmail = PatientEmail;
            Card.RoomNumber = RoomNumber;
            db.SaveChanges();
            /*string query = "Update GetWellSoonCards set Message=@Message, PatientName=@PatientName, PatientEmail=@PatientEmail, CardDesignId=@CardDesignId, RoomNumber=@RoomNumber where CardId=@CardId";
            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@Message", Message);
            sqlparams[1] = new SqlParameter("@CardDesignId", CardDesignId);
            sqlparams[2] = new SqlParameter("@PatientName", PatientName);
            sqlparams[3] = new SqlParameter("@PatientEmail", PatientEmail);
            sqlparams[4] = new SqlParameter("@RoomNumber", RoomNumber);
            sqlparams[5] = new SqlParameter("@CardId", CardId);

            //Execute
            db.Database.ExecuteSqlCommand(query, sqlparams);*/
            Debug.WriteLine("I am trying to edit the card with the message " + Message);
            return RedirectToAction("/Show/" + CardId);
        }
        public ActionResult Delete(int id)
        {
            //find the card with that particular id
            GetWellSoonCard Card = db.GetWellSoonCards.Find(id);            
            /*GetWellSoonCard Card = db.GetWellSoonCards.SqlQuery("Select * from GetWellSoonCards where CardId = @id", new SqlParameter("@id", id)).FirstOrDefault();*/
            //To do: Look into inner join Linq
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
            GetWellSoonCard Card = db.GetWellSoonCards.Find(CardId);
            db.GetWellSoonCards.Remove(Card);
            db.SaveChanges();
            /*string query = "Delete from GetWellSoonCards where CardId = @CardID";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CardId", CardId);
            db.Database.ExecuteSqlCommand(query, sqlparams);*/

            //when deleting is done, admin will return to the total list
            //user will return to personal list
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("PersonalList");
            }
            //return RedirectToAction("List");
        }

    }
}