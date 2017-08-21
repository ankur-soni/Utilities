using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Data.Entity;
using System.Data;

using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;


namespace Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
       private IPDEntities _db;
              
       
       public RepositoryBase()
       {
           _db = new IPDEntities();       
       }
        
       public IEnumerable<T> GetAll(T obj, string[] param, string spName)
       {
           try
           {
               var data = _db.Set<T>().ToList();
               return data;
           }
           catch (Exception)
           {
               throw;
           } 
       }


       public bool Insert(T obj, string[] param, string spName)
       {
           _db.Set<T>().Add(obj);
           
           bool status = Save();
           return status;
       }
        
       public T GetById(object Id)
       {

          return _db.Set<T>().Find(Id);           
       }

       public bool Update(T obj, string[] param, string spName)
       { 

           //_db.Entry(obj).State = System.Data.Entity.EntityState.Detached;
           _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
           bool status = Save();
           return status;


       }
        
       //public void Save()
       //{

       //    try
       //    {

       //    }
       //    catch (DbUpdateConcurrencyException ex)
       //    {
             

       //        // Update the values of the entity that failed to save from the store 

       //    } 


       //    try
       //    {

       //    }
       //    catch (OptimisticConcurrencyException)
       //    {



       //    }
       //            }

       public bool Save()
       {

           try
           {
               _db.SaveChanges();
               
           }
           catch (DbUpdateConcurrencyException ex)
           {

               // Update the values of the entity that failed to save from the store 
               var entry = ex.Entries.Single();
               entry.OriginalValues.SetValues(entry.GetDatabaseValues());

           }
           catch (DbEntityValidationException dbEx)
           {
               foreach (var validationErrors in dbEx.EntityValidationErrors)
               {
                   foreach (var validationError in validationErrors.ValidationErrors)
                   {
                       Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                   }
               }
           }
           catch (Exception ex)
           {
               return false;
           }

           return true;
       }


       public bool UpdateById(object Id)
       {
           var activityInDb = _db.Set<T>().Find(Id);

           // Activity does not exist in database and it's new one
           if (activityInDb == null)
           {

           }


           return true;
       }


       public bool check(int Id)
       {
           return true;
       }    

      
    }
}
