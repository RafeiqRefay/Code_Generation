
using System.Text;
using static ParameterInfoDTO;
using static ParameterInfoDTOView;


public class SqlBuilder
{
    public List<ParameterInfoDTOView> parameterInfoView { get; set; }
        = new List<ParameterInfoDTOView>();

    public string NameTable { get; set; }

    public List<ParameterInfoDTO> parameterInfoDTO { get; set; }
       = new List<ParameterInfoDTO>();
    public CodeGenerationPayload codeGenerationPayload { get; set; }

    public string ComplexQuere { get; set; } = null;







    public string GenerateAllSPs()
    {
        var sb = new StringBuilder();




        if (codeGenerationPayload.Search == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.AppendLine(GenerateSearchSP());
        sb.AppendLine("\nGO\n");
        if (codeGenerationPayload.GetAll == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.AppendLine(GenerateGetAllSP());
        sb.AppendLine("\nGO\n");

        if (codeGenerationPayload.AddNew == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.AppendLine(GenerateAddSP());
        sb.AppendLine("\nGO\n");


        if (codeGenerationPayload.UpdateBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.AppendLine(GenerateUpdateSP());
        sb.AppendLine("\nGO\n");

        if (codeGenerationPayload.DeleteBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.AppendLine(GenerateDeleteSP());
        sb.AppendLine("\nGO\n");
        if (codeGenerationPayload.GetOneBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)

            sb.AppendLine(GenerateGetOneBySP());
        sb.AppendLine("\nGO\n");


 

        return sb.ToString();
    }
    public string GenerateSearchSP()
    {
        // ===== تجهيز أعمدة WHERE =====
        var sb = new StringBuilder();

        var whereColumns = parameterInfoView
            .Where(p => p.UseInWhere &&
                       (p.UsedInGet == GetUsageTypeView.GetAll || p.UsedInGet == GetUsageTypeView.Both))
            .ToList();




        sb.AppendLine($"CREATE PROCEDURE [dbo].[SP_Search{NameTable}]");

        // ===== إضافة Parameters للـ WHERE =====
        if (whereColumns.Any())
        {
            var paramLines = whereColumns
                .Select(p => $"    @{p.Name} {(string.IsNullOrEmpty(p.DataTypeSql) ? "NVARCHAR(MAX)" : p.DataTypeSql)}" + "=NULL");
            sb.AppendLine(string.Join(",\n", paramLines));
        }

        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine();

        // ===== SELECT =====


        if (ComplexQuere == null)
        {
            sb.AppendLine("        SELECT");
            sb.AppendLine(BuildSelectColumns());
            sb.AppendLine($"        FROM [{NameTable}]");

        }
        else
        {
            sb.AppendLine(ComplexQuere);


        }

        int Count=whereColumns.Count();
        sb.AppendLine("where");
        sb.AppendLine("(");
        foreach (var param in whereColumns) {
            Count--;

            if (param.DataType == "int")
            {
                sb.AppendLine("(@" + param.Name + " is null or " + param.Name + " =@" + param.Name+")");
            }

          
            if (param.DataType != "int")
            {
                sb.AppendLine("(@" + param.Name + " is null or " + param.Name + " COLLATE SQL_Latin1_General_CP1_CI_AS LIKE @" + param.Name+" + '%')");


            }
            if (Count>= 1)
            {
                sb.AppendLine(" and");
            }

        }
        sb.AppendLine(")");

        sb.AppendLine(";");
        sb.AppendLine("END");

        return sb.ToString();

    }

    public string GenerateGetAllSP()
    {
        var sb = new StringBuilder();
      
        if (ComplexQuere == null)
        {
          
        }
        else { 
   
        }
        // ===== تجهيز أعمدة WHERE =====
       

      

        var spName = $"SP_GetAll{NameTable}";

        sb.AppendLine($"CREATE PROCEDURE [dbo].[{spName}]");

        

        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine();

        // ===== SELECT =====


        if (ComplexQuere == null)
        {
            sb.AppendLine("        SELECT");
            sb.AppendLine(BuildSelectColumns());
            sb.AppendLine($"        FROM [{NameTable}]");

        }
        else
        {
            sb.AppendLine(ComplexQuere);


        }





        sb.AppendLine(";");
        sb.AppendLine("END");

        return sb.ToString();
    }





    // ===================== FINAL SP =====================
    public string GenerateGetOneBySP()
    {
        var byColumns = GetByColumns();
        if (!byColumns.Any())
            throw new Exception("No columns defined for GetOne SP");

        var spName = BuildProcedureName(byColumns);
        var sb = new StringBuilder();

        sb.AppendLine($"CREATE PROCEDURE [dbo].[{spName}]");
        sb.AppendLine(BuildParameters(byColumns));
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine("    SET NOCOUNT ON;");
        sb.AppendLine();
        sb.AppendLine("    BEGIN TRY");
        sb.AppendLine();
        sb.AppendLine(BuildIfNotExists(byColumns));
        sb.AppendLine();


        if (ComplexQuere == null)
        {
            sb.AppendLine("        SELECT");
            sb.AppendLine(BuildSelectColumns());
            sb.AppendLine($"        FROM [{NameTable}]");

        }
        else
        {
            sb.AppendLine(ComplexQuere);


        };
       
        sb.AppendLine(BuildWhereClause(byColumns));
        sb.AppendLine();
        sb.AppendLine($"        SET @Message = '{NameTable} loaded successfully.';");
        sb.AppendLine("        SET @ErrorType = 0;");
        sb.AppendLine();
        sb.AppendLine("    END TRY");
        sb.AppendLine("    BEGIN CATCH");
        sb.AppendLine("        SET @Message =");
        sb.AppendLine("            ERROR_MESSAGE() + ' | Line: ' + CAST(ERROR_LINE() AS VARCHAR);");
        sb.AppendLine("        SET @ErrorType = 5;");
        sb.AppendLine("    END CATCH");
        sb.AppendLine("END");

        return sb.ToString();
    }

    // ======================================================
    // HELPERS
    // ======================================================

    private List<ParameterInfoDTOView> GetByColumns()
    {
        return parameterInfoView
            .Where(c => c.UseInWhere &&
                   (c.UsedInGet == GetUsageTypeView.GetOne ||
                    c.UsedInGet == GetUsageTypeView.Both))
            .ToList();
    }

    private string BuildProcedureName(List<ParameterInfoDTOView> byColumns)
    {
        return $"SP_Get{NameTable}By{string.Join("And", byColumns.Select(c => c.Name))}";
    }

    private string BuildParameters(List<ParameterInfoDTOView> byColumns)
    {
        var sb = new StringBuilder();

        foreach (var col in byColumns)
        {
           
           
            
                sb.AppendLine($"    @{col.Name} {col.DataTypeSql},");

            
        }

        sb.AppendLine("    @Message VARCHAR(150) OUTPUT,");
        sb.AppendLine("    @ErrorType INT OUTPUT");

        return sb.ToString();
    }

    private string BuildIfNotExists(List<ParameterInfoDTOView> byColumns)
    {
        return $@"
        IF NOT EXISTS (
                    SELECT 1
            FROM [{NameTable}]
            {BuildWhereClause(byColumns)}
        )
        BEGIN
            SET @Message = '{NameTable} not found.';
            SET @ErrorType = 2;
            RETURN;
        END";
    }

    private string BuildIfNotExistsComplex(List<ParameterInfoDTOView> byColumns)
    {
        return $@"
        IF NOT EXISTS (
        {ComplexQuere}
            
            {BuildWhereClause(byColumns)}
        )
        BEGIN
            SET @Message = '{NameTable} not found.';
            SET @ErrorType = 2;
            RETURN;
        END";
    }

    private string BuildSelectColumns()
    {
        var distinctColumns = parameterInfoView
            .GroupBy(c =>  $"{NameTable : c.NameTable)}.{c.Name}")
            .Select(g => g.First());
     

        var list = new List<string>();

        foreach (var col in distinctColumns)
        {
            var sourceTable = NameTable;

        

            list.Add($"            [{sourceTable}].[{col.Name}]");
        }

        return string.Join(",\n", list);
    }

   
    private string BuildWhereClause(List<ParameterInfoDTOView> byColumns)
    {
        var conditions = byColumns
            .Select(c =>  $"[{NameTable}].[{c.Name}] = @{c.Name}");

        return $"        WHERE {string.Join(" AND ", conditions)}";
    }

    // ===================== UPDATE =====================
    public string GenerateUpdateSP()
    {
        if (string.IsNullOrWhiteSpace(NameTable))
            throw new Exception("Table name is required.");

        // الأعمدة المستخدمة في WHERE

        var whereColumns = parameterInfoDTO
            .Where(p => p.UseInWhere && ((p.UsedInGet == ParameterInfoDTO.GetUsageType.Update) || (p.UsedInGet == ParameterInfoDTO.GetUsageType.Both)))
            .ToList();

        

        // الأعمدة المستخدمة في SET
        var setColumns = parameterInfoDTO
            .Where(p => p.UseColumnInUpdate)
            .ToList();

        // الأعمدة الأجنبية المستخدمة في UPDATE للتحقق من وجود القيمة في الجدول المرتبط
        var foreignCheckColumns = setColumns
            .Where(p => p.IsForeignKey)
            .ToList();

        // اسم الإجراء
        string procedureName = $"SP_Update{NameTable}By{string.Join("And", whereColumns.Select(c => c.Name))}";

        // بناء قائمة المتغيرات
        var parameters = whereColumns
            .Select(c => $"@{c.Name} {c.DataTypeSql}")
            .ToList();

        parameters.AddRange(setColumns
            .Where(c => !whereColumns.Contains(c))
            .Select(c => $"@{c.Name} {c.DataTypeSql}"));

        // إضافة متغير @Message و @ErrorType
        parameters.Add("@Message NVARCHAR(250) OUTPUT");
        parameters.Add("@ErrorType INT OUTPUT");

        // شرط WHERE
        string whereCondition = string.Join(" AND ", whereColumns.Select(c => $"[{c.Name}] = @{c.Name}"));

        // SET clause
        string setClause = string.Join(",\n            ",
       setColumns
           .Where(c => !(c.UseInWhere &&
                        (c.UsedInGet == ParameterInfoDTO.GetUsageType.Update ||
                         c.UsedInGet == ParameterInfoDTO.GetUsageType.Both)))
           .Select(c => $"[{c.Name}] = COALESCE(@{c.Name},{c.Name})")
   );


        // SELECT الأعمدة كاملة
        string selectColumns = string.Join(", ", parameterInfoDTO.Select(c => $"[{NameTable}].[{c.Name}]"));

        // بناء نص الإجراء
        string sql = $@"
Create PROCEDURE [dbo].[{procedureName}]
    {string.Join(",\n    ", parameters)}
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- تحقق وجود الصف
           IF NOT EXISTS (SELECT 1 FROM [{NameTable}] WHERE {whereCondition})
        BEGIN
            SET @Message = '{NameTable} not found.';
            SET @ErrorType = 2;
            RETURN;
        END

        -- تحقق الأعمدة الأجنبية
";

        // إضافة تحقق FOREIGN KEY
        foreach (var col in foreignCheckColumns)
        {
            string foreignTable = !string.IsNullOrWhiteSpace(col.NameTable) ? col.NameTable : col.NameTable;
            sql += $@"
        IF @{col.Name} IS NOT NULL AND  NOT EXISTS (SELECT 1 FROM [{foreignTable}] WHERE [{foreignTable}].[{col.ReferencedColumn}] = @{col.Name})
        BEGIN
            SET @Message = 'Invalid [{foreignTable}]';
            SET @ErrorType = 3;  -- AlreadyExists
            RETURN;
        END
";
        }

        // متابعة العملية
        sql += $@"
        -- تحديث الصف
        UPDATE [{NameTable}]
        SET
            {setClause}
        WHERE {whereCondition};

        SET @Message = '{NameTable} updated successfully.';
        SET @ErrorType = 0;

        -- ارجاع الصف المحدث
        SELECT {selectColumns}
        FROM [{NameTable}]
        WHERE {whereCondition};

    END TRY
    BEGIN CATCH
        SET @Message = ERROR_MESSAGE() + ' | Line: ' + CAST(ERROR_LINE() AS NVARCHAR);
        SET @ErrorType = 5;
    END CATCH
END
";

        return sql;
    }

















    public string GenerateAddSP()
    {
        if (parameterInfoDTO == null || !parameterInfoDTO.Any())
            throw new Exception("No columns defined for Add SP");

        var sb = new StringBuilder();

        // ===== اسم الـ SP =====
        sb.AppendLine($"Create PROCEDURE [dbo].[SP_Add{NameTable}]");

        // ===== تعريف الـ parameters =====
        var parameters = parameterInfoDTO
            .Where(p => !p.IsPrimaryKey) // لا تضيف PK
            .Select(p => $"    @{p.Name} {p.DataTypeSql}");

        sb.AppendLine(string.Join(",\n", parameters));

        // ===== الـ parameters الثابتة =====
        sb.AppendLine("    ,@NewID INT OUTPUT");
        sb.AppendLine("    ,@Message NVARCHAR(250) OUTPUT");
        sb.AppendLine("    ,@ErrorType INT OUTPUT");

        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine("    SET NOCOUNT ON;");
        sb.AppendLine();
        sb.AppendLine("    BEGIN TRY");

        // ===== تحقق من FK =====
        foreach (var fk in parameterInfoDTO.Where(p => p.IsForeignKey))
        {
            sb.AppendLine($"        IF NOT EXISTS (SELECT 1 FROM {fk.NameTable} WHERE {fk.ReferencedColumn} = @{fk.Name})");
            sb.AppendLine("        BEGIN");
            sb.AppendLine("            SET @NewID = -1;");
            sb.AppendLine($"            SET @Message = 'Invalid {fk.NameTable}.';");
            sb.AppendLine("            SET @ErrorType = 3;  -- AlreadyExists");
            sb.AppendLine("            RETURN;");
            sb.AppendLine("        END");
        }

        sb.AppendLine();

        // ===== INSERT ديناميكي =====
        var insertColumns = parameterInfoDTO.Where(p => !p.IsPrimaryKey).Select(p => p.Name).ToList();
        var insertValues = parameterInfoDTO.Where(p => !p.IsPrimaryKey).Select(p => "@" + p.Name).ToList();

        sb.AppendLine($"        INSERT INTO {NameTable} ({string.Join(", ", insertColumns)})");
        sb.AppendLine($"        VALUES ({string.Join(", ", insertValues)});");

        sb.AppendLine();
        sb.AppendLine("        SET @NewID = SCOPE_IDENTITY();");
        sb.AppendLine($"        SET @Message = '{NameTable} added successfully.';");
        sb.AppendLine("        SET @ErrorType = 0;  -- Success");

        sb.AppendLine();
        sb.AppendLine("    END TRY");
        sb.AppendLine("    BEGIN CATCH");
        sb.AppendLine("        SET @NewID = -1;");
        sb.AppendLine("        SET @Message = ERROR_MESSAGE();");
        sb.AppendLine("        SET @ErrorType = 5;  -- DatabaseError");
        sb.AppendLine("    END CATCH");
        sb.AppendLine("END");

        return sb.ToString();
    }
    public string GenerateDeleteSP()
    {
        if (string.IsNullOrWhiteSpace(NameTable))
            throw new Exception("Table name is required.");

        // الأعمدة المستخدمة في WHERE للحذف
        var whereColumns = parameterInfoDTO
       .Where(p => p.UseInWhere && ((p.UsedInGet == ParameterInfoDTO.GetUsageType.Delete) || (p.UsedInGet == ParameterInfoDTO.GetUsageType.Both)))
            .ToList();

        if (!whereColumns.Any())
            throw new Exception("At least one column must be used in WHERE clause for Delete.");

        // اسم الإجراء
        string procedureName = $"SP_Delete{NameTable}By{string.Join("And", whereColumns.Select(c => c.Name))}";

        // بناء قائمة المتغيرات
        var parameters = whereColumns
            .Select(c => $"@{c.Name} {c.DataTypeSql}")
            .ToList();

        // إضافة @Message و @ErrorType
        parameters.Add("@Message NVARCHAR(250) OUTPUT");
        parameters.Add("@ErrorType INT OUTPUT");

        // بناء شرط WHERE
        string whereCondition = string.Join(" AND ", whereColumns.Select(c => $"[{c.Name}] = @{c.Name}"));

        // بناء النص النهائي للإجراء
        string sql = $@"
CREATE PROCEDURE [dbo].[{procedureName}]
    {string.Join(",\n    ", parameters)}
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- التحقق أن الصف موجود
        IF NOT EXISTS (SELECT 1 FROM [{NameTable}] WHERE {whereCondition})
        BEGIN
            SET @Message = '{NameTable} not found.';
            SET @ErrorType = 2;
            RETURN;
        END

        DELETE FROM [{NameTable}] WHERE {whereCondition};

        SET @Message = '{NameTable} deleted successfully.';
        SET @ErrorType = 0;

    END TRY
    BEGIN CATCH
        -- FK Constraint
        IF ERROR_NUMBER() = 547
        BEGIN
            SET @Message = 'Cannot delete {NameTable}. It is linked to other records.';
            SET @ErrorType = 4;
        END
        ELSE
        BEGIN
            SET @Message = ERROR_MESSAGE();
            SET @ErrorType = 5;
        END
    END CATCH
END
";

        return sql;
    }




}