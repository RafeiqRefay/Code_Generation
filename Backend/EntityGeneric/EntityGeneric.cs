using System.Data.Common;
using System.Text;

public class EntityGeneric
{
    public List<ParameterInfoDTOView> parameterInfoDTOView { get; set; } = new();

    public List<ParameterInfoDTO> parameterInfoDTO { get; set; } = new();

    public string TableName { get; set; }

    public CodeGenerationPayload codeGenerationPayload { get; set; }

    public  string Code()
    {
        var sb = new StringBuilder();
        string fieldName =
          char.ToLower(TableName[0]) + TableName.Substring(1);



        sb.AppendLine($"export const {fieldName}Entity = {{");
        sb.AppendLine($"    entity: \"{TableName}s\",");
        sb.AppendLine("    operations: {");

        sb.Append(Operation());

        sb.AppendLine("    },");
        sb.AppendLine("};");

        return sb.ToString();
    }

    private string Operation()
    {
        var sb = new StringBuilder();

        if (codeGenerationPayload.Search == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.Append(BuildSearch());

            if (codeGenerationPayload.GetAll == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.Append(BuildGetAll());

        if (codeGenerationPayload.AddNew == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.Append(BuildAdd());

        if (codeGenerationPayload.UpdateBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.Append(BuildUpdate());

        if (codeGenerationPayload.DeleteBy == true || codeGenerationPayload.GAUDExtend == true || codeGenerationPayload.GAUDInit == true)
            sb.Append(BuildDelete());

        return sb.ToString();

    }
    private string BuildSearch()
    {
        var sb = new StringBuilder();



        sb.AppendLine("search: {");
        sb.AppendLine($"    endpoint: \"search{TableName}\",");
        sb.AppendLine("    filters: [");



            
  

        foreach (var item in parameterInfoDTOView )
        {
            if(item.UsedInGet==ParameterInfoDTOView.GetUsageTypeView.GetAll || item.UsedInGet == ParameterInfoDTOView.GetUsageTypeView.Both)
            {
              
              

                sb.AppendLine(
                    $"        {{ name: \"{item.Name}\", label: \"{item.Name}\", type: \"{item.DataType}\", }},");
            }
          
        }


        sb.AppendLine("],");

        sb.AppendLine("    columns: [");
        foreach (var item in parameterInfoDTOView)
        {

            string fieldName =
            char.ToLower(item.Name[0]) + item.Name.Substring(1);
            if (item.Name == "ID") {
                fieldName = "id";
            }


            sb.AppendLine(
                $"        {{ field: \"{fieldName}\", header: \"{item.Name}\" }},");
        }


        sb.AppendLine("    ],");
        sb.AppendLine("},");

        return sb.ToString();
        
    }


    private string BuildGetAll()
    {
        var sb = new StringBuilder();

        sb.AppendLine("getAll: {");
        sb.AppendLine("    endpoint: \"all\",");
        sb.AppendLine("    columns: [");

        foreach (var item in parameterInfoDTOView)
        {
            string fieldName =
            char.ToLower(item.Name[0]) + item.Name.Substring(1);
            if (item.Name == "ID")
            {
                fieldName = "id";
            }


            sb.AppendLine(
                $"        {{ field: \"{fieldName}\", header: \"{item.Name}\" }},");
        }


        sb.AppendLine("    ],");
        sb.AppendLine("},");

        return sb.ToString();
    }

    private string BuildAdd()
    {
        var sb = new StringBuilder();

        sb.AppendLine("add: {");
        sb.AppendLine("    endpoint: \"\",");
        sb.AppendLine("    fields: [");

        foreach (var item in parameterInfoDTO)
        {
            string fieldName =
           char.ToLower(item.Name[0]) + item.Name.Substring(1);
            if (item.Name == "ID")
            {
                fieldName = "id";
            }


            if (!item.IsPrimaryKey)
            {
                if (item.IsForeignKey)
                {
                    string ValueField = char.ToLower(item.ReferencedColumn[0]) + item.ReferencedColumn.Substring(1);
                    if (item.ReferencedColumn == "ID")
                    {
                        ValueField = "id";
                    }


                    sb.AppendLine($@"
        {{
            name: ""{fieldName}"",
            label: ""{item.Name}"",
            type: ""select"",
            source: {{
                entity: ""{item.NameTable}"",
                operation: ""getAll"",
                valueField: ""{ValueField}"",
                displayField: ""{item.Name}""
            }}
        }},");
                }
                else
                {
                    sb.AppendLine($@"
        {{
            name: ""{fieldName}"",
            label: ""{item.Name}"",
            type: ""{TypeInFront(item.DataType)}""
        }},");
                }
            }

      
        }

        sb.AppendLine("    ]");
        sb.AppendLine("},");

        return sb.ToString();
    }

    private string TypeInFront(string dataType)
    {
        switch (dataType.ToLower())
        {
            case "int":
            case "bigint":
            case "smallint":
            case "tinyint":
            case "decimal":
            case "numeric":
            case "float":
            case "real":
                return "number";

            case "bit":
            case "bool":
            case "boolean":
                return "checkbox";

            case "date":
                return "date";

            case "datetime":
            case "datetime2":
            case "smalldatetime":
                return "datetime-local";

            default:
                return "text";
        }
    }

    private string BuildUpdate()
    {
        var sb = new StringBuilder();


        var whereParams = parameterInfoDTO
            .Where(p => p.UseInWhere && (p.UsedInGet == ParameterInfoDTO.GetUsageType.Update || p.UsedInGet == ParameterInfoDTO.GetUsageType.Both))
            .ToList();


        var bySuffix = whereParams.Any()
    ? $"{string.Join(",", whereParams.Select(p => p.Name))}"
    : "";
     

        if (bySuffix == "ID")
        {
            bySuffix = "id";
        }
        sb.AppendLine("update: {");
        sb.AppendLine("    endpoint: \"{value}\",");
        sb.AppendLine($"    by: \"{bySuffix}\",");
        sb.AppendLine("    fields: [");

        foreach (var item in parameterInfoDTO.Where(x => x.UseColumnInUpdate))
        {
            string fieldName =
           char.ToLower(item.Name[0]) + item.Name.Substring(1);
            if (item.Name == "ID")
            {
                fieldName = "id";
            }


            if (item.IsForeignKey)
            {
                string ValueField = char.ToLower(item.ReferencedColumn[0]) + item.ReferencedColumn.Substring(1);
                if (item.ReferencedColumn == "ID")
                {
                    ValueField = "id";
                }

                sb.AppendLine($@"
        {{
            name: ""{fieldName}"",
            label: ""{item.Name}"",
            type: ""select"",
            source: {{
                entity: ""{item.NameTable}"",
                operation: ""getAll"",
                valueField: ""{ValueField}"",
                displayField: ""{item.Name}""
            }}
        }},");
            }
            else
            {
                sb.AppendLine($@"
        {{
            name: ""{fieldName}"",
            label: ""{item.Name}"",
            type: ""{TypeInFront(item.DataType)}""
        }},");
            }
        }

        sb.AppendLine("    ]");
        sb.AppendLine("},");

        return sb.ToString();
    }

    private string BuildDelete()
    {
        var sb = new StringBuilder();

     

        var whereParams = parameterInfoDTO
            .Where(p => p.UseInWhere && (p.UsedInGet == ParameterInfoDTO.GetUsageType.Update || p.UsedInGet == ParameterInfoDTO.GetUsageType.Both))
            .ToList();


        var bySuffix = whereParams.Any()
    ? $"{string.Join(",", whereParams.Select(p => p.Name))}"
    : "";
        if (bySuffix == "ID")
        {
            bySuffix = "id";
        }

        sb.AppendLine("delete: {");
        sb.AppendLine("    endpoint: \"{value}\",");
        sb.AppendLine($"    by: \"{bySuffix}\"");
        sb.AppendLine("},");

        return sb.ToString();
    }
}

