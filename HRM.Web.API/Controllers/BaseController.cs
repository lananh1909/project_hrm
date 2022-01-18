using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRM.Web.Core;
using HRM.Web.Core.Entities;
using HRM.Web.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Web.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class BaseController<TEntity> : ControllerBase where TEntity: BaseEntity
    {
        #region Field
        private readonly IBaseService<TEntity> _baseService;
        #endregion
        #region Contructor
        public BaseController(IBaseService<TEntity> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        [HttpGet("[controller]s")]
        public IActionResult GetAll()
        {
            try
            {
                var res = _baseService.GetEntities();
                if (((IEnumerable<TEntity>)res.Data).Any())
                {
                    return Ok(res);
                } else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }
        /// <summary>
        /// Lấy bản ghi theo khóa chính
        /// </summary>
        /// <param name="id">khóa chính</param>
        /// <returns>Bản ghi</returns>
        /// Created by: NLANH 17/8/2021
        [HttpGet("[controller]s/{id}")]
        public IActionResult GetById(string id)
        {
            try
            {
                var guidId = Guid.Empty;
                if (!Guid.TryParse(id, out guidId))
                {
                    return BadRequest(new ErrorServiceResult()
                    {
                        DevMessage = ErrorConstants.InvalidGuidMessage,
                        UserMessage = ErrorConstants.InvalidGuidMessage,
                        ErrorCode = ErrorConstants.InvalidGuid,
                        TraceId = Guid.NewGuid().ToString()
                    });
                }
                var res = _baseService.GetEntityById(guidId);
                if (res.Data != null)
                {
                    return Ok(res);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }
        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>
        ///  201 - Thêm mới thành công
        ///  400 - Dữ liệu vào chưa đúng định dạng
        ///  500 - Lỗi server
        /// </returns>
        /// Created by: NLANH 17/8/2021
        [HttpPost("[controller]")]
        public IActionResult Create([FromBody] TEntity entity)
        {
            try
            {
                var tableName = typeof(TEntity).Name;
                var property = typeof(TEntity).GetProperty("Id");
                var propertyValue = property.GetValue(entity);
                if (propertyValue != null && !propertyValue.Equals(Guid.Empty))
                {
                    property.SetValue(entity, Guid.Empty);
                }
                var res = _baseService.Add(entity);
                if (res.Success)
                {
                    return StatusCode(201, res);

                }
                else
                {
                    return BadRequest(new ErrorServiceResult()
                    {
                        DevMessage = res.Data.ToString(),
                        UserMessage = ErrorConstants.NullRequestBodyMessage,
                        ErrorCode = ErrorConstants.NullRequestBody,
                        TraceId = Guid.NewGuid().ToString(),
                        MoreInfo = ""
                    });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }

        /// <summary>
        /// Chỉnh sửa thông tin bản ghi
        /// </summary>
        /// <param name="id">Khóa chính</param>
        /// <param name="entity">Thông tin bản ghi</param>
        /// <returns>
        /// 200 - Sửa thành công
        /// 400 - Dữ liệu vào chưa đúng định dạng
        /// 500 - Lỗi Server
        /// </returns>
        /// Created by: NLANH 17/8/2021
        [HttpPut("[controller]s/{id}")]
        public IActionResult Update(Guid id, TEntity entity)
        {
            try
            {
                var res = _baseService.Update(entity);
                if (res.Success)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(new ErrorServiceResult()
                    {
                        DevMessage = res.Data.ToString(),
                        UserMessage = ErrorConstants.NullRequestBodyMessage,
                        ErrorCode = ErrorConstants.NullRequestBody,
                        TraceId = Guid.NewGuid().ToString(),
                        MoreInfo = ""
                    });
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }
        /// <summary>
        /// Xóa bản ghi theo khóa chính
        /// </summary>
        /// <param name="id">khóa chính</param>
        /// <returns>
        /// 200 - Xóa thành công
        /// 500 - Lỗi Server
        /// </returns>
        /// Created by: NLANH 17/8/2021
        [HttpDelete("[controller]/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var entity = _baseService.GetEntityById(id);
                var tEntity = (TEntity)entity.Data;
                var res = _baseService.Delete(tEntity);
                if (res.Success)
                {
                    return Ok(res);
                }
                else
                {
                    return NotFound(res);
                }
                
            }
            catch (Exception e)
            {
                return StatusCode(500, new ErrorServiceResult()
                {
                    DevMessage = e.Message,
                    UserMessage = ErrorConstants.ServerErrorMessage,
                    ErrorCode = ErrorConstants.ServerError,
                    TraceId = Guid.NewGuid().ToString(),
                    MoreInfo = ""
                });
            }
        }
        #endregion
    }
}
