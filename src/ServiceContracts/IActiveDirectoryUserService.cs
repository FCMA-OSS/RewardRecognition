using FCMA.RewardRecognition.Core.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCMA.RewardRecognition.Infrastructure.ServiceContracts
{
    public interface IActiveDirectoryUserService
    {
        List<RewardUserDataModel> GetAllActiveDirectoryUsers();

        void ResolveDirectReports(List<RewardUserDataModel> users);

        void ResolveManagerUserNames(List<RewardUserDataModel> users);

        string LowerFormat(string str);

        List<FullNameUserName> DirectReportsFormat(List<string> list);

        string FullNameFormat(string fullNameFullString);

        string UserNameFormat(string userName);

        string PropertyValue(ResultPropertyValueCollection resultPropertyValueCollection);
        List<string> PropertyValueList(ResultPropertyValueCollection resultPropertyValueCollection);
    }
}
