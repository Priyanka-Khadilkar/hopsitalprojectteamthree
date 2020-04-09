# hopsitalprojectteamthree
## Hospital Name: Temiskaming Hospital
## Golden rules (to team members):
  * Pull everything first
  * Update database
  * Add-migration
  * Hit build
  * Push everything
## Team members: Priyanka, Alexa, Paul, Vitaly, Kshitija, Eseroghene
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
          
        
      
