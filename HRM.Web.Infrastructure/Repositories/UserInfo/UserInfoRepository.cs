using HRM.Web.Core.Entities;
using HRM.Web.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Infrastructure
{
    public class UserInfoRepository: BaseRepository<UserInfo>, IUserInfoRepository
    {
        #region Fields
        private readonly HRMDbContext _dbContext;
        #endregion
        #region Contructor
        public UserInfoRepository (HRMDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion
    }
}
