///*
// * Renamed From MyAlabamaUtility.cs
// * Everything here copid from old app unless otherwise stated
// */
//using Snap.Proxies;

//namespace Snap.Services
//{
//    /// <summary>
//    /// Provides support functions for MyAlabama data access
//    /// </summary>
//    public static class MyAlabamaService
//    {
//        /// <summary>
//        /// Persists data fields to MyAlabama.
//        /// </summary>
//        /// <param name="currentUser">The current user.</param>
//        /// <param name="applicationId">The application id.</param>
//        /// <param name="fields">The fields.</param>
//        /// <param name="values">The values.</param>
//        /// <param name="recordStatus">The record status.</param>
//        public static void PersistToMyAlabama(Guid currentUser, Guid applicationId, string[] fields, string[] values, string recordStatus)
//        {
//            using (var svc = new MyAlabamaProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                svc.UploadRecord(currentUser,
//                    applicationId,
//                    System.Configuration.ConfigurationManager.AppSettings["MyAlabama.RecordType"],
//                    recordStatus,
//                    fields,
//                    values);
//            }
//        }

//        /// <summary>
//        /// Creates my alabama record.
//        /// </summary>
//        /// <param name="currentUser">The current user.</param>
//        /// <param name="applicationId">The application id.</param>
//        /// <param name="recordStatus">The record status.</param>
//        public static void CreateMyAlabamaRecord(Guid currentUser, Guid applicationId, string recordStatus)
//        {
//            using (MyAlabamaWebServiceProxy svc = new MyAlabamaWebServiceProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                svc.CreateBlankRecord(currentUser, applicationId, System.Configuration.ConfigurationManager.AppSettings["MyAlabama.RecordType"], recordStatus);
//            }
//        }

//        /// <summary>
//        /// Retreives my alabama token.
//        /// </summary>
//        public static TokenData RetrieveMyAlabamaToken(Guid tokenId)
//        {
//            //retrieve MyAlabama token
//            try
//            {
//                HttpContext context = HttpContext.Current;
//                using (var svc = new MyAlabamaWebServiceProxy(context, context.User.Identity.Name, context.Request.Path))
//                {
//                    if (tokenId != Guid.Empty)
//                    {
//                        TokenData tokenData = svc.VerifyUserToken(tokenId);
//                        if (tokenData != null)
//                        {
//                            return tokenData;
//                        }
//                    }
//                }
//                return null;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error Retreiving MyAlabama Token -- " + ex);
//            }
//        }

//        public static TokenData RetrieveOldMyAlabamaToken(Guid tokenId)
//        {
//            //retrieve MyAlabama token
//            try
//            {
//                HttpContext context = HttpContext.Current;
//                using (var svc = new MyAlabamaWebServiceProxy(context, context.User.Identity.Name, context.Request.Path))
//                {
//                    if (tokenId != Guid.Empty)
//                    {
//                        TokenData tokenData = svc.VerifyOldUserToken(tokenId);
//                        if (tokenData != null)
//                        {
//                            return tokenData;
//                        }
//                    }
//                }
//                return null;
//            }
//            catch (Exception ex)
//            {
//                throw new Exception("Error Retreiving MyAlabama Token -- " + ex);
//            }
//        }

//        /// <summary>
//        /// Gets my alabama field data.
//        /// </summary>
//        /// <param name="currentUser">The current user.</param>
//        /// <param name="fieldName">Name of the field.</param>
//        /// <returns></returns>
//        public static string GetMyAlabamaFieldData(Guid currentUser, string fieldName)
//        {
//            string result = string.Empty;
//            using (var svc = new MyAlabamaWebServiceProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                UserData[] fieldData = svc.GetFieldData(currentUser, false, fieldName, 0, 0);
//                if (fieldData.Length > 0) result = fieldData[0].Value;
//            }
//            return result;
//        }

//        /// <summary>
//        /// Updates my alabama application status.
//        /// </summary>
//        /// <param name="currentUser">The current user.</param>
//        /// <param name="applicationId">The application id.</param>
//        /// <param name="status">The status.</param>
//        public static void UpdateMyAlabamaApplicationStatus(Guid currentUser, Guid applicationId, string status)
//        {
//            string myIp = "130.160.68.11";

//            using (var svc = new MyAlabamaWebServiceProxy(myIp))
//            {
//                svc.UpdateRecordStatus(currentUser, applicationId, System.Configuration.ConfigurationManager.AppSettings["MyAlabama.RecordType"], status);
//            }
//        }

//        /// <summary>
//        /// Validates the advocate signature.
//        /// </summary>
//        /// <param name="userName">Name of the user.</param>
//        /// <param name="password">The password.</param>
//        public static string ValidateAdvocateSignature(string userName, string password)
//        {
//            using (var svc = new MyAlabamaWebServiceProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                return svc.ValidateUser(userName, password);
//            }
//        }

//        /// <summary>
//        /// Gets the advocate organization.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <returns></returns>
//        public static Organization GetAdvocateOrganization(string id)
//        {
//            using (var svc = new MyAlabamaWebServiceProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                return svc.GetAdvocateOrganizationById(id);
//            }
//        }

//        /// <summary>
//        /// Gets the user by id.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <returns></returns>
//        public static User GetUserById(Guid id)
//        {
//            using (var svc = new MyAlabamaWebServiceProxy(HttpContext.Current, HttpContext.Current.User.Identity.Name, HttpContext.Current.Request.Path))
//            {
//                return svc.GetUserByUserID(id);
//            }
//        }
//    }
//}
