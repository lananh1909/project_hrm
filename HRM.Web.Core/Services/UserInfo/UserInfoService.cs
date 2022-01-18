using HRM.Web.Core.Entities;
using HRM.Web.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Web.Core.Services
{
    public class UserInfoService: BaseService<UserInfo>, IUserInfoService
    {
        #region Field
        private readonly IUserInfoRepository _userInfoRepository;
        #endregion
        #region Constructor
        public UserInfoService(IUserInfoRepository userInfoRepository):base(userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }
        #endregion
    }
}
