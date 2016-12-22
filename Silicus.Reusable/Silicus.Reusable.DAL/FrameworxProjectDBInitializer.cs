using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL
{
    public class FrameworxProjectDBInitializer : DropCreateDatabaseIfModelChanges<FrameworxProjectDatabaseContext>
    {
        protected override void Seed(FrameworxProjectDatabaseContext context)
        {
            context.Frameworxs.Add(new Frameworx
            {
                Id = 1,
                Title = "Angular JS Template",
                CategoryId = 1,
                DemoLink = "https://github.com/Silicus-Online/Silicuslab/blob/master/AngularJSTemplate/ReadMe.docx",
                SourceCodeLink = "https://github.com/Silicus-Online/Silicuslab/tree/master/AngularJSTemplate",
                HtmlDescription = "<h4>Angular JS Template</h4><p>This template demonstrate some useful guidelines to build an AngularJS / WebAPI project while providing a standard AngularJS application code structure.</P><ul><li>Features:<ul><li> UI pages fetching data through API.</li><li>Customized tooltip.</li><li>Directive for Phone number formatting.</li><li>Directive for SSN formatting.</li><li>Directive for Date of Joining(If you enter the date in this text box it will automatically converted in to MM / DD / YYYY format. 01012016 will be converted to 01 / 01 / 2016).</li></ul></li><li>Benefits:<ul><li>Saves 50 % time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 2,
                Title = "Mvc Template With Identity",
                CategoryId = 1,
                DemoLink = "https://github.com/Silicus-Online/Silicuslab/blob/master/Silicus.MvcTemplateWithIdentity/How%20to%20use%20this%20template",
                SourceCodeLink = "https://github.com/Silicus-Online/Silicuslab/tree/master/Silicus.MvcTemplateWithIdentity",
                HtmlDescription = "<h4>Mvc Template With Identity</h4><p>A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using Identity.</li><li>Both filter based and explicit logging is demonstrated..</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 3,
                Title = "Mvc Template",
                CategoryId = 1,
                DemoLink = "https://github.com/Silicus-Online/Frameworx/blob/master/Silicus.MvcTemplate/How%20to%20use%20this%20template",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.MvcTemplate",
                HtmlDescription = "<h4>Mvc Template</h4><p> A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using SQLMembershipProvider </li><li>Both filter based and explicit logging is demonstrated.</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 4,
                Title = "Silicus Frameworx Logger",
                CategoryId = 2,
                DemoLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Logger",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Logger",
                HtmlDescription = "<h4>Silicus Frameworx Logger</h4><ul><li>A simple library that implements a Database Logger and a file based logger which is very simple to use.</li><li>Maintain log level and does logging asynchronously.</ li><li>Can be used in multithreaded environment.</li><li>Fail safe i.e. never throws, logs in windows event viewer.</ li><li>Extensible</li><li>Configurable</ li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 5,
                Title = "Silicus Frameworx Auditor",
                DemoLink = "https://github.com/Silicus-Online/Frameworx/blob/master/Silicus.FrameWorx.Auditor/Silicus.FrameWorx.Auditing.v11.suo",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Auditor",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Silicus Frameworx Auditor</h4><ul><li>A simple library that provides the ability to add business events into a database.</ li><li>AuditManager insert audit entry into SQL DB.</ li><li>Extensible</ li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 6,
                Title = "Silicus Frameworx Utility",
                DemoLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Utility",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Utility",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Silicus Frameworx Utility</h4><ul><li>Guard - provides input validation against NULL and Empty for various datatypes.</ li><li>BlobService – Provides generic methods to convert an object into binary data back and forth.</li><li>RijndaelEncryptionHelper - provides the ability to encrypt or decrypt plain text.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 7,
                Title = "Cloud Search",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cloud Search</h4><ul><li>Search framework for .NET based on Amazon Search 2013-01-01 API.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 8,
                Title = "Cloud Storage",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cloud Storage</h4><ul><li>Azure Storage framework provide access the Microsoft Azure Cloud Storage services including Blob, Queue, Table and File.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 9,
                Title = "Cryptography Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cryptography Framework</h4><ul><li>Cryptography library that wraps complex hashing algorithms for password hashing with quick and simple usage.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 10,
                Title = "WebAPI Client Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>WebAPI Client Framework</h4><ul><li>A simple wrapper around RestSharp and Json.NET.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 11,
                Title = "File Encryption",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>File Encryption</h4><ul><li>File Encryption is a file encryption library available for C# .NET framework that uses the industry standard Advanced Encryption Standard to easily and securely encrypt files.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 12,
                Title = "Cache Provider",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cache Provider</h4><ul><li>Cache Provider is caching abstraction layer for .NET written in C#. It supports various cache providers.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 13,
                Title = "Extension Methods",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Extension Methods</h4><ul><li>A Home of 500+ Extension for C# .net.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 14,
                Title = ".NET Logging Libraries",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>.NET Logging Libraries</h4><ul><li>Simple, pretty and powerful logger for .net.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 15,
                Title = "Interceptor",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Interceptor</h4><ul><li>A Framework to do the Logging and Intercepting Database Operations for Entity Framework.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 16,
                Title = "Simple Email",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Simple Email</h4><ul><li>A Simple SMTP Email Library with Email template engine to send templated emails.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 17,
                Title = "API Email",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>API Email</h4><ul><li>A Simple Emails library to send emails using Sendgrid, MailChimp etc.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 18,
                Title = "SMS Messaging",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Sms Messaging</h4><ul><li>A simple messaging library to send SMS using provider such as Twilo, Pilivo.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 19,
                Title = "PayPal Payment Processing",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>PayPal Payment Processing Framework</h4>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 20,
                Title = "Delimited File Reader",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Delimited File Reader</h4><ul><li>A powerful delimited file reader for reading the records line by line.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 21,
                Title = "Data Auditing for EF",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Data Auditing for Entity Framework</h4><ul><li>Data Auditing Framework Lets you easily implement auditing on your application entities.</li><li> An audit record is written whenever an update of delete operation is applied to an entity and stores the time of the change and the user making the change along with the original entity values. </li><li>Complete audit trail changes to your entity are being saved</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 22,
                Title = "Domain Validation Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Domain Validation Framework</h4><ul><li>A simple validation library made up of validation and lambda expression  to create rules that will help to validate the model classes.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 23,
                Title = "Application Configuration",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Application Configuration Framework</h4><ul><li>A Simple framework provides various data store to store the application related configuration.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 24,
                Title = "Transient Execution Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Transient Execution Framework</h4><ul><li>Transient Execution Framework can apply retry policies to operations that your application performs against database operations that may exhibit transient faults.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 25,
                Title = "Transient Fault Detection",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Transient Fault Detection Framework</h4><ul><li>Transient Fault Detection Framework Provides the transient error detection logic that can recognize transient faults when dealing with Windows Azure Caching, Windows Azure Service Bus, SQL Database, Windows Azure storage services.</ li></ul>"
            });

            context.Categories.Add(new FrameworxCategory { Id = 1, Name = "Templates" });
            context.Categories.Add(new FrameworxCategory { Id = 2, Name = "Components" });

            context.CodeTypes.Add(new CodeType { Id = 1, Name = "C#" });
            context.CodeTypes.Add(new CodeType { Id = 2, Name = "VB.NET" });
            context.CodeTypes.Add(new CodeType { Id = 3, Name = "Angular JS" });
            context.CodeTypes.Add(new CodeType { Id = 4, Name = "JAVA" });
            context.CodeTypes.Add(new CodeType { Id = 5, Name = "PHP" });


            context.ExtensionSolutions.Add(new ExtensionSolution { Id = 1, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 1, MethodName = "My Extension 1", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.ExtensionSolutions.Add(new ExtensionSolution { Id = 2, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 2, MethodName = "My Extension 2", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.ExtensionSolutions.Add(new ExtensionSolution { Id = 3, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 3, MethodName = "My Extension 3", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.ExtensionSolutions.Add(new ExtensionSolution { Id = 4, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 4, MethodName = "My Extension 4", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });


            context.OtherCodes.Add(new OtherCode { Id = 1, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 1, MethodName = "Code Method 1", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.OtherCodes.Add(new OtherCode { Id = 2, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 2, MethodName = "Code Method 2", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.OtherCodes.Add(new OtherCode { Id = 3, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 3, MethodName = "Code Method 3", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });
            context.OtherCodes.Add(new OtherCode { Id = 4, FrequentSearchedCount = 0, reviewerid = 3780, UserDisplayName = "Devendra Birthare", userid = 3780, CreationDate = DateTime.Now, CodeTypeId = 4, MethodName = "Code Method 4", Description = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for inheritance or the overhead of virtual method invocations, or to require implementors of an interface to implement either trivial or woefully complex functionality.A particularly useful scenario the feature operates on an which there no concrete implemention or a useful implementation is not provided by the class library author, e.g.such often the libraries that provide developers a plugin architecture or similar functionality.Consider the following code and suppose it the only code contained Nevertheless, every implementor of the ILogger gain the ability to write a formatted, just by including a MyCoolLogger statement, without having to implement it once and without being required to subclass a provided implementation of ILogger.", ExampleUsage = "This is Example usage.", Code = "However, extension methods allow features to be implemented once in ways that enable reuse without the need for" });

            context.ProductBacklogs.Add(new ProductBacklog { Id = 1, Title = "Project 1", Description = "Small Description for Project 1", Owener = "Devendra Birthare", Status = "Pending", Priority = "Low", AssignedTo = "Devendra Birthare", ReceivedBy = "Devendra Birthare", Age = 1 });
            context.ProductBacklogs.Add(new ProductBacklog { Id = 2, Title = "Project 2", Description = "Small Description for Project 2", Owener = "Devendra Birthare", Status = "Pending", Priority = "High", AssignedTo = "Devendra Birthare", ReceivedBy = "Devendra Birthare", Age = 1 });
            context.ProductBacklogs.Add(new ProductBacklog { Id = 3, Title = "Project 3", Description = "Small Description for Project 3", Owener = "Devendra Birthare", Status = "Pending", Priority = "Medium", AssignedTo = "Devendra Birthare", ReceivedBy = "Devendra Birthare", Age = 1 });

            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 1,
                Name = "Shailendra Birthare"

            });
            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 2,
                Name = "Shailendra Birthare"

            });
            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 3,
                Name = "Shailendra Birthare"

            });
            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 4,
                Name = "Shailendra Birthare"

            });
            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 5,
                Name = "Shailendra Birthare"

            });
            context.FrameworxCredits.Add(new FrameworxCredits
            {
                Id = 1,
                FrameworxId = 6,
                Name = "Shailendra Birthare"

            });


        }
    }

}