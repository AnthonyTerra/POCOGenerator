using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Db.DbObject;
using Db.Extensions;

namespace Db.Helpers
{
    public static class DbHelper
    {
        #region Connection String

        public static string ConnectionString { get; set; }

        #endregion

        #region Server

        public static string GetServerVersion()
        {
            try
            {
                string connectionString = ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"select serverproperty('ProductVersion')";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        return command.ExecuteScalar() as string;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static void BuildServerSchema(Server server, string initialCatalog, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            server.Databases = GetDatabases(initialCatalog);

            foreach (var database in server.Databases.OrderBy<Database, string>(d => d.ToString()))
            {
                database.Server = server;
                BuildDatabaseSchema(database, buildingDbObject, builtDbObject);
            }
        }

        #endregion

        #region Databases

        public static List<Database> GetDatabases(string initialCatalog = null)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    List<Database> databases =
                        connection.GetSchema("Databases").ToList<Database>().
                        Where(d => d.database_name != "master" && d.database_name != "model" && d.database_name != "msdb" && d.database_name != "tempdb").
                        Where(d => string.IsNullOrEmpty(initialCatalog) || string.Compare(d.database_name, initialCatalog, true) == 0).ToList<Database>();
                    return databases;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get databases.", ex);
            }
        }

        private static void BuildDatabaseSchema(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(database);

            database.Errors = new List<Exception>();

            try
            {
                database.SystemObjects = GetSystemObjects(database);
                if (database.SystemObjects != null)
                {
                    database.SystemObjects.ForEach(so =>
                    {
                        so.Database = database;
                        so.type = so.type.Trim();
                    });
                }
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                database.ExtendedProperties = GetExtendedProperties(database);
                if (database.ExtendedProperties != null)
                    database.ExtendedProperties.ForEach(ep => ep.Database = database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                var keys = GetKeys(database);

                database.PrimaryKeys = keys.Item1;
                if (database.PrimaryKeys != null)
                    database.PrimaryKeys.ForEach(pk => pk.Database = database);

                database.UniqueKeys = keys.Item2;
                if (database.UniqueKeys != null)
                    database.UniqueKeys.ForEach(uk => uk.Database = database);

                database.ForeignKeys = keys.Item3;
                if (database.ForeignKeys != null)
                    database.ForeignKeys.ForEach(fk => fk.Database = database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                database.IndexColumns = GetIndexColumns(database);
                if (database.IndexColumns != null)
                    database.IndexColumns.ForEach(ic => ic.Database = database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                database.IdentityColumns = GetIdentityColumns(database);
                if (database.IdentityColumns != null)
                    database.IdentityColumns.ForEach(ic => ic.Database = database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                database.ComputedColumns = GetComputedColumns(database);
                if (database.ComputedColumns != null)
                    database.ComputedColumns.ForEach(cc => cc.Database = database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildSchemaTVPs(database, buildingDbObject, builtDbObject);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildSchemaTables(database, buildingDbObject, builtDbObject);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildSchemaViews(database, buildingDbObject, builtDbObject);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildSchemaProcedures(database, buildingDbObject, builtDbObject);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildSchemaFunctions(database, buildingDbObject, builtDbObject);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            try
            {
                BuildNavigationProperties(database);
            }
            catch (Exception ex)
            {
                database.Errors.Add(ex);
            }

            if (builtDbObject != null)
                builtDbObject(database);
        }

        #endregion

        #region System Objects

        private static List<SystemObject> GetSystemObjects(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
                                al.type,
                                object_schema = ss.name,
                                object_name = al.name
                            from sys.all_objects al
                            inner join sys.schemas ss on al.schema_id = ss.schema_id
                            where al.is_ms_shipped = 1
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable systemObjectsDT = new DataTable();
                            systemObjectsDT.Load(reader);
                            return systemObjectsDT.ToList<SystemObject>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get system objects.", ex);
            }
        }

        #endregion

        #region Extended Properties

        private static List<ExtendedProperty> GetExtendedProperties(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            -- database
                            select Class = ep.class, Class_Desc = ep.class_desc, Schema_Id = 0, Table_Id = 0, Id = db_id(), Schema_Name = '', Table_Name = '', Name = db_name(), Description = ep.value
                            from sys.extended_properties ep
                            where ep.class = 0
                            and ep.major_id = 0
                            and ep.minor_id = 0
                            and ep.name = N'MS_Description'

                            union all

                            -- schema
                            select Class = ep.class, Class_Desc = ep.class_desc, Schema_Id = s.schema_id, Table_Id = 0, Id = 0, Schema_Name = s.name, Table_Name = '', Name = '', Description = ep.value
                            from sys.extended_properties ep
                            inner join sys.schemas s on ep.major_id = s.schema_id
                            where ep.class = 3
                            and ep.minor_id = 0
                            and ep.name = N'MS_Description'

                            union all

                            -- table
                            select Class = ep.class, Class_Desc = ep.class_desc, Schema_Id = s.schema_id, Table_Id = t.object_id, Id = 0, Schema_Name = s.name, Table_Name = t.name, Name = '', Description = ep.value
                            from sys.extended_properties ep
                            inner join sys.tables t on ep.major_id = t.object_id
                            inner join sys.schemas s on t.schema_id = s.schema_id
                            where ep.class = 1
                            and ep.minor_id = 0
                            and ep.name = N'MS_Description'

                            union all

                            -- table column
                            select Class = ep.class, Class_Desc = ep.class_desc, Schema_Id = s.schema_id, Table_Id = t.object_id, Id = c.column_id, Schema_Name = s.name, Table_Name = t.name, Name = c.name, Description = ep.value
                            from sys.extended_properties ep
                            inner join sys.tables t on ep.major_id = t.object_id
                            inner join sys.schemas s on t.schema_id = s.schema_id
                            inner join sys.columns c on ep.major_id = c.object_id and ep.minor_id = c.column_id
                            where ep.class = 1
                            and ep.name = N'MS_Description'

                            union all

                            -- index
                            select Class = ep.class, Class_Desc = ep.class_desc, Schema_Id = s.schema_id, Table_Id = t.object_id, Id = i.index_id, Schema_Name = s.name, Table_Name = t.name, Name = i.name, Description = ep.value
                            from sys.extended_properties ep
                            inner join sys.tables t on ep.major_id = t.object_id
                            inner join sys.schemas s on t.schema_id = s.schema_id
                            inner join sys.indexes i on ep.major_id = i.object_id and ep.minor_id = i.index_id
                            where ep.class = 7
                            and ep.name = N'MS_Description'

                            order by Class, Schema_Name, Table_Name, Name
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable computedColumnsDT = new DataTable();
                            computedColumnsDT.Load(reader);
                            return computedColumnsDT.ToList<ExtendedProperty>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get extended properties.", ex);
            }
        }

        #endregion

        #region Primary, Unique & Foreign Keys

        private static Tuple<List<PrimaryKey>, List<UniqueKey>, List<ForeignKey>> GetKeys(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            declare @PrimaryKeys table (
	                            Id int,
                                Name nvarchar(128),
                                Schema_Id int,
                                Schema_Name nvarchar(128),
                                Table_Id int,
                                Table_Name nvarchar(128),
                                Ordinal tinyint,
                                Column_Id int,
                                Column_Name nvarchar(128),
                                Is_Descending bit,
                                Is_Identity bit,
                                Is_Computed bit
                            )

                            insert into @PrimaryKeys
                            select
	                            Id = kc.object_id,
                                Name = kc.name,
                                Schema_Id = ss.schema_id,
                                Schema_Name = ss.name,
                                Table_Id = kc.parent_object_id,
                                Table_Name = object_name(kc.parent_object_id),
                                Ordinal = ic.key_ordinal,
                                Column_Id = c.column_id,
                                Column_Name = c.name,
                                Is_Descending = ic.is_descending_key,
                                Is_Identity = c.is_identity,
                                Is_Computed = c.is_computed
                            from sys.key_constraints kc
                            inner join sys.index_columns ic on kc.parent_object_id = ic.object_id and kc.unique_index_id = ic.index_id and kc.type = 'PK'
                            inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
                            inner join sys.schemas ss on kc.schema_id = ss.schema_id
                            order by Schema_Name, Table_Name, Ordinal

                            declare @UniqueKeys table (
	                            Id int,
                                Name nvarchar(128),
                                Schema_Id int,
                                Schema_Name nvarchar(128),
                                Table_Id int,
                                Table_Name nvarchar(128),
                                Ordinal tinyint,
                                Column_Id int,
                                Column_Name nvarchar(128),
                                Is_Descending bit,
                                Is_Identity bit,
                                Is_Computed bit
                            )

                            insert into @UniqueKeys
                            select
	                            Id = kc.object_id,
                                Name = kc.name,
                                Schema_Id = ss.schema_id,
                                Schema_Name = ss.name,
                                Table_Id = kc.parent_object_id,
                                Table_Name = object_name(kc.parent_object_id),
                                Ordinal = ic.key_ordinal,
                                Column_Id = c.column_id,
                                Column_Name = c.name,
                                Is_Descending = ic.is_descending_key,
                                Is_Identity = c.is_identity,
                                Is_Computed = c.is_computed
                            from sys.key_constraints kc
                            inner join sys.index_columns ic on kc.parent_object_id = ic.object_id and kc.unique_index_id = ic.index_id and kc.type = 'UQ'
                            inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
                            inner join sys.schemas ss on kc.schema_id = ss.schema_id
                            order by Schema_Name, Table_Name, Ordinal

                            declare @ForeignKeys table (
                                Id int,
                                Name nvarchar(128),
                                Is_One_To_One bit,
                                Is_One_To_Many bit,
                                Is_Many_To_Many bit,
                                Is_Many_To_Many_Complete bit,
	                            Is_Cascade_Delete bit,
	                            Is_Cascade_Update bit,
                                Foreign_Schema_Id int,
                                Foreign_Schema nvarchar(128),
                                Foreign_Table_Id int,
                                Foreign_Table nvarchar(128),
                                Foreign_Column_Id int,
                                Foreign_Column nvarchar(128),
                                Is_Foreign_PK bit,
                                Primary_Schema_Id int,
                                Primary_Schema nvarchar(128),
                                Primary_Table_Id int,
                                Primary_Table nvarchar(128),
                                Primary_Column_Id int,
                                Primary_Column nvarchar(128),
                                Is_Primary_PK bit,
                                Ordinal int
                            )

                            insert into @ForeignKeys
                            select
	                            Id = f.object_id,
                                Name = f.name,
                                Is_One_To_One = 0,
                                Is_One_To_Many = 1,
                                Is_Many_To_Many = 0,
                                Is_Many_To_Many_Complete = 0,
	                            Is_Cascade_Delete = (case when f.delete_referential_action = 1 then 1 else 0 end),
	                            Is_Cascade_Update = (case when f.update_referential_action = 1 then 1 else 0 end),
                                Foreign_Schema_Id = ssf.schema_id,
                                Foreign_Schema = ssf.name,
                                Foreign_Table_Id = f.parent_object_id,
                                Foreign_Table = object_name(f.parent_object_id),
                                Foreign_Column_Id = fc.parent_column_id,
                                Foreign_Column = col_name(fc.parent_object_id, fc.parent_column_id),
                                Is_Foreign_PK = (case when pkf.Column_Id is null then 0 else 1 end),
                                Primary_Schema_Id = ssp.schema_id,
                                Primary_Schema = ssp.name,
                                Primary_Table_Id = f.referenced_object_id,
                                Primary_Table = object_name(f.referenced_object_id),
                                Primary_Column_Id = fc.referenced_column_id,
                                Primary_Column = col_name(fc.referenced_object_id, fc.referenced_column_id),
                                Is_Primary_PK = (case when pkp.Column_Id is null then 0 else 1 end),
	                            Ordinal = fc.constraint_column_id
                            from sys.foreign_keys f
                            inner join sys.foreign_key_columns fc on f.object_id = fc.constraint_object_id
                            inner join sys.schemas ssf on f.schema_id = ssf.schema_id
                            inner join sys.tables st on f.referenced_object_id = st.object_id
                            inner join sys.schemas ssp on st.schema_id = ssp.schema_id
                            left outer join @PrimaryKeys pkf on ssf.schema_id = pkf.Schema_Id and f.parent_object_id = pkf.Table_Id and fc.parent_column_id = pkf.Column_Id
                            left outer join @PrimaryKeys pkp on ssp.schema_id = pkp.Schema_Id and f.referenced_object_id = pkp.Table_Id and fc.referenced_column_id = pkp.Column_Id
                            order by Foreign_Schema, Foreign_Table, Ordinal

                            -- one-to-one
                            update fk
                            set Is_One_To_One = 1,
                                Is_One_To_Many = 0,
                                Is_Many_To_Many = 0,
                                Is_Many_To_Many_Complete = 0
                            from @ForeignKeys fk
                            where Is_Foreign_PK = 1 and Is_Primary_PK = 1
                            and Id not in (
	                            -- foreign table with a primary key column that is not included in the foreign key
	                            select Id
	                            from (
		                            -- primary keys of the foreign table
		                            select fk.Id, pk.Column_Id
		                            from @PrimaryKeys pk
		                            inner join (
			                            select distinct Id, Foreign_Schema_Id, Foreign_Table_Id
			                            from @ForeignKeys
			                            where Is_Foreign_PK = 1 and Is_Primary_PK = 1
		                            ) fk on pk.Schema_Id = fk.Foreign_Schema_Id and pk.Table_Id = fk.Foreign_Table_Id

		                            except

		                            -- foreign column that are pk columns and reference pk columns
		                            select Id, Column_Id = Foreign_Column_Id
		                            from @ForeignKeys
		                            where Is_Foreign_PK = 1 and Is_Primary_PK = 1
	                            ) t
                            )

                            declare @ManyToMany table (
	                            Id int
                            )

                            -- candidates for many-to-many
                            insert into @ManyToMany
                            select distinct fk.Id
                            from @ForeignKeys fk
                            inner join (
	                            -- foreign table with more than one reference to another table
	                            select Foreign_Schema_Id, Foreign_Table_Id
	                            from (
		                            -- foreign key with foreign column that are pk columns and reference pk columns
		                            select distinct Foreign_Schema_Id, Foreign_Table_Id, Primary_Schema_Id, Primary_Table_Id
		                            from @ForeignKeys
		                            where Is_Foreign_PK = 1 and Is_Primary_PK = 1
	                            ) t
	                            group by Foreign_Schema_Id, Foreign_Table_Id
	                            having count(*)>1
                            ) j on fk.Foreign_Schema_Id = j.Foreign_Schema_Id and fk.Foreign_Table_Id = j.Foreign_Table_Id

                            declare @ForeignColumns table (
	                            Id int,
	                            Column_Id int
                            )

                            -- primary keys of the many-to-many tables
                            insert into @ForeignColumns
                            select fk.Id, pk.Column_Id
                            from @PrimaryKeys pk
                            inner join (
	                            select distinct fk.Id, fk.Foreign_Schema_Id, fk.Foreign_Table_Id
	                            from @ForeignKeys fk
	                            inner join @ManyToMany mtm on fk.Id = mtm.Id
	                            where fk.Is_Foreign_PK = 1 and fk.Is_Primary_PK = 1
                            ) fk on pk.Schema_Id = fk.Foreign_Schema_Id and pk.Table_Id = fk.Foreign_Table_Id

                            delete from fc
                            from @ForeignColumns fc
                            inner join (
	                            select mtm.Id, c.Foreign_Column_Id
	                            from @ManyToMany mtm cross join (
		                            select fk.Foreign_Column_Id
		                            from @ForeignKeys fk
		                            inner join @ManyToMany mtm on fk.Id = mtm.Id
		                            where fk.Is_Foreign_PK = 1 and fk.Is_Primary_PK = 1
	                            ) c
                            ) c on fc.Id = c.Id and fc.Column_Id = c.Foreign_Column_Id

                            -- not many-to-many
                            -- foreign table with a primary key column that is not included in the foreign key
                            delete from mtm
                            from @ManyToMany mtm
                            inner join @ForeignColumns fc on mtm.Id = fc.Id

                            -- many-to-many
                            update fk
                            set fk.Is_One_To_One = 0,
                                fk.Is_One_To_Many = 0,
                                fk.Is_Many_To_Many = 1,
                                fk.Is_Many_To_Many_Complete = 1
                            from @ForeignKeys fk
                            inner join @ManyToMany mtm on fk.Id = mtm.Id

                            -- many-to-many join table is not complete
                            -- there is at least one more column that is not part of the pk
                            update fk
                            set fk.Is_Many_To_Many_Complete = 0
                            from @ForeignKeys fk
                            inner join (
	                            -- the columns of the many-to-many join table
	                            select
		                            mtm.Foreign_Schema_Id,
		                            mtm.Foreign_Table_Id,
		                            Column_Id = c.column_id
	                            from sys.columns c
	                            inner join sys.sysobjects so on c.object_id = so.id
	                            inner join sys.schemas ss on so.uid = ss.schema_id
	                            inner join (
		                            select distinct Foreign_Schema_Id, Foreign_Table_Id
		                            from @ForeignKeys
		                            where Is_Many_To_Many = 1
	                            ) mtm on mtm.Foreign_Schema_Id = ss.schema_id and mtm.Foreign_Table_Id = c.object_id

	                            except

	                            -- the columns of the many-to-many foreign key
	                            select Foreign_Schema_Id, Foreign_Table_Id, Column_Id = Foreign_Column_Id
	                            from @ForeignKeys fk
	                            where Is_Many_To_Many = 1
                            ) t on fk.Foreign_Schema_Id = t.Foreign_Schema_Id and fk.Foreign_Table_Id = t.Foreign_Table_Id

                            select * from @PrimaryKeys order by Schema_Name, Table_Name, Ordinal
                            select * from @UniqueKeys order by Schema_Name, Table_Name, Ordinal
                            select * from @ForeignKeys order by Foreign_Schema, Foreign_Table, Ordinal
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataSet keysDS = new DataSet();
                            keysDS.Tables.Add("PrimaryKeys");
                            keysDS.Tables.Add("UniqueKeys");
                            keysDS.Tables.Add("ForeignKeys");
                            keysDS.Load(reader, LoadOption.PreserveChanges, keysDS.Tables["PrimaryKeys"], keysDS.Tables["UniqueKeys"], keysDS.Tables["ForeignKeys"]);
                            return new Tuple<List<PrimaryKey>, List<UniqueKey>, List<ForeignKey>>(
                                keysDS.Tables["PrimaryKeys"].ToList<PrimaryKey>(),
                                keysDS.Tables["UniqueKeys"].ToList<UniqueKey>(),
                                keysDS.Tables["ForeignKeys"].ToList<ForeignKey>()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get primary, unique & foreign keys.", ex);
            }
        }

        #endregion

        #region Index Columns

        private static List<IndexColumn> GetIndexColumns(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
                                Name = i.name,
	                            Schema_Name = s.name,
	                            Table_Name = t.name,
	                            Is_Unique = i.is_unique,
                                Is_Clustered = cast((case when i.type = 1 then 1 else 0 end) as bit),
	                            Ordinal = ic.key_ordinal,
	                            Column_Name = c.name,
	                            Is_Descending = ic.is_descending_key
                            from sys.indexes i
                            inner join sys.index_columns ic on  i.object_id = ic.object_id and i.index_id = ic.index_id 
                            inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id 
                            inner join sys.tables t on i.object_id = t.object_id 
                            inner join sys.schemas s on t.schema_id = s.schema_id
                            where i.type in (1,2) -- clustered, nonclustered
                            and i.is_primary_key = 0
                            and i.is_unique_constraint = 0
                            and i.is_disabled = 0
                            and i.is_hypothetical = 0
                            and t.is_ms_shipped = 0 
                            and ic.is_included_column = 0
                            order by Schema_Name, Table_Name, Name, Ordinal
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable indexColumnsDT = new DataTable();
                            indexColumnsDT.Load(reader);
                            return indexColumnsDT.ToList<IndexColumn>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get index columns.", ex);
            }
        }

        #endregion

        #region Identity & Computed Columns

        private static List<IdentityColumn> GetIdentityColumns(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
	                            Schema_Name = ss.name,
	                            Table_Name = object_name(c.object_id),
	                            Column_Name = c.name
                            from sys.columns c
                            inner join sys.sysobjects so on c.object_id = so.id
                            inner join sys.schemas ss on so.uid = ss.schema_id
                            where c.is_identity = 1
                            order by Schema_Name, Table_Name, Column_Name
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable identityColumnsDT = new DataTable();
                            identityColumnsDT.Load(reader);
                            return identityColumnsDT.ToList<IdentityColumn>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get identity columns.", ex);
            }
        }

        private static List<ComputedColumn> GetComputedColumns(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
	                            Schema_Name = ss.name,
	                            Table_Name = object_name(c.object_id),
	                            Column_Name = c.name
                            from sys.columns c
                            inner join sys.sysobjects so on c.object_id = so.id
                            inner join sys.schemas ss on so.uid = ss.schema_id
                            where c.is_computed = 1
                            order by Schema_Name, Table_Name, Column_Name
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable computedColumnsDT = new DataTable();
                            computedColumnsDT.Load(reader);
                            return computedColumnsDT.ToList<ComputedColumn>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get computed columns.", ex);
            }
        }

        #endregion

        #region TVPs

        private static void BuildSchemaTVPs(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            database.TVPs = GetTVPs(database);

            if (database.SystemObjects != null)
                database.TVPs = database.TVPs.Where(tvp => database.SystemObjects.Count(so => so.object_schema == tvp.tvp_schema && so.object_name == tvp.tvp_name && so.type == "TT") == 0).ToList<TVP>();

            foreach (var tvp in database.TVPs.OrderBy<TVP, string>(t => t.ToString()))
            {
                tvp.Database = database;
                GetTVPSchema(tvp, buildingDbObject, builtDbObject);
                if (tvp.TVPColumns != null)
                    tvp.TVPColumns.ForEach(c => c.TVP = tvp);
            }
        }

        private static List<TVP> GetTVPs(Database database)
        {
            try
            {
                string connectionString = database.GetConnectionString(ConnectionString);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
	                            tvp_schema = ss.name, 
	                            tvp_name = stt.name, 
	                            stt.type_table_object_id 
                            from sys.table_types stt 
                            inner join sys.schemas ss on stt.schema_id = ss.schema_id
                        ";
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable tvpsDT = new DataTable();
                            tvpsDT.Load(reader);
                            return tvpsDT.ToList<TVP>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get TVPs.", ex);
            }
        }

        public static void GetTVPSchema(TVP tvp, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(tvp);

            string connectionString = tvp.Database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = @"
                            select 
	                            sc.*, 
	                            data_type = st.name 
                            from sys.columns sc 
                            inner join sys.types st on sc.system_type_id = st.system_type_id and sc.user_type_id = st.user_type_id
                            where sc.object_id = @tvp_id
                        ";
                        command.Parameters.Add("@tvp_id", SqlDbType.Int).Value = tvp.type_table_object_id;
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = 60;

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable tvpColumnsDT = new DataTable();
                            tvpColumnsDT.Load(reader);
                            tvp.TVPColumns = tvpColumnsDT.ToList<TVPColumn>();
                        }
                    }
                }
                catch (Exception ex)
                {
                    tvp.Error = ex;
                }
            }

            if (builtDbObject != null)
                builtDbObject(tvp);
        }

        private static DataTable GetTVPDataTable(TVP tvp)
        {
            if (tvp.TVPDataTable != null)
                return tvp.TVPDataTable;

            DataTable tvpDataTable = new DataTable();

            if (tvp.TVPColumns != null)
            {
                foreach (TVPColumn column in tvp.TVPColumns)
                {
                    switch ((column.data_type ?? string.Empty).ToLower())
                    {
                        case "bigint": tvpDataTable.Columns.Add(column.name, typeof(long)); break;
                        case "binary": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "bit": tvpDataTable.Columns.Add(column.name, typeof(bool)); break;
                        case "char": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "date": tvpDataTable.Columns.Add(column.name, typeof(DateTime)); break;
                        case "datetime": tvpDataTable.Columns.Add(column.name, typeof(DateTime)); break;
                        case "datetime2": tvpDataTable.Columns.Add(column.name, typeof(DateTime)); break;
                        case "datetimeoffset": tvpDataTable.Columns.Add(column.name, typeof(DateTimeOffset)); break;
                        case "decimal": tvpDataTable.Columns.Add(column.name, typeof(decimal)); break;
                        case "filestream": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "float": tvpDataTable.Columns.Add(column.name, typeof(double)); break;
                        case "geography": tvpDataTable.Columns.Add(column.name, typeof(Microsoft.SqlServer.Types.SqlGeography)); break;
                        case "geometry": tvpDataTable.Columns.Add(column.name, typeof(Microsoft.SqlServer.Types.SqlGeometry)); break;
                        case "hierarchyid": tvpDataTable.Columns.Add(column.name, typeof(Microsoft.SqlServer.Types.SqlHierarchyId)); break;
                        case "image": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "int": tvpDataTable.Columns.Add(column.name, typeof(int)); break;
                        case "money": tvpDataTable.Columns.Add(column.name, typeof(decimal)); break;
                        case "nchar": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "ntext": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "numeric": tvpDataTable.Columns.Add(column.name, typeof(decimal)); break;
                        case "nvarchar": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "real": tvpDataTable.Columns.Add(column.name, typeof(Single)); break;
                        case "rowversion": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "smalldatetime": tvpDataTable.Columns.Add(column.name, typeof(DateTime)); break;
                        case "smallint": tvpDataTable.Columns.Add(column.name, typeof(short)); break;
                        case "smallmoney": tvpDataTable.Columns.Add(column.name, typeof(decimal)); break;
                        case "sql_variant": tvpDataTable.Columns.Add(column.name, typeof(object)); break;
                        case "text": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "time": tvpDataTable.Columns.Add(column.name, typeof(TimeSpan)); break;
                        case "timestamp": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "tinyint": tvpDataTable.Columns.Add(column.name, typeof(byte)); break;
                        case "uniqueidentifier": tvpDataTable.Columns.Add(column.name, typeof(Guid)); break;
                        case "varbinary": tvpDataTable.Columns.Add(column.name, typeof(byte[])); break;
                        case "varchar": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        case "xml": tvpDataTable.Columns.Add(column.name, typeof(string)); break;
                        default: tvpDataTable.Columns.Add(column.name, typeof(object)); break;
                    }
                }
            }

            tvp.TVPDataTable = tvpDataTable;

            return tvpDataTable;
        }

        #endregion

        #region Tables

        private static void BuildSchemaTables(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            string connectionString = database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Tables = connection.GetSchema("Tables", new string[] { database.database_name, null, null, "BASE TABLE" }).ToList<Table>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get tables.", ex);
                }
            }

            if (database.SystemObjects != null)
                database.Tables = database.Tables.Where(t => database.SystemObjects.Count(so => so.object_schema == t.table_schema && so.object_name == t.table_name && (so.type == "IT" || so.type == "S" || so.type == "U")) == 0).ToList<Table>();

            foreach (var table in database.Tables.OrderBy<Table, string>(t => t.ToString()))
            {
                table.Database = database;
                GetTableSchema(table, buildingDbObject, builtDbObject);
                if (table.TableColumns != null)
                    table.TableColumns.ForEach(c => c.Table = table);
            }
        }

        public static void GetTableSchema(Table table, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(table);

            string connectionString = table.Database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    table.TableColumns = connection.GetSchema("Columns", new string[] { table.Database.database_name, table.table_schema, table.table_name, null }).ToList<TableColumn>();

                    if (table.Database.ExtendedProperties != null)
                        table.ExtendedProperties = table.Database.ExtendedProperties.Where(ep => ep.Class == 1 && ep.Schema_Name == table.table_schema && ep.Table_Name == table.table_name && ep.Id == 0).ToList<ExtendedProperty>();

                    foreach (TableColumn column in table.TableColumns)
                    {
                        if (table.Database.PrimaryKeys != null)
                            column.PrimaryKey = table.Database.PrimaryKeys.Where(pk => pk.Schema_Name == table.table_schema && pk.Table_Name == table.table_name && pk.Column_Name == column.column_name).FirstOrDefault<PrimaryKey>();

                        if (table.Database.UniqueKeys != null)
                            column.UniqueKeys = table.Database.UniqueKeys.Where(uk => uk.Schema_Name == table.table_schema && uk.Table_Name == table.table_name && uk.Column_Name == column.column_name).ToList<UniqueKey>();

                        if (table.Database.ForeignKeys != null)
                        {
                            column.ForeignKeys = table.Database.ForeignKeys.Where(fk => fk.Foreign_Schema == table.table_schema && fk.Foreign_Table == table.table_name && fk.Foreign_Column == column.column_name).ToList<ForeignKey>();
                            if (column.HasForeignKeys)
                            {
                                foreach (ForeignKey fk in column.ForeignKeys)
                                {
                                    fk.FromTable = table;
                                    fk.ToTable = table.Database.Tables.Where(tbl => fk.Primary_Schema == tbl.table_schema && fk.Primary_Table == tbl.table_name).FirstOrDefault<Table>();
                                }
                            }

                            column.PrimaryForeignKeys = table.Database.ForeignKeys.Where(fk => fk.Primary_Schema == table.table_schema && fk.Primary_Table == table.table_name && fk.Primary_Column == column.column_name).ToList<ForeignKey>();
                        }

                        if (table.Database.IndexColumns != null)
                        {
                            column.IndexColumns = table.Database.IndexColumns.Where(ic => ic.Schema_Name == table.table_schema && ic.Table_Name == table.table_name && ic.Column_Name == column.column_name).ToList<IndexColumn>();
                            if (column.HasIndexColumns)
                            {
                                if (table.Database.ExtendedProperties != null)
                                {
                                    foreach (IndexColumn ic in column.IndexColumns)
                                        ic.ExtendedProperties = table.Database.ExtendedProperties.Where(ep => ep.Class == 7 && ep.Schema_Name == ic.Schema_Name && ep.Table_Name == ic.Table_Name && ep.Name == ic.Name).ToList<ExtendedProperty>();
                                }
                            }
                        }

                        if (table.Database.IdentityColumns != null)
                            column.is_identity = table.Database.IdentityColumns.Exists(c => c.Schema_Name == table.table_schema && c.Table_Name == table.table_name && c.Column_Name == column.column_name);

                        if (table.Database.ComputedColumns != null)
                            column.is_computed = table.Database.ComputedColumns.Exists(c => c.Schema_Name == table.table_schema && c.Table_Name == table.table_name && c.Column_Name == column.column_name);

                        if (table.Database.ExtendedProperties != null)
                            column.ExtendedProperties = table.Database.ExtendedProperties.Where(ep => ep.Class == 1 && ep.Schema_Name == table.table_schema && ep.Table_Name == table.table_name && ep.Name == column.column_name).ToList<ExtendedProperty>();
                    }
                }
                catch (Exception ex)
                {
                    table.Error = ex;
                }
            }

            if (builtDbObject != null)
                builtDbObject(table);
        }

        #endregion

        #region Views

        private static void BuildSchemaViews(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            string connectionString = database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Views = connection.GetSchema("Tables", new string[] { database.database_name, null, null, "VIEW" }).ToList<View>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get views.", ex);
                }
            }

            if (database.SystemObjects != null)
                database.Views = database.Views.Where(v => database.SystemObjects.Count(so => so.object_schema == v.table_schema && so.object_name == v.table_name && so.type == "V") == 0).ToList<View>();

            foreach (var view in database.Views.OrderBy<View, string>(v => v.ToString()))
            {
                view.Database = database;
                GetViewSchema(view, buildingDbObject, builtDbObject);
                if (view.TableColumns != null)
                    view.TableColumns.ForEach(c => c.Table = view);
            }
        }

        public static void GetViewSchema(View view, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(view);

            string connectionString = view.Database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    view.TableColumns = connection.GetSchema("Columns", new string[] { view.Database.database_name, view.table_schema, view.table_name, null }).ToList<TableColumn>();
                }
                catch (Exception ex)
                {
                    view.Error = ex;
                }
            }

            if (builtDbObject != null)
                builtDbObject(view);
        }

        #endregion

        #region Procedures

        private static void BuildSchemaProcedures(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            string connectionString = database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Procedures = connection.GetSchema("Procedures", new string[] { database.database_name, null, null, "PROCEDURE" }).ToList<Procedure>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get procedures.", ex);
                }
            }

            if (database.SystemObjects != null)
                database.Procedures = database.Procedures.Where(p => database.SystemObjects.Count(so => so.object_schema == p.routine_schema && so.object_name == p.routine_name && (so.type == "P" || so.type == "PC" || so.type == "RF" || so.type == "X")) == 0).ToList<Procedure>();

            foreach (var procedure in database.Procedures.OrderBy<Procedure, string>(p => p.ToString()))
            {
                procedure.Database = database;
                GetProcedureSchema(procedure, buildingDbObject, builtDbObject);
                if (procedure.ProcedureParameters != null)
                    procedure.ProcedureParameters.ForEach(p => p.Procedure = procedure);
                if (procedure.ProcedureColumns != null)
                    procedure.ProcedureColumns.ForEach(c => c.Procedure = procedure);
            }
        }

        public static void GetProcedureSchema(Procedure procedure, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(procedure);

            string connectionString = procedure.Database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    procedure.ProcedureParameters = connection.GetSchema("ProcedureParameters", new string[] { procedure.Database.database_name, procedure.routine_schema, procedure.routine_name, null }).ToList<ProcedureParameter>();
                    GetProcedureSchema(procedure, connection);
                }
                catch (Exception ex)
                {
                    if (ex.Message.StartsWith("Invalid object name '#"))
                    {
                        try
                        {
                            GetProcedureWithTemporaryTablesSchema(procedure, connection);
                        }
                        catch
                        {
                            procedure.Error = new Exception(ex.Message, new Exception("Temporary tables in stored procedure.\nYou may want to add this code to the stored procedure to retrieve the schema:\nIF 1=0\nBEGIN\n    SET FMTONLY OFF\nEND"));
                        }
                    }
                    else
                    {
                        procedure.Error = ex;
                    }
                }
            }

            if (builtDbObject != null)
                builtDbObject(procedure);
        }

        private static void GetProcedureSchema(Procedure procedure, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = string.Format("[{0}].[{1}]", procedure.routine_schema, procedure.routine_name);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                foreach (ProcedureParameter parameter in procedure.ProcedureParameters.OrderBy<ProcedureParameter, int>(c => c.ordinal_position ?? 0))
                {
                    if (parameter.is_result == "NO")
                    {
                        command.Parameters.Add(GetSqlParameter(parameter, procedure.Database));
                    }
                }

                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    DataTable schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        procedure.ProcedureColumns = schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
                        {
                            if (columnName == "NumericScale")
                                return (int?)(value as short?);
                            return value;
                        }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false).ToList();
                    }
                }
            }
        }

        private static void GetProcedureWithTemporaryTablesSchema(Procedure procedure, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 60;

                string commandText = @"
                    IF 1=0
                    BEGIN
                        SET FMTONLY OFF;
                    END
                    exec [{0}].[{1}] ";
                commandText = string.Format(commandText, procedure.routine_schema, procedure.routine_name);
                foreach (ProcedureParameter parameter in procedure.ProcedureParameters.OrderBy<ProcedureParameter, int>(c => c.ordinal_position ?? 0))
                {
                    if (parameter.is_result == "NO")
                    {
                        commandText += parameter.parameter_name + ",";
                        command.Parameters.Add(GetSqlParameter(parameter, procedure.Database));
                    }
                }
                commandText = commandText.TrimEnd(',');
                command.CommandText = commandText;

                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    DataTable schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        procedure.ProcedureColumns = schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
                        {
                            if (columnName == "NumericScale")
                                return (int?)(value as short?);
                            return value;
                        }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false).ToList();
                    }
                }
            }
        }

        #endregion

        #region Functions

        private static void BuildSchemaFunctions(Database database, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            string connectionString = database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    database.Functions = connection.GetSchema("Procedures", new string[] { database.database_name, null, null, "FUNCTION" }).ToList<Function>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to get functions.", ex);
                }
            }

            if (database.SystemObjects != null)
                database.Functions = database.Functions.Where(f => database.SystemObjects.Count(so => so.object_schema == f.routine_schema && so.object_name == f.routine_name && (so.type == "AF" || so.type == "FN" || so.type == "FS" || so.type == "FT" || so.type == "IF" || so.type == "TF")) == 0).ToList<Function>();

            List<Function> scalarFunctions = new List<Function>();

            foreach (var function in database.Functions.OrderBy<Function, string>(f => f.ToString()))
            {
                function.Database = database;
                bool isScalarFunction = GetFunctionSchema(function, buildingDbObject, builtDbObject);
                if (isScalarFunction)
                {
                    scalarFunctions.Add(function);
                }
                else
                {
                    if (function.ProcedureParameters != null)
                        function.ProcedureParameters.ForEach(p => p.Procedure = function);
                    if (function.ProcedureColumns != null)
                        function.ProcedureColumns.ForEach(c => c.Procedure = function);
                }
            }

            database.Functions = database.Functions.Except(scalarFunctions).ToList<Function>();
        }

        public static bool GetFunctionSchema(Function function, Action<IDbObject> buildingDbObject, Action<IDbObject> builtDbObject)
        {
            if (buildingDbObject != null)
                buildingDbObject(function);

            bool isScalarFunction = true;

            string connectionString = function.Database.GetConnectionString(ConnectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    function.ProcedureParameters = connection.GetSchema("ProcedureParameters", new string[] { function.Database.database_name, function.routine_schema, function.routine_name, null }).ToList<ProcedureParameter>();

                    isScalarFunction = function.ProcedureParameters.Where(param => param.IsResult).Count() == 1;
                    if (isScalarFunction == false)
                        GetFunctionSchema(function, connection);
                }
                catch (Exception ex)
                {
                    function.Error = ex;
                }
            }

            if (isScalarFunction == false && builtDbObject != null)
                builtDbObject(function);

            return isScalarFunction;
        }

        private static void GetFunctionSchema(Function function, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 60;

                string commandText = string.Format("select * from [{0}].[{1}](", function.routine_schema, function.routine_name);
                foreach (ProcedureParameter parameter in function.ProcedureParameters.OrderBy<ProcedureParameter, int>(c => c.ordinal_position ?? 0))
                {
                    if (parameter.is_result == "NO")
                    {
                        commandText += parameter.parameter_name + ",";
                        command.Parameters.Add(GetSqlParameter(parameter, function.Database));
                    }
                }
                commandText = commandText.TrimEnd(',');
                commandText += ")";
                command.CommandText = commandText;

                using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    DataTable schemaTable = reader.GetSchemaTable();
                    if (schemaTable != null)
                    {
                        function.ProcedureColumns = schemaTable.Cast<ProcedureColumn>((string columnName, Type columnType, object value) =>
                        {
                            if (columnName == "NumericScale")
                                return (int?)(value as short?);
                            return value;
                        }).Where(c => string.IsNullOrEmpty(c.ColumnName) == false).ToList();
                    }
                }
            }
        }

        #endregion

        #region Navigation Properties

        private static void BuildNavigationProperties(Database database)
        {
            if (database.ForeignKeys != null && database.ForeignKeys.Count > 0)
            {
                foreach (var fk in database.ForeignKeys)
                {
                    fk.NavigationPropertyRefFrom = GetNavigationPropertyRefFrom(fk);
                    fk.NavigationPropertyRefTo = GetNavigationPropertyRefTo(fk);
                    fk.NavigationPropertiesRefToManyToMany = GetNavigationPropertiesRefToManyToMany(fk);

                    fk.NavigationPropertyRefTo.InverseProperty = fk.NavigationPropertyRefFrom;
                    if (fk.NavigationPropertiesRefToManyToMany != null && fk.NavigationPropertiesRefToManyToMany.Count() > 0)
                    {
                        foreach (var np in fk.NavigationPropertiesRefToManyToMany)
                            np.InverseProperty = fk.NavigationPropertyRefFrom;
                    }
                }
            }
        }

        private static NavigationProperty GetNavigationPropertyRefFrom(ForeignKey fk)
        {
            return new NavigationProperty()
            {
                ForeignKey = fk,
                IsRefFrom = true,
                IsSingle = true,
                PropertyName = GetNavigationPropertyPrimaryPropertyName(fk)
            };
        }

        private static NavigationProperty GetNavigationPropertyRefTo(ForeignKey fk)
        {
            return new NavigationProperty()
            {
                ForeignKey = fk,
                IsRefFrom = false,
                IsSingle = fk.Is_One_To_One,
                PropertyName = GetNavigationPropertyForeignPropertyName(fk, fk.Is_One_To_One == false)
            };
        }

        private static IEnumerable<NavigationProperty> GetNavigationPropertiesRefToManyToMany(ForeignKey fk)
        {
            // the fk is part of a many-to-many relationship
            if (fk.Is_Many_To_Many)
            {
                // complete many-to-many relationship. all the columns in the join table are part of a pk of other tables. (otherwise treat the join table as one-to-many relationship)
                if (fk.Is_Many_To_Many_Complete)
                {
                    // the join table is the foreign table in the fk
                    return fk.Database.ForeignKeys
                        // all the other fks in the many-to-many relationship. they all point to the same join table
                        .Where(joinedFK => joinedFK != fk && joinedFK.Foreign_Schema_Id == fk.Foreign_Schema_Id && joinedFK.Foreign_Table_Id == fk.Foreign_Table_Id)
                        .OrderBy(joinedFK => joinedFK.Ordinal)
                        // before: other tables (joinedFK.Primary) <- join table (fk.Foreign) -> this table (fk.Primary)
                        // after:  other tables (joinedFK.Primary)              ->               this table (fk.Primary)
                        .Select(joinedFK => new ForeignKey()
                        {
                            Id = 0,
                            Name = string.Empty,

                            Foreign_Schema_Id = joinedFK.Primary_Schema_Id,
                            Foreign_Schema = joinedFK.Primary_Schema,
                            Foreign_Table_Id = joinedFK.Primary_Table_Id,
                            Foreign_Table = joinedFK.Primary_Table,
                            Foreign_Column_Id = joinedFK.Primary_Column_Id,
                            Foreign_Column = joinedFK.Primary_Column,
                            FromTable = joinedFK.ToTable,

                            Is_One_To_One = fk.Is_One_To_One,
                            Is_One_To_Many = fk.Is_One_To_Many,
                            Is_Many_To_Many = fk.Is_Many_To_Many,
                            Is_Many_To_Many_Complete = fk.Is_Many_To_Many_Complete,
                            Is_Cascade_Delete = fk.Is_Cascade_Delete,
                            Is_Cascade_Update = fk.Is_Cascade_Update,
                            Is_Foreign_PK = fk.Is_Foreign_PK,
                            Primary_Schema_Id = fk.Primary_Schema_Id,
                            Primary_Schema = fk.Primary_Schema,
                            Primary_Table_Id = fk.Primary_Table_Id,
                            Primary_Table = fk.Primary_Table,
                            Primary_Column_Id = fk.Primary_Column_Id,
                            Primary_Column = fk.Primary_Column,
                            Is_Primary_PK = fk.Is_Primary_PK,
                            Ordinal = fk.Ordinal,
                            Database = fk.Database,
                            ToTable = fk.ToTable
                        })
                        .Select(virtualFK => new NavigationProperty()
                        {
                            ForeignKey = virtualFK,
                            IsRefFrom = false,
                            IsSingle = virtualFK.Is_One_To_One,
                            PropertyName = GetNavigationPropertyForeignPropertyName(virtualFK, virtualFK.Is_One_To_One == false)
                        });
                }
            }

            return null;
        }

        #region Navigation Property Name

        private static string GetNavigationPropertyPrimaryPropertyName(ForeignKey fk, bool isSingular = true)
        {
            string propertyName = fk.Primary_Table;

            // ef convention <navigation property Name>_<primary key property name of navigation property type>
            if (fk.Foreign_Column.EndsWith("_" + fk.Primary_Column))
                propertyName = fk.Foreign_Column.Substring(0, fk.Foreign_Column.LastIndexOf("_" + fk.Primary_Column));

            propertyName = NameHelper.AddNamePrefix(propertyName, fk.Primary_Column);

            if (NameHelper.IsNameVerb(propertyName))
            {
                propertyName = NameHelper.ConjugateNameVerbToPastParticiple(propertyName);
            }
            else
            {
                if (isSingular)
                    propertyName = NameHelper.GetSingularName(propertyName);
                else
                    propertyName = NameHelper.GetPluralName(propertyName);
            }

            propertyName = NameHelper.TransformName(propertyName);
            propertyName = NameHelper.CleanName(propertyName);
            return propertyName;
        }

        private static string GetNavigationPropertyForeignPropertyName(ForeignKey fk, bool isPlural = true)
        {
            string propertyName = fk.Foreign_Table;

            propertyName = NameHelper.AddNamePrefix(propertyName, fk.Foreign_Column);

            if (NameHelper.IsNameVerb(propertyName))
            {
                propertyName = NameHelper.ConjugateNameVerbToPastParticiple(propertyName);
            }
            else
            {
                if (isPlural)
                    propertyName = NameHelper.GetPluralName(propertyName);
                else
                    propertyName = NameHelper.GetSingularName(propertyName);
            }

            propertyName = NameHelper.TransformName(propertyName);
            propertyName = NameHelper.CleanName(propertyName);
            return propertyName;
        }

        #endregion

        #endregion

        #region SqlParameter

        private static SqlParameter GetSqlParameter(ProcedureParameter parameter, Database database)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameter.parameter_name;
            sqlParameter.Value = DBNull.Value;

            string data_type = (parameter.data_type ?? string.Empty).ToLower();

            // http://msdn.microsoft.com/en-us/library/cc716729.aspx
            switch (data_type)
            {
                case "bigint": sqlParameter.SqlDbType = SqlDbType.BigInt; break;
                case "binary": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "bit": sqlParameter.SqlDbType = SqlDbType.Bit; break;
                case "char": sqlParameter.SqlDbType = SqlDbType.Char; break;
                case "date": sqlParameter.SqlDbType = SqlDbType.Date; break;
                case "datetime": sqlParameter.SqlDbType = SqlDbType.DateTime; break;
                case "datetime2": sqlParameter.SqlDbType = SqlDbType.DateTime2; break;
                case "datetimeoffset": sqlParameter.SqlDbType = SqlDbType.DateTimeOffset; break;
                case "decimal": sqlParameter.SqlDbType = SqlDbType.Decimal; break;
                case "filestream": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "float": sqlParameter.SqlDbType = SqlDbType.Float; break;
                case "geography":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "Geography";
                    break;
                case "geometry":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "Geometry";
                    break;
                case "hierarchyid":
                    sqlParameter.SqlDbType = SqlDbType.Udt;
                    sqlParameter.UdtTypeName = "HierarchyId";
                    break;
                case "image": sqlParameter.SqlDbType = SqlDbType.Image; break;
                case "int": sqlParameter.SqlDbType = SqlDbType.Int; break;
                case "money": sqlParameter.SqlDbType = SqlDbType.Money; break;
                case "nchar": sqlParameter.SqlDbType = SqlDbType.NChar; break;
                case "ntext": sqlParameter.SqlDbType = SqlDbType.NText; break;
                case "numeric": sqlParameter.SqlDbType = SqlDbType.Decimal; break;
                case "nvarchar": sqlParameter.SqlDbType = SqlDbType.NVarChar; break;
                case "real": sqlParameter.SqlDbType = SqlDbType.Real; break;
                case "rowversion": sqlParameter.SqlDbType = SqlDbType.Timestamp; break;
                case "smalldatetime": sqlParameter.SqlDbType = SqlDbType.SmallDateTime; break;
                case "smallint": sqlParameter.SqlDbType = SqlDbType.SmallInt; break;
                case "smallmoney": sqlParameter.SqlDbType = SqlDbType.SmallMoney; break;
                case "sql_variant": sqlParameter.SqlDbType = SqlDbType.Variant; break;
                case "text": sqlParameter.SqlDbType = SqlDbType.Text; break;
                case "time": sqlParameter.SqlDbType = SqlDbType.Time; break;
                case "timestamp": sqlParameter.SqlDbType = SqlDbType.Timestamp; break;
                case "tinyint": sqlParameter.SqlDbType = SqlDbType.TinyInt; break;
                case "uniqueidentifier": sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier; break;
                case "varbinary": sqlParameter.SqlDbType = SqlDbType.VarBinary; break;
                case "varchar": sqlParameter.SqlDbType = SqlDbType.VarChar; break;
                case "xml": sqlParameter.SqlDbType = SqlDbType.Xml; break;
                default:
                    if (database.TVPs != null)
                    {
                        // could be more than one tvp with the same name but with different schema
                        // there's no way to differentiate between them
                        // beacuse the data type from the procedure parameter doesn't come with the schema name
                        TVP tvp = database.TVPs.Where(t => string.Compare(t.tvp_name, parameter.data_type, true) == 0).FirstOrDefault<TVP>();
                        if (tvp != null)
                        {
                            sqlParameter.TypeName = parameter.data_type;
                            sqlParameter.SqlDbType = SqlDbType.Structured;
                            sqlParameter.Value = GetTVPDataTable(tvp);
                        }
                    }
                    break;
            }

            if (data_type == "binary" || data_type == "char" || data_type == "nchar" || data_type == "nvarchar" || data_type == "varbinary" || data_type == "varchar")
            {
                if (parameter.character_maximum_length == -1 || parameter.character_maximum_length > 0)
                    sqlParameter.Size = parameter.character_maximum_length.Value;
            }

            if (parameter.parameter_mode == "IN")
                sqlParameter.Direction = ParameterDirection.Input;
            else if (parameter.parameter_mode == "INOUT")
                sqlParameter.Direction = ParameterDirection.InputOutput;
            else if (parameter.parameter_mode == "OUT")
                sqlParameter.Direction = ParameterDirection.Output;

            return sqlParameter;
        }

        #endregion

        #region Connected Tables

        public static List<Table> GetConnectedTables(Table startTable, bool refFrom, bool refTo, bool recursively)
        {
            if (startTable == null)
                return new List<Table>(0);

            List<Table> tables = new List<Table>() { startTable };
            List<Table> newTables = new List<Table>();

            do
            {
                newTables.Clear();

                foreach (ForeignKey fk in startTable.Database.ForeignKeys)
                {
                    if (refFrom && tables.Contains(fk.FromTable) && tables.Contains(fk.ToTable) == false)
                        newTables.Add(fk.ToTable);
                    else if (refTo && tables.Contains(fk.FromTable) == false && tables.Contains(fk.ToTable))
                        newTables.Add(fk.FromTable);
                }

                tables.AddRange(newTables);

            } while (recursively && newTables.Count > 0);

            return tables;
        }

        #endregion
    }
}
