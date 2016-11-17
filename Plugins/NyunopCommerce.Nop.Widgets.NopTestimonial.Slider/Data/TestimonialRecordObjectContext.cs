using Nop.Core;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace NyunopCommerce.Nop.Widgets.NopTestimonial.Slider.Data
{
    public class TestimonialRecordObjectContext : DbContext, IDbContext
    {
        public TestimonialRecordObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #region Implementation of IDbContext

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TestimonialRecordMap());

            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var dbScript = "IF OBJECT_ID('Testimonials') IS NOT NULL DROP TABLE Testimonials";
            Database.ExecuteSqlCommand(dbScript);
            SaveChanges();

            Database.SetInitializer<TestimonialRecordObjectContext>(null);

            Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
            SaveChanges();

            //parth
            //string macadd = GetMAC();
            //var file = System.Web.HttpContext.Current.Server.MapPath("~/Themes/DefaultClean/DBScripts/") + macadd + ".txt";
            //if (System.IO.File.Exists(file))
            //{
            //    System.IO.StreamReader datas = new System.IO.StreamReader(file);
            //    StringBuilder resultScript = new StringBuilder(string.Empty);
            //    resultScript.AppendLine(datas.ReadToEnd());
            //    resultScript.AppendLine("ALTER TABLE Testimonials ADD Test bit NULL Default 1 with values");
            //    Database.SetInitializer<TestimonialRecordObjectContext>(null);
            //    //foreach (char line in datas.ReadToEnd())
            //    //{
            //    //    dbScript += line;    
            //    //}
            //    //using (System.IO.StreamReader reader = new System.IO.StreamReader(file))
            //    //{
            //    //    dbScript += reader.ReadLine();
            //    //}
            //    System.IO.StreamReader reader = new System.IO.StreamReader(file);
            //    //foreach (string lines in System.IO.File.ReadLines(file))
            //    //{
            //    //    dbScript += lines;    
            //    //}
            //    string linecheck = "some";
            //    while (linecheck != null)
            //    {
            //        linecheck = reader.ReadLine();
            //        dbScript += reader.ReadLine();
                    
            //    }
            //    dbScript += "ALTER TABLE Testimonials ADD Test bit NULL Default 1 with values";
            //    //System.Data.Entity.Database database = new System.Data.Entity.Database();
            //    Database.ExecuteSqlCommand(dbScript);
            //    SaveChanges();
            //    datas.Close();
            //}
            //parth
        }
        private string GetMAC()
        {
            string macAddresses = "";

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }

        public void Uninstall()
        {

            //var dbScript = "DROP TABLE MyTestimonial";
            Database.SetInitializer<TestimonialRecordObjectContext>(null);
            var dbScript = "IF OBJECT_ID('Testimonials') IS NOT NULL DROP TABLE Testimonials";
            Database.ExecuteSqlCommand(dbScript);
            SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public System.Collections.Generic.IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }
        public void Detach(object data)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Gets or sets a value indicating whether proxy creation setting is enabled (used in EF)
        /// </summary>
        public bool ProxyCreationEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether auto detect changes setting is enabled (used in EF)
        /// </summary>
        public bool AutoDetectChangesEnabled { get; set; } 

    }
}
