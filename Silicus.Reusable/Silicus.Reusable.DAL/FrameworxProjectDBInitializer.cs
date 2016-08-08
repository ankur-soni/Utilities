﻿using Silicus.FrameworxProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silicus.FrameworxProject.DAL
{
    public class FrameworxProjectDBInitializer : CreateDatabaseIfNotExists<FrameworxProjectDatabaseContext>
    {
        protected override void Seed(FrameworxProjectDatabaseContext context)
        {
            context.Frameworxs.Add(new Frameworx { Id = 1, Title = "Angular JS Template",DemoLink= "https://www.google.co.in/",SourceCodeLink = "https://www.google.co.in/" , CategoryId = 1, HtmlDescription = "<h4>Angular JS Template</h4><p>This template demonstrate some useful guidelines to build an AngularJS / WebAPI project while providing a standard AngularJS application code structure.</P><ul><li>Features:<ul><li> UI pages fetching data through API.</li><li>Customized tooltip.</li><li>Directive for Phone number formatting.</li><li>Directive for SSN formatting.</li><li>Directive for Date of Joining(If you enter the date in this text box it will automatically converted in to MM / DD / YYYY format. 01012016 will be converted to 01 / 01 / 2016).</li></ul></li><li>Benefits:<ul><li>Saves 50 % time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"});
            context.Frameworxs.Add(new Frameworx { Id = 2, Title = "Mvc Template With Identity", DemoLink = "https://www.google.co.in/", SourceCodeLink = "https://www.google.co.in/", CategoryId = 1, HtmlDescription = "<h4>Mvc Template With Identity</h4><p>A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using Identity.</li><li>Both filter based and explicit logging is demonstrated..</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>" });
            context.Frameworxs.Add(new Frameworx { Id = 3, Title = "Mvc Template", DemoLink = "https://www.google.co.in/", SourceCodeLink = "https://www.google.co.in/", CategoryId = 1, HtmlDescription = "<h4>Mvc Template</h4><p> A template showcasing typical multi - layer architecture of an MVC web application.</P><ul><li>Features:<ul><li>Authentication(Form based) and Authorization using SQLMembershipProvider </li><li>Both filter based and explicit logging is demonstrated.</li><li>Filter based auditing is demonstrated.</li><li>Exception Handling (Different error view for different HTTP error code) is demonstrated.</li><li>User Account Management – Users and Roles can be created through UI.Supports Login, Forgot Password, Change Password, Reset Password, Auto logoff after x hours, etc.</li><li>Data Access Layer – Fully working and ready to use EntityFramework based library demonstrated. CRUD Operation with DB – End to End communication</li><li>Send Email</li><li>Deployment Technique – VS Publish profile – two profiles demonstrated with web.config transformations.</li><li>Unit test cases added for sample controller and classes.</li><li>LightInject is used for dependency injection.</li><li>AutoMapper is used for mapping entities to model.</li></ul></li><li>Benefits:<ul><li>Saves 70% time that is spent typically in Sprint 0 and 1.</li></ul></li></ul>"});
            context.Frameworxs.Add(new Frameworx { Id = 4, Title = "Silicus Frameworx Logger", DemoLink = "https://www.google.co.in/", SourceCodeLink = "https://www.google.co.in/", CategoryId = 2, HtmlDescription = "<h4>Silicus Frameworx Logger</h4><ul><li>A simple library that implements a Database Logger and a file based logger which is very simple to use.</li><li>Maintain log level and does logging asynchronously.</ li><li>Can be used in multithreaded environment.</li><li>Fail safe i.e. never throws, logs in windows event viewer.</ li><li>Extensible</li><li>Configurable</ li></ul>" });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 4,
                Title = "Silicus Frameworx Auditor",
                DemoLink = "https://www.google.co.in/",
                SourceCodeLink = "https://www.google.co.in/",
                CategoryId = 2,
                HtmlDescription = "<h4>Silicus Frameworx Auditor</h4><ul><li>A simple library that provides the ability to add business events into a database.</ li><li>AuditManager insert audit entry into SQL DB.</ li><li>Extensible</ li></ul>"
            });
            context.Frameworxs.Add(new Frameworx
            {
                Id = 4,
                Title = "Silicus Frameworx Utility",
                DemoLink = "https://www.google.co.in/",
                SourceCodeLink = "https://www.google.co.in/",
                CategoryId = 2,
                HtmlDescription = "<h4>Silicus Frameworx Utility</h4><ul><li>Guard - provides input validation against NULL and Empty for various datatypes.</ li><li>BlobService – Provides generic methods to convert an object into binary data back and forth.</li><li>RijndaelEncryptionHelper - provides the ability to encrypt or decrypt plain text.</ li></ul>"
            });
            context.Categories.Add(new Category { Id = 1, Name ="Templates"});
            context.Categories.Add(new Category { Id = 2, Name = "Components" });

        }
    }

}
