# DevMvcComponent #

Written by [Alim Ul Karim](https://github.com/aukgit "Alim Ul Karim (Github account)") | Actually development started on January 2014

A nuget plugin for ASP.NET MVC developers , to reduce development time, it contains many must have features like sending emails, sending email on any exception, holding caching , cookies, pagination (not only pagination, with total count caching) and many more.

Reach [Alim Ul Karim](https://github.com/aukgit "Alim Ul Karim (Github account)") at [me@alimkarim.com](me@alimkarim.com "email alim ul karim")

## Install the nuget package:
![nuget installation code](https://raw.githubusercontent.com/aukgit/DevMVCComponent/001b38c94e354037c37e3eac3ee1603da4dd5cde/Images/nuget.png)

    Install-Package DevMVCComponent

### Setting up the component at your global asax file. 

However, it can be also used inside console or windows forms or any other app , just configure before running for the first time.

    protected void Application_Start()
    {
	   	//... all other codes
	    DevMvcComponent.Starter.Setup(...)
    }
   

### One way to setup (note that by default sending emails will be async, however we can disable it in this approach):

    var mailer = new DevMvcComponent.Mailer. CustomMailConfig(senderEmail, senderPassword, hostName, senderPort, isSSL);
	// to have an non-async mailer, use this
	// mailer.SendAsynchronousEmails = false;
    DevMvcComponent.Starter.Setup("Application Name", "Primary developer email", System.Reflection.Assembly.GetExecutingAssembly(), mailer);

### Another way to setup(note that by default sending emails will be async):

    DevMvcComponent.Starter.Setup("Application Name", "Primary developer email", System.Reflection.Assembly.GetExecutingAssembly(), senderEmail, senderPassword, hostName, senderPort, isSSL);

### Mailer can also be created easily for gmail:

    var gmailer = new GmailConfig("you@gmail.com", "password"); // by default port is 587 and SSL secure.
    var gmailer2 = new GmailConfig("you@gmail.com", "password", "smtp.gmail.com", 587); // change ports as well.

Any mailer configs ( please make sure that you import : DevMvcComponent.Enums, DevMvcComponent.Mailer ) :

    gmailer.EnableSsl = true;
    gmailer.Port = 587;
    gmailer.Host = "smtp.gmail.com";
    gmailer.SendAsynchronousEmails = true;

To send email through mailer ( please make sure that you import : DevMvcComponent.Enums, DevMvcComponent.Mailer ) :
    
    gmailer.QuickSend("sendingto@gmail.com", "Subject", "HTML body"); // an email will be send to the "sendingto@gmail.com" async style, which can be modified via gmailer.SendAsynchronousEmails property.

To send carbon copy email through mailer ( please make sure that you import : DevMvcComponent.Enums, DevMvcComponent.Mailer ) :
    
    gmailer.QuickSend("sendingto1@gmail.com,sendingto2@gmail.com", "Subject", "HTML body", MailingType.CarbonCopy, searchForCommas: true); // an email will be send to "sendingto@gmail.com" and "sendingto2@gmail.com" as a carbon-copy and async style.
To send blind carbon copy email through mailer ( please make sure that you import : DevMvcComponent.Enums, DevMvcComponent.Mailer ) :
    
    gmailer.QuickSend("sendingto1@gmail.com,sendingto2@gmail.com", "Subject", "HTML body", MailingType.MailBlindCarbonCopy, searchForCommas: true); // an email will be send to "sendingto@gmail.com" and "sendingto2@gmail.com" as a carbon-copy and async style.

###Handle any exception through an email (these will be sent based on the configuration done in the starter):

    try
    {
    	... done anything...
    } catch (Exception ex)
    {
    	DevMvcComponent.Starter.Error.HandleBy(ex,"method name"); // email will be sent to developer with all stack trace report via the mailer instanticated at the Setup();
		// user should be notified by nice message by sending -1 or anything else.
    }   
###Database exception handling using Entityframework, assume that we have an entity object which is failed during the transaction period:

    try
    {
    	... saving things to database ...
    } catch (Exception ex)
    {
    	DevMvcComponent.Starter.Error.HandleBy(ex,"method name", EnityObject); // email will be sent to developer with all stack trace report via the mailer instanticated at the Setup();
		// if any meaningful subject requires then 
    	// DevMvcComponent.Starter.Error.HandleBy(ex,"method name", "Email subject " , EnityObject); // email will be sent with this entity information 
		// user should be notified by nice message by sending -1 or anything else.
    }   

### Database pagination (with caching for total counting):

Every time executing the Entities.Count() operation is a waste of cpu clock cycles as well as database processing. Thus this framework stores that info in the cache and don't run the count next time until the cache expires.

Direct pagination on IQueryable ( must understand the differene between IQueryable and IEnumerable )

**Must order by (a clustered index would be better for performance) before pagination.**

	using DevMvcComponent.Pagination;

    var products = db.Products
    .Select(n => new {
        n.ProductID,
        n.ProductName,
        n.Dated
    })
    .OrderBy(n=> n.ProductID); // Must order by (a clustered index would be better for performance) before pagination.

	if (page <= 0) {
	    page = 1;
	}
	var pageInfo = new PaginationInfo() {
	    PageNumber = page,
	    PagesExists = null,
	    ItemsInPage = 30
	};
	var paged = products.GetPageData(pageInfo, cacaheName: "Products.Get.Count");
	var newPaged = products.ToList().Select(n => new {
	    n.ProductID,
	    n.ProductName,
	    Dated = n.Dated.ToString("dd-MMM-yyyy")
	});

Examples : 

- [jQuery Server Side Combo Project](http://bit.ly/1OnTnyW)   ([code file](http://bit.ly/1OnTquy "Code file for pagination"))  
- [We Review App](http://bit.ly/1OnTJFM)   ([code file](http://bit.ly/1OnTI4B "Code file for pagination"))  
- [Server side pagination unordered list generate](http://bit.ly/1CRFvfK "Server side HTML ul list for SEO optimization")  [ **any js based pagination generate will not saved search engines, so it must be generated from server side** ]

## Difference Between IQueryable and IEnumerable

Unlike List<> (in memory object) generic type IQueryable<>(lazy) or IEnumerable<>(lazy) are lazy that means it load the data when needed it.

Example:

	var arr = new int[] { 1, 2, 3, 4, 10, 5 };
    var greaterThanFive = arr.Where(singleItem => singleItem > 5); // this doesn't get executed until it is needed. And it is an IEnumerable type of list right now.
	// when we execute 
	foreach(var item in greaterThanFive) {...} // now it got executed and give us emurated data. So far IQueryable and IEnumerable does the same thing.

So far IQueryable and IEnumerable does the same thing in terms of lazy operation.
***However , IEnumerable is lazy for C# objects and IQueryable is lazy for database objects.***

Example 1 ( after that if we do pagination there is no value of it ) :	

    using(var db = new DbContext()){
    	//using removes db from memory when the operation is done.
    	var query1 = db.Table.Where(row=> row.Id > 50); // IQueryable<>  query doesn't run yet.
    	var query2 = query1.Where(row=> row.Id < 100); // IQueryable<> query doesn't run yet.
    	IEnumerable<Table> results = query2.AsEnumerable(); // query runs on the database and now we get plain C# objects. "Select * from Table Where id > 50 AND id < 100;"
    	// after that if we do pagination there is no value of it.
    }


Example 2 ( after that if we do pagination there is no value of it ) :	

    using(var db = new DbContext()){
    	//using removes db from memory when the operation is done.
    	var query1 = db.Table.Where(row=> row.Id > 50); // IQueryable<> query doesn't run yet.
    	var query2 = query1.Where(row=> row.Id < 100); // IQueryable<> query doesn't run yet.
    	IEnumerable<Table> results = query2.ToList(); // query runs on the database and now we get plain C# objects. "Select * from Table Where id > 50 AND id < 100;"
    	// after that if we do pagination there is no value of it.
    }

**In summary, there is no benefit of having database pagination with IEnumerable or IList or any other list type , if it is not IQueryable<>.**