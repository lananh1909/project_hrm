using HRM.Web.Core.Entities;
using HRM.Web.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRM.Web.Core.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : BaseEntity
    {
        #region Field
        private readonly IBaseRepository<TEntity> _baseRepository;
        #endregion
        #region Contructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
        }
        #endregion
        public ServiceResult Add(TEntity entity)
        {
            try
            {
                _baseRepository.Add(entity);
                return new ServiceResult()
                {
                    Success = true,
                    Data = { }
                };
            } catch (ArgumentNullException ex)
            {
                return new ServiceResult()
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        public ServiceResult Delete(TEntity entity)
        {
            try
            {
                _baseRepository.Delete(entity);
                return new ServiceResult()
                {
                    Success = true,
                    Data = { }
                };
            }
            catch (ArgumentNullException ex)
            {
                return new ServiceResult()
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }

        public ServiceResult GetEntities()
        {
            var result = _baseRepository.GetEntities();
            return new ServiceResult()
            {
                Success = true,
                Data = result
            };
        }

        public ServiceResult GetEntityById(Guid entityId)
        {
            var entity = _baseRepository.GetEntityById(entityId);
            return new ServiceResult()
            {
                Success = true,
                Data = entity
            };
        }

        public ServiceResult Update(TEntity entity)
        {
            try
            {
                _baseRepository.Update(entity);
                return new ServiceResult()
                {
                    Success = true,
                    Data = { }
                };
            }
            catch (ArgumentNullException ex)
            {
                return new ServiceResult()
                {
                    Success = false,
                    Data = ex.Message
                };
            }
        }
    }
}
