using System.Text;

public class DTOBuilder
{
    public List<ParameterInfoDTO> parameterInfoDTO { get; set; }
        = new List<ParameterInfoDTO>();

    public string TableName { get; set; }

    public string BuildDTO()
    {
        if (string.IsNullOrWhiteSpace(TableName))
            throw new InvalidOperationException("TableName is required");

        var sb = new StringBuilder();
        sb.AppendLine("using System.ComponentModel.DataAnnotations;\r\n");
        sb.AppendLine($"public class {TableName}DTO");
        sb.AppendLine("{");

        // ===== Properties =====
        foreach (var p in parameterInfoDTO)
        {
            if (p.IsForeignKey && p.DataType == "int")
            {
                sb.AppendLine(
                    $"    [Range(1, int.MaxValue, ErrorMessage = \"Invalid {p.Name}.\")]");
            }

            sb.AppendLine($"    public {p.DataType} {p.Name} {{ get; set; }}");
        }

        sb.AppendLine();

        // ===== Default ctor =====
        sb.AppendLine($"    public {TableName}DTO() {{ }}");
        sb.AppendLine();

        // ===== Full ctor =====
        sb.Append($"    public {TableName}DTO(");
        sb.Append(string.Join(", ",
            parameterInfoDTO.Select(p =>
                $"{p.DataType} {ToCamelCase(p.Name)}")));
        sb.AppendLine(")");
        sb.AppendLine("    {");

        foreach (var p in parameterInfoDTO)
        {
            sb.AppendLine($"        {p.Name} = {ToCamelCase(p.Name)};");
        }

        sb.AppendLine("    }");
        sb.AppendLine("}");

        return sb.ToString();
    }

    // ===== Helper =====
    private string ToCamelCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}