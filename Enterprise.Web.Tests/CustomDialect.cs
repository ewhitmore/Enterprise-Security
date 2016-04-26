using System.Data;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Dialect.Function;

namespace Enterprise.Persistence.Tests
{
    /// <summary>
    /// Extend SQLiteDialect to convert DATETIME2 to TEXT which is supported by SQLITE
    /// http://ewhitmor.blogspot.com/2016/04/sqlite-nhibernate-datetime2.html
    /// </summary>
    public class CustomDialect : SQLiteDialect
    {
        protected override void RegisterColumnTypes()
        {
            base.RegisterColumnTypes();
            RegisterColumnType(DbType.DateTime2, "DATETIME2");
        }

        protected override void RegisterFunctions()
        {
            base.RegisterFunctions();
            RegisterFunction("current_timestamp", new NoArgSQLFunction("TEXT", NHibernateUtil.DateTime2, true));

        }

        protected override void RegisterKeywords()
        {
            base.RegisterKeywords();
            RegisterKeyword("datetime2");

        }
    }
}
