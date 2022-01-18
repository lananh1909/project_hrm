using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using HRM.Web.Core;
using HRM.Web.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
namespace HRM.Web.Infrastructure
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: BaseEntity
    {
        #region Fields
        private readonly HRMDbContext _dbContext;
        private DbSet<TEntity> entities;
        #endregion
        #region Contructor
        public BaseRepository(HRMDbContext dbContext)
        {
            _dbContext = dbContext;
            entities = dbContext.Set<TEntity>();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Thêm mới phần tử
        /// </summary>
        /// <param name="entity">Thông tin phần tử</param>
        /// <returns>Số phần tử được thêm mới</returns>
        /// Created by: NLANH 17/8/2021
        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entities.AddAsync(entity);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Xóa phần tử theo khóa chính
        /// </summary>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Số phần tử bị xóa</returns>
        /// Created by: NLANH 17/8/2021
        public void Delete(TEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _dbContext.SaveChanges();
        }
        /// <summary>
        /// Lấy về tất cả phần tử
        /// </summary>
        /// <returns>Mảng tất cả các phần tử</returns>
        /// Created by: NLANH 17/8/2021
        public IEnumerable<TEntity> GetEntities()
        {
            return entities.AsEnumerable();
        }
        /// <summary>
        /// Lấy về phần tử theo khóa chính
        /// </summary>
        /// <param name="entityId">Khóa chính</param>
        /// <returns>Phần tử tìm thấy</returns>
        /// Created by: NLANH 17/8/2021
        public TEntity GetEntityById(Guid entityId)
        {
            return entities.SingleOrDefault(s => s.Id == entityId);
        }
        /// <summary>
        /// Cập nhập thông tin phần tử
        /// </summary>
        /// <param name="entity">Thông tin phần tử cập nhập</param>
        /// <returns>Số phần tử được cập nhập</returns>
        /// Created by: NLANH 17/8/2021
        public void Update(TEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _dbContext.SaveChanges();
        }
        #endregion

    }
}
