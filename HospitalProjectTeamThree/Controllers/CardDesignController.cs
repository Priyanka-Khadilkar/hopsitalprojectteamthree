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
//need this for pagination
using PagedList;

namespace HospitalProjectTeamThree.Controllers
{
    public class CardDesignController : Controller
    {
        private HospitalProjectTeamThreeContext db = new HospitalProjectTeamThreeContext();
        // GET: CardDesign
        public ActionResult Index()
        {
           return View();
            
        }
        [Authorize(Roles = "Admin, Editor")]
        //admin and editor can see the list of card designs
        public ActionResult List(int? page)
        {
            string query = "Select * from CardDesigns";
            List<CardDesign> CardDesigns = db.CardDesigns.SqlQuery(query).ToList();
            Debug.WriteLine("Iam trying to list all the cards");
            //return View(carddesigns);
            //allow 4 index at the page
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //passing the pagelist to the view
            return View(CardDesigns.ToPagedList(pageNumber, pageSize));
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string DesignName)
        {
            string query = "Insert into CardDesigns (DesignName) values(@DesignName)";
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = new SqlParameter("@DesignName", DesignName);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            Debug.WriteLine("I am tryting to add the card with the name " + DesignName);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Show (int id)
        {
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns where CardDesignId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card design id" + id);
            return View(Design);
        }
        [Authorize(Roles = "Admin, Editor")]
        public ActionResult Update (int id)
        {
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns where CardDesignId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card design id" + id);
            return View(Design);
        }
        [HttpPost]
        public ActionResult Update(string DesignName, HttpPostedFileBase CardDesignPic, int CardDesignId)
        {
            int HasPic = 0;
            string PicExt = "";

            if (CardDesignPic != null)
            {
                Debug.WriteLine("Something identified...");

                if (CardDesignPic.ContentLength > 0)
                {
                    Debug.WriteLine("Successfully Identified Image");

                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(CardDesignPic.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = CardDesignId + "." + extension;

                            //get a direct file path to ~/Content/Comics/{id}.{extension}
                            string path = Path.Combine(Server.MapPath("~/Images/GetWellCard/"), fn);

                            //save the file
                            CardDesignPic.SaveAs(path);
                            //if these are all successful then we can set these fields
                            HasPic = 1;
                            PicExt = extension;

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }
                    }
                }
            }
            string query = "Update CardDesigns set HasPic=@HasPic, DesignName=@DesignName, PicExt=@PicExt where CardDesignId=@CardDesignId";
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@HasPic", HasPic);
            sqlparams[1] = new SqlParameter("@DesignName", DesignName);
            sqlparams[2] = new SqlParameter("@PicExt", PicExt);
            sqlparams[3] = new SqlParameter("@CardDesignId", CardDesignId);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("Show/" + CardDesignId);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            CardDesign Design = db.CardDesigns.SqlQuery("Select * from CardDesigns where CardDesignId = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Debug.WriteLine("I am trying to show card design id" + id);
            return View(Design);
        }
        [HttpPost]
        public ActionResult Delete(int id, int CardDesignId)
        {
            string query = "Delete from CardDesigns where CardDesignId = @CardDesignID";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CardDesignId", CardDesignId);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
    }
}