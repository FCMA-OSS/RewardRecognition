using FCMA.RewardRecognition.Core.Models;
using FCMA.RewardRecognition.Infrastructure.ServiceContracts;
using System;
using System.Collections.Generic;

using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.Services
{
    public class ActiveDirectoryUserService : IActiveDirectoryUserService
    {
        private readonly IActiveDirectoryQueryContext _activeDirectoryQueryContext;

        public ActiveDirectoryUserService(IActiveDirectoryQueryContext activeDirectoryQueryContext)
        {
            _activeDirectoryQueryContext = activeDirectoryQueryContext;
        }

        [ExcludeFromCodeCoverage]
        public List<RewardUserDataModel> GetAllActiveDirectoryUsers()
        {
            var users = new List<RewardUserDataModel>();
            var adEntry = new DirectoryEntry(_activeDirectoryQueryContext.DirectoryEntry);
            var searcher = new DirectorySearcher(adEntry);

            //Properties of the active directory searcher set by the context
            searcher.SizeLimit = _activeDirectoryQueryContext.SizeLimit;
            searcher.PageSize = _activeDirectoryQueryContext.PageLimit;
            searcher.Filter = _activeDirectoryQueryContext.Filter;

            //Properties that will be returned within the result set
            searcher.PropertiesToLoad.Add("cn"); //UserFullName
            searcher.PropertiesToLoad.Add("userPrincipalName"); //UserName
            searcher.PropertiesToLoad.Add("directReports"); //DirectReports
            searcher.PropertiesToLoad.Add("mail"); //EmailAddress
            searcher.PropertiesToLoad.Add("manager"); //Manager
            searcher.PropertiesToLoad.Add("physicalDeliveryOfficeName"); //OfficeLocation
            searcher.PropertiesToLoad.Add("title"); //JobTitle

            var result = searcher.FindAll();

            //Iterate through the results and cast them into the RewardUserModel                  
            foreach (SearchResult r in result)
            {
                users.Add(new RewardUserDataModel
                {
                    UserFullName = PropertyValue(r.Properties["cn"]),
                    UserName = UserNameFormat(PropertyValue(r.Properties["userPrincipalName"])),
                    DirectReports = DirectReportsFormat(PropertyValueList(r.Properties["directReports"])),
                    EmailAddress = LowerFormat(PropertyValue(r.Properties["mail"])),
                    Manager = new FullNameUserName { FullName = FullNameFormat(PropertyValue(r.Properties["manager"])), UserName = string.Empty },
                    OfficeLocation = PropertyValue(r.Properties["physicalDeliveryOfficeName"]),
                    JobTitle = PropertyValue(r.Properties["title"])
                });
            }

            ResolveManagerUserNames(users);
            ResolveDirectReports(users);
            return users;

        }            

        public void ResolveDirectReports(List<RewardUserDataModel> users)
        {

            users.Where(w => w.DirectReports.Any()).ToList().ForEach(u =>
            {

                var directReports = users.Where(w => w.Manager.UserName == u.UserName).ToList();

                u.DirectReports.ForEach(dr =>
                {
                    var report = directReports.SingleOrDefault(s => s.UserFullName == dr.FullName);
                    if (report != null)
                        dr.UserName = report.UserName;
                });

            });
        }

        public void ResolveManagerUserNames(List<RewardUserDataModel> users)
        {
            users.ForEach(u =>
            {

                if (!string.IsNullOrEmpty(u.Manager.FullName))
                {
                    var potentialManagers = users.Where(w => w.UserFullName == u.Manager.FullName).ToList();
                    potentialManagers.ForEach(m =>
                    {

                        if (m.DirectReports.Any(a => a.FullName == u.UserFullName))
                        {
                            u.Manager.UserName = m.UserName;
                        }
                    });
                }
            });

        }

        [ExcludeFromCodeCoverage]
        public string LowerFormat(string str)
        {
            return str.ToLower();
        }

        public List<FullNameUserName> DirectReportsFormat(List<string> list)
        {
            var ret = new List<FullNameUserName>();
            list.ForEach(l =>
            {
                var fullName = new FullNameUserName { FullName = FullNameFormat(l), UserName = string.Empty };
                ret.Add(fullName);
            });

            return ret;
        }

        public string FullNameFormat(string fullNameFullString)
        {
            if (string.IsNullOrEmpty(fullNameFullString))
                return string.Empty;
            var split1 = fullNameFullString.Split(',');
            if (split1.Count() < 2)
                throw new ArgumentException(string.Format("Unexpected input could not parse first and last name from data. ({0})", fullNameFullString));
            var split2 = split1[0].Split('=');
            if (split2.Count() < 2)
                throw new ArgumentException(string.Format("Unexpected input could not parse first and last name from data. ({0})", fullNameFullString));
            return split2[1];
        }

        public string UserNameFormat(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentException(string.Format("No user name provided could not correctly parse. ({0}).", userName));
            var userNameParts = userName.Split('@');
            if (userNameParts.Count() > 1)
                return userNameParts[0].ToLower();
            return userName.ToLower();
        }

        [ExcludeFromCodeCoverage]
        public string PropertyValue(ResultPropertyValueCollection resultPropertyValueCollection)
        {

            if (resultPropertyValueCollection.Count != 0)
                return resultPropertyValueCollection[0].ToString();
            return string.Empty;

        }
        [ExcludeFromCodeCoverage]
        public List<string> PropertyValueList(ResultPropertyValueCollection resultPropertyValueCollection)
        {
            var propertyCount = resultPropertyValueCollection.Count;
            if (propertyCount != 0)
            {
                var list = new List<string>();
                for (var i = 0; i < propertyCount; i++)
                {
                    list.Add(resultPropertyValueCollection[i].ToString());
                }
                return list;
            }
            return new List<string>();

        }
    }
}
