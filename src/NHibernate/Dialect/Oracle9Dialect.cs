using System;
using System.Collections;
using System.Data;
using NHibernate.Dialect.Function;
using NHibernate.Dialect.Schema;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using Environment=NHibernate.Cfg.Environment;
using System.Data.Common;

namespace NHibernate.Dialect
{
	/// <summary>
	/// It's a immature version, it just work.
	/// An SQL dialect for Oracle 9
	/// </summary>
	/// <remarks>
	/// The Oracle9Dialect defaults the following configuration properties:
	/// <list type="table">
	///		<listheader>
	///			<term>Property</term>
	///			<description>Default Value</description>
	///		</listheader>
	///		<item>
	///			<term>use_outer_join</term>
	///			<description><see langword="true" /></description>
	///		</item>
	///		<item>
	///			<term>connection.driver_class</term>
	///			<description><see cref="NHibernate.Driver.OracleClientDriver" /></description>
	///		</item>
	/// </list>
	/// </remarks>
	[Obsolete("This dialect will be removed in the next NHibernate version; Use Oracle9iDialect")]
	public class Oracle9Dialect : Dialect
	{
		/// <summary></summary>
		public Oracle9Dialect()
		{
			DefaultProperties[Environment.PrepareSql] = "false";
			DefaultProperties[Environment.ConnectionDriver] = "NHibernate.Driver.OracleClientDriver";

			RegisterColumnType(DbType.AnsiStringFixedLength, "CHAR(255)");
			RegisterColumnType(DbType.AnsiStringFixedLength, 2000, "CHAR($l)");
			RegisterColumnType(DbType.AnsiString, "VARCHAR2(255)");
			RegisterColumnType(DbType.AnsiString, 2000, "VARCHAR2($l)");
			RegisterColumnType(DbType.AnsiString, 2147483647, "CLOB"); // should use the IType.ClobType
			RegisterColumnType(DbType.Binary, "RAW(2000)");
			RegisterColumnType(DbType.Binary, 2000, "RAW($l)");
			RegisterColumnType(DbType.Binary, 2147483647, "BLOB");
			RegisterColumnType(DbType.Boolean, "NUMBER(1,0)");
			RegisterColumnType(DbType.Byte, "NUMBER(3,0)");
			RegisterColumnType(DbType.Currency, "NUMBER(19,1)");
			RegisterColumnType(DbType.Date, "DATE");
			RegisterColumnType(DbType.DateTime, "TIMESTAMP(4)");
			RegisterColumnType(DbType.Decimal, "NUMBER(19,5)");
			RegisterColumnType(DbType.Decimal, 19, "NUMBER(19, $l)");
			// having problems with both ODP and OracleClient from MS not being able
			// to read values out of a field that is DOUBLE PRECISION
			RegisterColumnType(DbType.Double, "DOUBLE PRECISION"); //"FLOAT(53)" );
			//RegisterColumnType(DbType.Guid, "CHAR(38)");
			RegisterColumnType(DbType.Int16, "NUMBER(5,0)");
			RegisterColumnType(DbType.Int32, "NUMBER(10,0)");
			RegisterColumnType(DbType.Int64, "NUMBER(20,0)");
			RegisterColumnType(DbType.UInt16, "NUMBER(5,0)");
			RegisterColumnType(DbType.UInt32, "NUMBER(10,0)");
			RegisterColumnType(DbType.UInt64, "NUMBER(20,0)");
			RegisterColumnType(DbType.Single, "FLOAT(24)");
			RegisterColumnType(DbType.StringFixedLength, "NCHAR(255)");
			RegisterColumnType(DbType.StringFixedLength, 2000, "NCHAR($l)");
			RegisterColumnType(DbType.String, "NVARCHAR2(255)");
			RegisterColumnType(DbType.String, 2000, "NVARCHAR2($l)");
			RegisterColumnType(DbType.String, 1073741823, "NCLOB");
			RegisterColumnType(DbType.Time, "DATE");

			RegisterFunction("abs", new StandardSQLFunction("abs"));
			RegisterFunction("sign", new StandardSQLFunction("sign", NHibernateUtil.Int32));

			RegisterFunction("acos", new StandardSQLFunction("acos", NHibernateUtil.Double));
			RegisterFunction("asin", new StandardSQLFunction("asin", NHibernateUtil.Double));
			RegisterFunction("atan", new StandardSQLFunction("atan", NHibernateUtil.Double));
			RegisterFunction("cos", new StandardSQLFunction("cos", NHibernateUtil.Double));
			RegisterFunction("cosh", new StandardSQLFunction("cosh", NHibernateUtil.Double));
			RegisterFunction("exp", new StandardSQLFunction("exp", NHibernateUtil.Double));
			RegisterFunction("ln", new StandardSQLFunction("ln", NHibernateUtil.Double));
			RegisterFunction("sin", new StandardSQLFunction("sin", NHibernateUtil.Double));
			RegisterFunction("sinh", new StandardSQLFunction("sinh", NHibernateUtil.Double));
			RegisterFunction("stddev", new StandardSQLFunction("stddev", NHibernateUtil.Double));
			RegisterFunction("sqrt", new StandardSQLFunction("sqrt", NHibernateUtil.Double));
			RegisterFunction("tan", new StandardSQLFunction("tan", NHibernateUtil.Double));
			RegisterFunction("tanh", new StandardSQLFunction("tanh", NHibernateUtil.Double));
			RegisterFunction("variance", new StandardSQLFunction("variance", NHibernateUtil.Double));

			RegisterFunction("round", new StandardSQLFunction("round"));
			RegisterFunction("trunc", new StandardSQLFunction("trunc"));
			RegisterFunction("ceil", new StandardSQLFunction("ceil"));
			RegisterFunction("floor", new StandardSQLFunction("floor"));

			RegisterFunction("chr", new StandardSQLFunction("chr", NHibernateUtil.Character));
			RegisterFunction("initcap", new StandardSQLFunction("initcap"));
			RegisterFunction("lower", new StandardSQLFunction("lower"));
			RegisterFunction("ltrim", new StandardSQLFunction("ltrim"));
			RegisterFunction("rtrim", new StandardSQLFunction("rtrim"));
			RegisterFunction("soundex", new StandardSQLFunction("soundex"));
			RegisterFunction("upper", new StandardSQLFunction("upper"));
			RegisterFunction("ascii", new StandardSQLFunction("ascii", NHibernateUtil.Int32));
			RegisterFunction("length", new StandardSQLFunction("length", NHibernateUtil.Int64));
			RegisterFunction("left", new SQLFunctionTemplate(NHibernateUtil.String, "substr(?1, 1, ?2)"));
			RegisterFunction("right", new SQLFunctionTemplate(NHibernateUtil.String, "substr(?1, -?2)"));

			RegisterFunction("to_char", new StandardSQLFunction("to_char", NHibernateUtil.String));
			RegisterFunction("to_date", new StandardSQLFunction("to_date", NHibernateUtil.Timestamp));

			RegisterFunction("lastday", new StandardSQLFunction("lastday", NHibernateUtil.Date));
			RegisterFunction("sysdate", new NoArgSQLFunction("sysdate", NHibernateUtil.Date, false));
			RegisterFunction("uid", new NoArgSQLFunction("uid", NHibernateUtil.Int32, false));
			RegisterFunction("user", new NoArgSQLFunction("user", NHibernateUtil.String, false));
			RegisterFunction("current_timestamp", new CurrentTimeStamp());

			RegisterFunction("str", new SQLFunctionTemplate(NHibernateUtil.String, "cast(?1 as nvarchar2(2000))"));

			// Multi-param string dialect functions...
			RegisterFunction("concat", new VarArgsSQLFunction(NHibernateUtil.String, "(", "||", ")"));
			RegisterFunction("instr", new StandardSQLFunction("instr", NHibernateUtil.String));
			RegisterFunction("instrb", new StandardSQLFunction("instrb", NHibernateUtil.String));
			RegisterFunction("lpad", new StandardSQLFunction("lpad", NHibernateUtil.String));
			RegisterFunction("replace", new StandardSQLFunction("replace", NHibernateUtil.String));
			RegisterFunction("rpad", new StandardSQLFunction("rpad", NHibernateUtil.String));
			RegisterFunction("substr", new StandardSQLFunction("substr", NHibernateUtil.String));
			RegisterFunction("substring", new StandardSQLFunction("substr", NHibernateUtil.String)); 
			RegisterFunction("substrb", new StandardSQLFunction("substrb", NHibernateUtil.String));
			RegisterFunction("translate", new StandardSQLFunction("translate", NHibernateUtil.String));

			// Multi-param numeric dialect functions...
			RegisterFunction("atan2", new StandardSQLFunction("atan2", NHibernateUtil.Single));
			RegisterFunction("log", new StandardSQLFunction("log", NHibernateUtil.Int32));
			RegisterFunction("mod", new StandardSQLFunction("mod", NHibernateUtil.Int32));
			RegisterFunction("nvl", new StandardSQLFunction("nvl"));
			RegisterFunction("power", new StandardSQLFunction("power", NHibernateUtil.Single));

			// Multi-param date dialect functions...
			RegisterFunction("add_months", new StandardSQLFunction("add_months", NHibernateUtil.Date));
			RegisterFunction("months_between", new StandardSQLFunction("months_between", NHibernateUtil.Single));
			RegisterFunction("next_day", new StandardSQLFunction("next_day", NHibernateUtil.Date));
		}

