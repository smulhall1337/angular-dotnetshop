namespace Infrastructure.Services
{
    public class AppSubmitterLoggerService
    {

        public IEnumerable<ApplicationSubmitterLogger> GetApplicationSubmitterLoggers()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            return db.ApplicationSubmitterLogger.AsEnumerable();
        }

        public ApplicationSubmitterLogger GetApplicationSubmitterLogger(int id)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            ApplicationSubmitterLogger applicationsubmitterlogger = db.ApplicationSubmitterLogger.Find(id);
            if (applicationsubmitterlogger == null)
            {
                // throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return applicationsubmitterlogger;
        }

        public ApplicationSubmitterLogger GetLastApplication()
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime today = DateTime.Now;
            ApplicationSubmitterLogger applicationsubmitterlogger = db.ApplicationSubmitterLogger.Where(x => x.EntryDate <= today).FirstOrDefault();
            if (applicationsubmitterlogger == null)
            {
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return applicationsubmitterlogger;
        }

        public IEnumerable<ApplicationSubmitterLogger> GetLogsBySourceName(string source)
        {
            using FoodAssistanceDatabaseContext db = new FoodAssistanceDatabaseContext();
            DateTime today = DateTime.Now;
            IEnumerable<ApplicationSubmitterLogger> applicationsubmitterlogger = db.ApplicationSubmitterLogger.Where(x => x.AppSource == source).AsEnumerable();
            if (applicationsubmitterlogger == null)
            {
                //throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return applicationsubmitterlogger;
        }
    }
}
