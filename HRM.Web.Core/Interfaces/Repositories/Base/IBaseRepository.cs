using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Web.Core
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy về tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        IEnumerable<TEntity> GetEntities();
        /// <summary>
        /// Lất về bản ghi theo khóa chính
        /// </summary>
        /// <param name="entityId">khóa chính</param>
        /// <returns>Thông tin bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        TEntity GetEntityById(Guid entityId);
        /// <summary>
        /// THêm mới bản ghi
        /// </summary>
        /// <param name="entity">thông tin bản ghi</param>
        /// <returns>Số bản ghi đã thêm mới</returns>
        /// Created by: NLANH 17/8/2021
        void Add(TEntity entity);
        /// <summary>
        /// Chỉnh sửa bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi chỉnh sửa</param>
        /// <returns>Số bản ghi được cập nhập</returns>
        /// Created by: NLANH 17/8/2021
        void Update(TEntity entity);
        /// <summary>
        /// Xóa bản ghi theo khóa chính
        /// </summary>
        /// <param name="entityId">khóa chính</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Created by: NLANH 17/8/2021
        void Delete(TEntity entity);
    }
}
