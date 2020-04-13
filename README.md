# hopsitalprojectteamthree
## Hospital Name: Temiskaming Hospital
## Golden rules (to team members):
  * Pull everything first
  * Update database
  * Add-migration
  * Hit build
  * Push everything
## Team members: 
* Priyanka 
* Alexa 
* Viet Phuong (Paul) Tran - n01400583
* Vitaly Bulyma  - n00224782
* Kshitija 
* Eseroghene
## Team members' feature and contribution"
### Paul: Log in system + Get Well Soon Card
  * Log in system
    * Description: Users will be classified into 3 types with different roles that they have on the site: Admin, Editor, Registered User
    * Files contributed: 
      * Model
        * AccountViewModels: add in the AdminRegister Model where only admin can add and assign role to users.
        * RoleViewModels: a class contains all the role name in the website
      * View:
        * AdminRegister (under Account folder): this is the interface allows admin to assign roles and register for employees.
        * Role folder: these interfaces allow admin to add, update, delete and view all the roles.
      * Controllers:
        * Account Controller: modify the Register function so anyone who register becomes Registered User. Add in the AdminRegister to allow admin to assign roles for different people
        * Role Controller: algorithm behind the Roles view and models.
  * Get Well Soon Card:
    * Description: Logged in user can see, add, update and delete a card for the patient who is now in the hospital
    * Files contributed:
      * Model:
        * GetWellSoonCard.cs, CardDesign.cs: These two are in a one to many relationship. One Card has one design but one designs can be applied in many cards
        * ShowGetWell.cs, PersonalListGetWell.cs, ListGetWell.cs, UpdateGetWell.cs, AddGetWellCard.cs: These viewmodels are used to display information in respecitve page
      * View:
        * CardDesign folder: showing all the card designs for admin and editor to view, add, edit and delete
        * GetWellSoonCard folder: 
          * List: a page displaying all cards for admin, editor to view
          * Personal List: a page displaying all cards for a particular users who logged
          * Index: a page for guests to logged in and create the card
          * Add, Delete, Show, Update: the interface for users to interact with the page with different actions.
       * Controllers: 
        * CardDesignController, GetWellSoonCardController: the algorithm behind the CardDesign's and GetWellSoonCard's views and models
### Vitaliy: Public Health Crisis Alert and Long-term Stay at the Hospital
  * Public Health Crisis Alert
![View of All Articles](/HospitalProjectTeamThree/Images/Room/Articles.png)

    * Description: This feature is only activated when the public health crisis is currently active, and has not been officially ended. All users will be able to see historical list of previous crises, and read articles related to each crisis.  If there is no active public health crisis, there will be no interaction with this feature by any users, and no access to it. To activate the alert on the main page (/Home/Index), the administrator of the page needs to log in, and navigate to /Crisis/Index where, as added security, they will be re-directed to appropriate page based on their permissions. When redirected, administrators can create new “Crisis” entry which will make this crisis active. To deactivate crisis alert, admin can update the crisis from the administrator interface. Administrators also able to add, edit, and delete articles accessible from /Article/Index, where they will be redirected to admin interface
    * Files Contributed:
      * Models:
        * Crisis.cs – represents information needed to describe the idea of “Crisis” entry, and list of all articles
        * Article.cs – represent information needed to describe an “Article” entry
      * ViewModels:
        * ShowCrisis.cs – allows to pass information about both article and crisis to same view
      * Views: 
        * Article:
          * Add.cshtml – administrator interface where they can add new article
          * Index.cshtml – the page which re-directs user to appropriate interface based on their permissions.
          * List.cshtml – Public list of all articles and crises list in the system
          * ListAdm.cshtml – Administrator interface in addition to see a list of all articles and crises, it displays controls that allow to update or delete the record
          * Update.cshtml – Administrator page, where they can update specific record selected from administrator list interface
          * ViewArticle.cshtml – Public page to read selected article from list of all articles
        * Crisis:
          * Add.cshtml - administrator interface where they can add new crisis, and automatically create an alert on main page
          * Index.cshtml – this page redirects user to appropriate public or administrator list of all crisis’s records
          * List.cshtml – public list of crises 
          * ListAdm.cshtml – Administrator view of crises list, where they have controls to update, edit, or delete record
          * Update.cshtml – Administrator page, where they can update specific record selected from administrator list interface
          * ViewCrisis.cs - Public page where users can see all articles related to the crisis
      * Controllers:
        * ArticleController.cs, CrisisControler.cs – enables functionality and interaction between models, views, and databases

![View of All Rooms](/HospitalProjectTeamThree/Images/Room/AllRooms.PNG)        
* Long-term Stay at the Hospital

    * Description: This feature is designed for hospital users to explore which rooms are available for long-term stay. The list of all rooms available from main menu from main navigation accessible at /Room/ShowAll. In addition to browsing rooms and read their descriptions, logged in users will be able to book a selected room for specified dates.
    
    * Files Contributed:
      * Models:
        * Room.cs – represents information needed to describe the idea of “Room”, also bookings associated with that room 
        * RoomBooking.cs – represent information needed to describe a “Booking” entry, includes room information and user information associated with it.
      * ViewModels: 
        * FeaturedRoom.cs – allows to pass information about both selected room and all rooms available to view
        * RoomBookings.cs - allows to pass information about both booked room and user who booked the room to one view
      * Views:
        * ShowAll.cshtml – Displays all room types available at the hospital
        * ShowOne.cshtml – Shows one room, selected from the list, with more details. If person viewing the page is not registered, it prompts to log in to book the room. 
        * RoomBooking.cshtml – Retrieves the information of logged in user, and room selected from the list to populate the room booking registration form. 
        * Confirm.cshtml – Displays information about the room and user associated with that room after booking the room.
        * AdminView.cshtml – Administrator interface, where all information about number of rooms, rooms occupancy, and occupants is displayed (*not completed, this view is not database driven at the moment)
      * Controllers:
        * RoomController.cs – enables functionality and interaction between models, views, and databases
      