		/// <summary></summary>
		public override string AddColumnString
		{
			get { return "add"; }
		}

		/// <summary></summary>
		public override string GetSequenceNextValString(string sequenceName)
		{
			return "select " + sequenceName + ".nextval from dual";
		}

		/// <summary></summary>
		public override string GetCreateSequenceString(string sequenceName)
		{
			return
				"create sequence " + sequenceName +
				" INCREMENT BY 1 START WITH 1 MAXVALUE 1.0E28 MINVALUE 1 NOCYCLE CACHE 20 NOORDER";
		}

		/// <summary></summary>
		public override string GetDropSequenceString(string sequenceName)
		{
			return "drop sequence " + sequenceName;
		}

		/// <summary></summary>
		public override string CascadeConstraintsString
		{
			get { return " cascade constraints"; }
		}

		public override string ForUpdateNowaitString
		{
			get { return " for update nowait"; }
		}

		/// <summary></summary>
		public override bool SupportsSequences
		{
			get { return true; }
		}

		/// <summary></summary>
		public override bool SupportsLimit
		{
			get { return true; }
		}

		public override bool SupportsVariableLimit
		{
			get { return false; }
		}


		public override SqlString GetLimitString(SqlString querySqlString, int offset, int limit)
		{
			SqlStringBuilder pagingBuilder = new SqlStringBuilder();
			var hasOffset = offset > 0;

			pagingBuilder.Add("SELECT * FROM (");
			pagingBuilder.Add(querySqlString);
			if (hasOffset)
			{
				pagingBuilder.Add(") WHERE rownum BETWEEN ");
				pagingBuilder.Add(offset.ToString());
				pagingBuilder.Add(" AND ");
				pagingBuilder.Add((limit + offset).ToString());
			}
			else
			{
				pagingBuilder.Add(") WHERE rownum <= ");
				pagingBuilder.Add(limit.ToString());
			}

			return pagingBuilder.ToSqlString();
		}

		/// <summary></summary>
		public override bool BindLimitParametersInReverseOrder
		{
			get { return true; }
		}

		/// <summary></summary>
		public override bool UseMaxForLimit
		{
			get { return true; }
		}

		public override bool ForUpdateOfColumns
		{
			get { return true; }
		}

		public override string GetForUpdateString(string aliases)
		{
			return ForUpdateString + " of " + aliases;
		}

		public override string GetForUpdateNowaitString(string aliases)
		{
			return ForUpdateString + " of " + aliases + " nowait";
		}

		public override IDataBaseSchema GetDataBaseSchema(DbConnection connection)
		{
			return new OracleDataBaseSchema(connection);
		}

		[Serializable]
		private class CurrentTimeStamp : NoArgSQLFunction
		{
			public CurrentTimeStamp()
				: base("current_timestamp", NHibernateUtil.DateTime, true)
			{ }

			public override SqlString Render(IList args, ISessionFactoryImplementor factory)
			{
				return new SqlString(Name);
			}
		}
	}
}
