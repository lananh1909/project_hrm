using System;

namespace HRM.Web.Core.Interfaces
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Lấy về tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        ServiceResult GetEntities();
        /// <summary>
        /// Lấy về bản ghi theo khóa chính
        /// </summary>
        /// <param name="entityId">Giá trị khóa chính</param>
        /// <returns>Thông tin bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        ServiceResult GetEntityById(Guid entityId);
        /// <summary>
        /// THêm mới bản ghi
        /// </summary>
        /// <param name="entity">thông tin bản ghi</param>
        /// <returns>Số bản ghi đã thêm mới</returns>
        /// Created by: NLANH 17/8/2021
        ServiceResult Add(TEntity entity);
        /// <summary>
        /// Chỉnh sửa bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi chỉnh sửa</param>
        /// <returns>Số bản ghi được cập nhập</returns>
        /// Created by: NLANH 17/8/2021
        ServiceResult Update(TEntity entity);
        /// <summary>
        /// Xóa bản ghi theo khóa chính
        /// </summary>
        /// <param name="entityId">khóa chính</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Created by: NLANH 17/8/2021
        ServiceResult Delete(TEntity entity);
        /// <summary>
        /// Validate các thuộc tính của đối tượng
        /// </summary>
        /// <param name="entity">Thông tin đối tượng</param>
        /// <returns>Kết quả validate</returns>
        /// Created by: NLANH 17/8/2021
        //ServiceResult Validate(TEntity entity);
        ///// <summary>
        ///// Giúp các đối tượng kế thừa bổ sung các validate
        ///// </summary>
        ///// <param name="entity">Thông tin đối tượng</param>
        ///// <returns>Mặc định trả về kết quả đúng</returns>
        ///// Created by: NLANH 17/8/2021
        //ServiceResult BaseValidate(TEntity entity);
    }
}
