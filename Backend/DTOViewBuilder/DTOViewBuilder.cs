using System.Text;

public class DTOViewBuilder
{
    public List<ParameterInfoDTOView> parameterInfoDTOView { get; set; }
        = new List<ParameterInfoDTOView>();

    public string TableName { get; set; }

    public string BuildDTOView()
    {
        if (string.IsNullOrWhiteSpace(TableName))
            throw new InvalidOperationException("TableName is required");

        var sb = new StringBuilder();
        sb.AppendLine("using System.ComponentModel.DataAnnotations;\r\n");
        sb.AppendLine($"public class {TableName}ViewDTO");
        sb.AppendLine("{");

        // ===== Properties =====
        foreach (var p in parameterInfoDTOView)
        {
            string propName =p.Name;
            sb.AppendLine($"    public {p.DataType} {propName} {{ get; set; }}");
        }

        sb.AppendLine();

        // ===== Default ctor =====
        sb.AppendLine($"    public {TableName}ViewDTO() {{ }}");
        sb.AppendLine();

        // ===== Full ctor =====
        sb.Append($"    public {TableName}ViewDTO(");
        sb.Append(string.Join(", ",
            parameterInfoDTOView.Select(p =>
            {
                string propName =  p.Name;
                return $"{p.DataType} {ToCamelCase(propName)}";
            })));
        sb.AppendLine(")");
        sb.AppendLine("    {");

        foreach (var p in parameterInfoDTOView)
        {
            string propName =  p.Name;
            sb.AppendLine($"        {propName} = {ToCamelCase(propName)};");
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
