using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL
{
    public class FrameworxProjectDBInitializer : DropCreateDatabaseAlways<FrameworxProjectDatabaseContext>
    {
        protected override void Seed(FrameworxProjectDatabaseContext context)
        {
            context.Frameworxs.Add(new Frameworx
            {
                Id = 1,
                Title = "Angular JS Template",
                CategoryId = 1,
                DemoLink = "https://github.com/Silicus-Online/Silicuslab/blob/master/AngularJSTemplate/ReadMe.docx",
                OwnerId = 3780,
                SourceCodeLink = "https://github.com/Silicus-Online/Silicuslab/tree/master/AngularJSTemplate",
                HtmlDescription = "<h4>Angular JS Template</h4><p>This template demonstrate some useful guidelines to build an AngularJS / WebAPI project while providing a standard AngularJS application code structure.</P><ul><li>Features:<ul><li> UI pages fetching data through API.</li><li>Customized tooltip.</li><li>Directive for Phone number formatting.</li><li>Directive for SSN formatting.</li><li>Directive for Date of Joining(If you enter the date in this text box it will automatically converted in to MM / DD / YYYY format. 01012016 will be converted to 01 / 01 / 2016).</li></ul></li><li>Benefits:<ul><li>Saves 50 % time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 2,
                Title = "Mvc Template With Identity",
                CategoryId = 1,
                OwnerId = 3780,
                DemoLink = "https://github.com/Silicus-Online/Silicuslab/blob/master/Silicus.MvcTemplateWithIdentity/How%20to%20use%20this%20template",
                SourceCodeLink = "https://github.com/Silicus-Online/Silicuslab/tree/master/Silicus.MvcTemplateWithIdentity",
                HtmlDescription = "<h4>Mvc Template With Identity</h4><p>A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using Identity.</li><li>Both filter based and explicit logging is demonstrated..</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 3,
                Title = "Mvc Template",
                CategoryId = 1,
                OwnerId = 3780,
                DemoLink = "https://github.com/Silicus-Online/Frameworx/blob/master/Silicus.MvcTemplate/How%20to%20use%20this%20template",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.MvcTemplate",
                HtmlDescription = "<h4>Mvc Template</h4><p> A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using SQLMembershipProvider </li><li>Both filter based and explicit logging is demonstrated.</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 4,
                Title = "Silicus Frameworx Logger",
                CategoryId = 2,
                OwnerId = 3780,
                DemoLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Logger",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Logger",
                HtmlDescription = "<h4>Silicus Frameworx Logger</h4><ul><li>Logging library is a tool to help the programmer output log statements to a variety of output targets.</li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 5,
                Title = "Silicus Frameworx Auditor (EF)",
                DemoLink = "https://github.com/Silicus-Online/Frameworx/blob/master/Silicus.FrameWorx.Auditor/Silicus.FrameWorx.Auditing.v11.suo",
                SourceCodeLink = "https://github.com/Silicus-Online/Frameworx/tree/master/Silicus.FrameWorx.Auditor",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Silicus Frameworx Auditor (EF)</h4><p>Audit trail and data versioning framework for using Entity Framework 6.0 or above.</p>"
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
                Title = "Cloud Search (AWS)",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cloud Search (AWS)</h4>" +
                "<ul><li>Amazon CloudSearch is a managed service in the AWS Cloud that makes it simple and cost-effective to set up, manage, and scale a search solution for your website or application. With Amazon CloudSearch, you can quickly add rich search capabilities to your website or application. With Amazon CloudSearch you can search large collections of data such as web pages, document files, forum posts, or product information. Cloud Search framework for .NET  based on Amazon Cloud Search 2013-01-01 API. Amazon CloudSearch to index and search both structured data and plain text. </li>" +
                "<li>Amazon CloudSearch Features:</li>" +
                "<ol>" +
                "<li>Full text search with language-specific text processing</li>" +
                "<li>Boolean search</li>" +
                "<li>Prefix searches</li>" +
                "<li>Range searches</li>" +
                "<li>Term boosting</li>" +
                "<li>Faceting</li>" +
                "<li>Highlighting</li>" +
                "<li>Autocomplete Suggestions</li>" +
                "</ol>" +
                "<li>APIS Supported</li>" +
                "<li>Document Service API - Yes</li>" +
                "<li>Search API - Yes</li>" +
                "<li>Configuration API - No</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 8,
                Title = "Cloud Storage",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cloud Storage</h4>" +
                "<ul><li>Microsoft Azure Cloud Storage framework for .NET allows you to build Azure applications that take advantage of scalable cloud computing resources.</li>" +
                "<li>This repository contains the Cloud storage framework for .NET  based on Azure Storage .NET SDK. For documentation of the complete Azure SDK, please see the Microsoft Azure .NET Developer Center.</li>" +
                "<li>Features" +
                "<ul>" +
                "<li>Tables" +
                "<ol>" +
                "<il>Create/Delete Tables</li>" +
                "<il>Query/Create/Read/Update/Delete Entities</li>" +
                "</ol>" +
                "</li>" +
                "<li>Blobs" +
                "<ol>" +
                "<il>Create/Read/Update/Delete Blobs</li>" +
                "</ol>" +
                "</li>" +
                "<li>Files" +
                "<ol>" +
                "<il>Create/Update/Delete Directories</li>" +
                "<il>Create/Read/Update/Delete Files</li>" +
                "</ol>" +
                "</li>" +
                "<li>Queues" +
                "<ol>" +
                "<il>Create/Delete Queues</li>" +
                "<il>Insert/Peek Queue Messages</li>" +
                "<il>Advanced Queue Operations</li>" +
                "</ol>" +
                "</li>" +
                "</ul></li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 9,
                Title = "Cryptography Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cryptography Framework</h4>" +
                "<ul><li>Cryptography library that wraps complex hashing algorithms for  Password hashing with quick and simple usage. Password storage is a large topic in application security. If a security failure occurs, and the database is stolen, the passwords of the users are some of the most important data stored. Given the state of contemporary authentication, they do not need to be stored in plain text, so they should not. A hashed representation of the password, using a contemporary encryption algorithm and process, is the accepted way to store a password in today's systems.</li>" +
                "<li>Ref:<a> https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2 </a></li>" +
                "<p>PBKDF2 - It uses a pseudorandom function and a configurable number of iterations to derive a cryptographic key from a password. Because this process is difficult to reverse (similar to a cryptographic hash function) but can also be configured to be slow to compute, key derivation functions are ideally suited for password hashing use cases.</li>" +
                "<li>Salt helps make constructing attacker tools such as rainbow, look-up and reverse tables harder if not, in a lot of cases, impossible  to use. Making these ingredients secret, but doing so can complicate a setup as well.   For example, if you find yourself attempting to use the same salt for all password hashing in order to hide the salt in a config file or source code, you will have just violated one of the first rules of using a salt – the importance of using a unique and random salt per password hash.  And observing the Kerckhoffs's principle (<a>https://en.wikipedia.org/wiki/Kerckhoffs%27s_principle</a>) your supposedly hidden salt is mostly likely not all that hidden.</li>" +
                "<li>The library is  implementing the PBKDF2 function in a .NET application with the use of the Rfc2898DerivedBytes System.Security.Cryptography library.</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 10,
                Title = "WebAPI Client Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>WebAPI Client Framework</h4><ul><li>A wrapper for RestSharp to provide more domain-oriented design for fetching restful data.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 11,
                Title = "File Encryption",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>File Encryption</h4>" +
                "<ul>" +
                "<li>File Encryption is a file encryption library available for C# .NET framework that uses the industry standard Advanced Encryption Standard to easily and securely encrypt files.</li>" +
                "<li>Using a powerful 256-bit encryption algorithm, File Encryption can safely secure your most sensitive files. Once a file is encrypted, you do not have to worry about a person reading your sensitive information, as an encrypted file is completely useless without the password. It simply cannot be read.</li>" +
                "</ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 12,
                Title = "Cache Provider",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Cache Provider</h4>" +
                "<ul><li>Cache Provider for .NET helps enterprise applications leverage the benefits of the established Provider Model in .NET while enabling Cache for the applications. Driven by configuration, it abstracts application code from the Cache Store and provides an API which is easy to use.</p><p>Cache provider is easy to use and start with, but it is also just a few lines of configuration to use a more complex caching strategy without you having to change your code.</li>" +
                "<li>Start with a simple in-memory approach and scale out later to Redis or other distributed solutions.</li>" +
                "<li>Provider Supported." +
                "<ol>" +
               "<li>Redis</li>" +
               "<li>Memcached</li>" +
                "<li>Dictionary Cache</li>" +
               "<li>No Cache</li>" +
                "</ol></li></ul>"
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
                HtmlDescription = "<h4>Interceptor</h4><ul><li>Interception Framework is a library to provide interception for DbCommand in entity framework.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 16,
                Title = "SMTP Emailer",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>SMTP Emailer</h4><ul><li>Simple SMTP Service wrapper with powerful xml based template engine for sending the customized email from .NET.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 17,
                Title = "SendGrid Email",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>SendGrid Email</h4><ul><li>Sendgrid  APIs are based on HTTP methods, which make it easy to write applications for sending the transaction email . Sendgrid framework enables seamless integration of outbound email Apis in your application.</ li></ul>"
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
                HtmlDescription = "<h4>PayPal Payment Processing Framework</h4>" +
                "<ul>" +
                "<li>Payment Gateway Framework (PGF) is a framework that allows programmers a uniform and comprehensive way of developing applications that can perform payment activities in the application. It supports both PayPal Standard & Direct Payment methods.</li>"
                + "</ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 20,
                Title = "CSV Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>CSV Framework</h4>" +
               "<ul><li>Delimited File Framework is a framework that allows programmers a uniform and comprehensive way of developing applications that can perform read and write operations on delimited file.</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 21,
                Title = "Auditing (EF)",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Auditing (EF)</h4><ul><li>Data Auditing Framework Lets you easily implement auditing on your application entities.</li><li> An audit record is written whenever an update of delete operation is applied to an entity and stores the time of the change and the user making the change along with the original entity values. </li><li>Complete audit trail changes to your entity are being saved</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 22,
                Title = "Domain Validation Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Domain Validation Framework</h4><ul><li>Domain Validation Framework with  a fluent interface and lambda expressions for building validation rules.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 23,
                Title = "Configuration Framework",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Configuration Framework</h4><ul><li>Application Configuration framework library to read configuration from DB or External configuration file.</ li></ul>"
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
                Title = "Transient Fault Handler",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Transient Fault Handler</h4><ul><li>The Transient Error Handler can apply retry policies to operations that your application performs against services that may exhibit transient faults. This makes it easier to implement consistent retry behavior for any transient faults that may affect your application. The Transient Error Handler uses detection strategies to identify all known transient error conditions. Use one of the built -in detection strategies for SQL Azure, Microsoft Azure Storage, Azure Caching, or the Azure Service Bus. Define detection strategies for any other services that application uses.</ li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 26,
                Title = "Application Framework (WPF)",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Application Framework (WPF)</h4>" +
                "<ul><li>Silicus Application Framework for Windows Presentation Foundation. It is a lightweight Framework that helps you to create well-structured WPF Applications. It supports you in applying a Layered Architecture and the Model-View-ViewModel (aka MVVM, M-V-VM, PresentationModel) pattern.</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 27,
                Title = "Dropbox",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Dropbox<h4>" +
                    "<ul><li>Drobox framework allows developers to work with files in Dropbox, including advanced functionality like full-text search, thumbnails, and sharing.</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 28,
                Title = "Extensions",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Extensions<h4>" +
                  "<ul><li>A collection of 500 + Extensions Methods, to sped up the development and provide the extension methods on core objects to perform advanced operation not supported by .net framework.</li></ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 39,
                Title = "Google Drive",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Google Drive<h4>" +
               "<ul>" +
               "<li>The Google API client library for .NET enables access to Google APIs such as Drive. The core functionality of Drive apps is to download and upload files in Google Drive. However, the Drive platform provides a lot more than just storage.</li>" +
               "</ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 30,
                Title = "Identity Provider",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Identity Provider<h4>" +
             "<ul>" +
             "<li>Access Control Service or ACS), an identity provider is a service that authenticates user or client identities and issues security tokens that ACS consumes.</li>" +
             "</ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 31,
                Title = "WebMail MVC",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>WebMail MVC<h4>" +
                 "<ul>" +
                 "<li>MvcMailer provides you with an ActionMailer style email sending  for ASP.Net MVC 3+. Produce professional looking emails composed of mvc master pages and views with ViewBag.</li>" +
                 "</ul>"
            });


            context.Frameworxs.Add(new Frameworx
            {
                Id = 32,
                Title = "One Drive",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>One Drive<h4>" +
                 "<ul>" +
                 "<li>One Drive framework provides a set of HTTP services to connect your application to files and folders in Office 365 and SharePoint Server 2016.</li>" +
                 "</ul>"
            });


            context.Frameworxs.Add(new Frameworx
            {
                Id = 34,
                Title = "Plivo Messaging",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Plivo Messaging<h4>" +
               "<ul>" +
               "<li>Plivo APIs are based on HTTP methods, which make it easy to write applications. Plivo framework enables seamless integration of outbound SMS Apis in your application.</li>" +
               "</ul>"
            });

            context.Frameworxs.Add(new Frameworx
            {
                Id = 35,
                Title = "Twilio Messaging",
                CategoryId = 2,
                OwnerId = 3780,
                HtmlDescription = "<h4>Twilio Messaging<h4>" +
               "<ul>" +
               "<li>Twilio APIs are based on HTTP methods, which make it easy to write applications. Plivo framework enables seamless integration of outbound SMS Apis in your application.</li>" +
               "</ul>"
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

            context.FrameworxUsers.Add(new FrameworxUser { Id = 1, EmailAddress = "Amit.Khandelwal@silicus.com" });


        }
    }

}