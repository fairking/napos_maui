using Napos.Core.Abstract;
using Napos.Core.Helpers;
using Napos.Data.Entities;
using QueryMan;
using SqlKata.Compilers;
using System;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Napos.Data
{
    public class DataContext : QueryRunner
    {
        private static PropertyInfo _createdProperty = typeof(BaseAuditedEntity).GetProperty(nameof(BaseAuditedEntity.Created));

        private static PropertyInfo _updatedProperty = typeof(BaseAuditedEntity).GetProperty(nameof(BaseAuditedEntity.Updated));

        private readonly IDateTimeService _dtService;

        public DataContext(IDbConnection connection, Compiler compiler, IDateTimeService dtService, bool snakeCase = false, int timeout = 30, IsolationLevel defaultIsoLevel = IsolationLevel.ReadCommitted) 
            : base(connection, compiler, snakeCase, timeout, defaultIsoLevel)
        {
            _dtService = dtService;
        }

        protected override void OnSaveOrUpdate<T>(T obj, bool insert)
        {
            ApplyAudit(obj, insert);
            base.OnSaveOrUpdate(obj, insert);
        }

        protected override Task OnSaveOrUpdateAsync<T>(T obj, bool insert, CancellationToken cancellationToken = default)
        {
            ApplyAudit(obj, insert);
            return base.OnSaveOrUpdateAsync(obj, insert, cancellationToken);
        }

        private void ApplyAudit(object auditedEntity, bool insert)
        {
            if (!auditedEntity.Is<BaseAuditedEntity>())
                return;

            var now = _dtService.NowUtc();

            if (insert)
            {
                _createdProperty.SetValue(auditedEntity, now);
            }

            _updatedProperty.SetValue(auditedEntity, now);
        }

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            try
            {
                BeginTransaction();

                var result = await func();

                Commit();

                return result;
            }
            catch
            {
                Rollback();
                throw;
            }
        }

        public async Task Execute(Func<Task> func)
        {
            try
            {
                BeginTransaction();

                await func();

                Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
        }
    }
}
