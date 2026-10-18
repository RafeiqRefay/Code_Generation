function ConvertToTypeSql(dataType) {
  if (dataType === "int") {
    return "int";
  } else if (dataType === "long") {
    return "bigint";
  } else if (dataType === "string") {
    return "nvarchar(MAX)";
  } else if (dataType === "DateTime") {
    return "datetime";
  } else if (dataType === "bool") {
    return "bit";
  } else if (dataType === "decimal") {
    return "decimal(18,2)";
  } else {
    return "nvarchar(MAX)";
  }
}
let CountRowDTO = 1;
let CountRowDTOView = 1;

function addRowDTO() {
  // نحدد tbody
  const tbody = document.getElementById("dtoTableBodyUnique");

  // نأخذ الصف الأصلي
  const originalRow = document.getElementById("dtoRow1");

  // ننسخ الصف بالكامل مع كل العناصر
  const newRow = originalRow.cloneNode(true); // true = نسخ كل المحتوى

  // نضيف الصف الجديد إلى نهاية tbody مباشرة
  tbody.appendChild(newRow);
}

const AddNewRowDTO = document.getElementById("btnAddDtoRow");
AddNewRowDTO.onclick = function () {
  addRowDTO();
};

function addRowDTOView() {
  const tbody = document.getElementById("viewDtoTableBodyUnique");

  // نأخذ الصف الأصلي
  const originalRow = document.getElementById("viewDtoRow1");

  // ننسخ الصف بالكامل مع كل العناصر
  const newRow = originalRow.cloneNode(true); // true = نسخ كل المحتوى

  // نضيف الصف الجديد إلى نهاية tbody مباشرة
  tbody.appendChild(newRow);
}

const AddNewRowDTOView = document.getElementById("btnAddViewDtoRow");
AddNewRowDTOView.onclick = function () {
  addRowDTOView();
};

let DtoParameters = [];
const GetUsageType = {
  None: 0,
  Delete: 1,
  Update: 2,
  Both: 3,
};

class InfoDTO {
  constructor({
    Name,
    DataType,
    DataTypeSql,
    NameTable,
    IsPrimaryKey,
    IsForeignKey,
    UseInWhere,
    UsedInGet,
    UseColumnInUpdate,
    ReferencedColumn,
  }) {
    this.Name = Name;
    this.DataType = DataType;
    this.DataTypeSql = DataTypeSql;
    this.NameTable = NameTable;
    this.IsPrimaryKey = IsPrimaryKey;
    this.IsForeignKey = IsForeignKey;
    this.UseInWhere = UseInWhere;
    this.UsedInGet = UsedInGet;
    this.UseColumnInUpdate = UseColumnInUpdate;
    this.ReferencedColumn = ReferencedColumn;
  }
}

let DtoViewParameters = [];

const GetUsageTypeView = {
  None: 0,
  GetAll: 1,
  GetOne: 2,
  Both: 3,
};

class InfoDTOView {
  constructor({
    Name,
    DataType,
    DataTypeSql,

    IsPrimaryKey,

    UseInWhere,
    UsedInGet,
  }) {
    this.Name = Name;
    this.DataType = DataType;
    this.DataTypeSql = DataTypeSql;

    this.IsPrimaryKey = IsPrimaryKey;

    this.UseInWhere = UseInWhere;
    this.UsedInGet = UsedInGet; // من نوع GetUsageTypeView
  }
}
function LoadDataIntoDtoViewParameters() {
  const rows = document.querySelectorAll("#dtoTableBodyUnique tr");
  const dtoParameters = [];

  rows.forEach((row) => {
    const cells = row.querySelectorAll("td");

    const name = cells[0]?.querySelector("input")?.value?.trim() || "";
    const dataType = cells[1]?.querySelector("select")?.value?.trim() || "";
    const isPrimaryKey = cells[2]?.querySelector("input")?.checked ?? null;
    const isForeignKey = cells[3]?.querySelector("input")?.checked ?? null;
    const useInWhere = cells[4]?.querySelector("input")?.checked ?? null;
    const usedInGet =
      parseInt(cells[5]?.querySelector("select")?.value) ||
      GetUsageTypeView.None;
    const useInUpdate = cells[6]?.querySelector("input")?.checked ?? "";
    const tableName = cells[7]?.querySelector("input")?.value?.trim() || "";
    const ReferencedColumn =
      cells[8]?.querySelector("input")?.value?.trim() || "";

    const dtoObject = new InfoDTO({
      Name: name,
      DataType: dataType,
      DataTypeSql: ConvertToTypeSql(dataType),
      NameTable: tableName,
      IsPrimaryKey: isPrimaryKey,
      IsForeignKey: isForeignKey,
      UseInWhere: useInWhere,
      UsedInGet: usedInGet,
      UseColumnInUpdate: useInUpdate,
      ReferencedColumn: ReferencedColumn,
    });

    dtoParameters.push(dtoObject);
  });

  // تحديث المصفوفة العالمية
  DtoParameters = dtoParameters;

  console.log("DTO Parameters:", DtoParameters);
}

function LoadDataIntoDtoViewParametersView() {
  const rows = document.querySelectorAll("#viewDtoTableBodyUnique tr");
  const dtoViewParameters = [];

  rows.forEach((row) => {
    const cells = row.querySelectorAll("td");

    const name = cells[0]?.querySelector("input")?.value?.trim() || "";
    const dataType = cells[1]?.querySelector("select")?.value?.trim() || "";
    const isPrimaryKey = cells[2]?.querySelector("input")?.checked ?? null;

    const useInWhere = cells[3]?.querySelector("input")?.checked ?? null;
    const usedInGet =
      parseInt(cells[4]?.querySelector("select")?.value) ||
      GetUsageTypeView.None;

    const dtoViewObject = new InfoDTOView({
      Name: name,
      DataType: dataType,
      DataTypeSql: ConvertToTypeSql(dataType),

      IsPrimaryKey: isPrimaryKey,

      UseInWhere: useInWhere,
      UsedInGet: usedInGet,
    });

    dtoViewParameters.push(dtoViewObject);
  });

  // تحديث المصفوفة العالمية
  DtoViewParameters = dtoViewParameters;

  console.log("DTO View Parameters:", DtoViewParameters);
}

function LoadDataIntoPayload() {
  return {
    dtoParameters: DtoParameters || [],
    dtoViewParameters: DtoViewParameters || [],
    tableName:
      document.getElementById("tableNameInputUnique")?.value?.trim() || null,
    deleteBy: document.getElementById("opDeleteByUnique")?.checked ?? null,
    updateBy: document.getElementById("opUpdateByUnique")?.checked ?? null,
    addNew: document.getElementById("opAddNewUnique")?.checked ?? null,
    getOneBy: document.getElementById("opGetOneByUnique")?.checked ?? null,
    getAll: document.getElementById("opGetAllUnique")?.checked ?? null,
      Search: document.getElementById("opSearchUnique")?.checked??null,
    allGAUD: document.getElementById("opModeAllGaud")?.checked ?? null,
    GAUDInit: document.getElementById("opModeAllGaudFile")?.checked ?? null,
    GAUDExtend:
      document.getElementById("AllGAUDWhitOutCreateFile")?.checked ?? null,
    ProjectPath: document.getElementById("ProjectPath")?.value?.trim() || null,
    DataBaseName:
      document.getElementById("DataBaseName")?.value?.trim() || null,
    ComplexQuere: document.getElementById("opComplexQuery")?.checked ?? null,
    GetQuereComplex:
      document.getElementById("ComplexQueryText")?.value?.trim() || null,
      PathPage:document.getElementById("PathPage")?.value?.trim() || null,
      PathStore:document.getElementById("PathStore")?.value?.trim() || null,
      PathEntity:document.getElementById("PathEntity")?.value?.trim() || null,
  };
}
const GenerateCode = document.getElementById("btnGenerateCode");

GenerateCode.onclick = async function () {
  LoadDataIntoDtoViewParameters();
  LoadDataIntoDtoViewParametersView();

  const createPayload = LoadDataIntoPayload();

  try {
    const response = await fetch(
      "https://localhost:7068/api/CodeGeneration/generate",
      {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(createPayload),
      },
    );

    if (!response.ok) throw new Error("Request failed: " + response.status);

    const result = await response.json();
    console.log("Full API Response:", result);

    // حفظ البيانات مؤقتًا في الجلسة
    sessionStorage.setItem("apiResult", JSON.stringify(result));

    // فتح الصفحة الجديدة في نفس التبويب
    window.location.href = "newpage.html";
  } catch (error) {
    console.error("Error sending request:", error);
  }
};

// تأكد أن الصفحة محملة قبل إضافة الحدث
document.addEventListener("DOMContentLoaded", function () {
  const tableNameInput = document.getElementById("tableNameInputUnique");

  tableNameInput.addEventListener("input", function () {
    const tableName = this.value;

    // مثال: تحديث summary مباشرة
    const summaryTableName = document.querySelector(
      "#summaryTableName .summary-value",
    );
    summaryTableName.textContent = tableName || "—";
  });
});

document.addEventListener("DOMContentLoaded", function () {
  const RowDTOInput = document.getElementById("btnAddDtoRow");
  RowDTOInput.addEventListener("click", function () {
    CountRowDTO++;

    const summaryRowDTO = document.querySelector(
      "#summaryDtoColumnsCount .summary-value",
    );
    summaryRowDTO.textContent = CountRowDTO || "0";
  });
});

document.addEventListener("DOMContentLoaded", function () {
  const RowDTOViewInput = document.getElementById("btnAddViewDtoRow");
  RowDTOViewInput.addEventListener("click", function () {
    CountRowDTOView++;

    const summaryRowDTOView = document.querySelector(
      "#summaryViewDtoColumnsCount .summary-value",
    );
    summaryRowDTOView.textContent = CountRowDTOView || "0";
  });
});
CountRowDTO = document.querySelectorAll("#dtoTableBodyUnique tr").length;

const summaryRowDTO = document.querySelector(
  "#summaryDtoColumnsCount .summary-value",
);
summaryRowDTO.textContent = CountRowDTO;

document
  .getElementById("dtoTableBodyUnique")
  .addEventListener("click", function (e) {
    if (e.target.classList.contains("btn-row-delete")) {
      // حذف الصف الأب مباشرة
      e.target.closest("tr").remove();

      // تحديث العدّاد
      CountRowDTO = document.querySelectorAll("#dtoTableBodyUnique tr").length;
      summaryRowDTO.textContent = CountRowDTO || "0";
    }
  });
CountRowDTOView = document.querySelectorAll(
  "#viewDtoTableBodyUnique tr",
).length;

const summaryRowDTOView = document.querySelector(
  "#summaryViewDtoColumnsCount .summary-value",
);
summaryRowDTOView.textContent = CountRowDTOView;

document
  .getElementById("viewDtoTableBodyUnique")
  .addEventListener("click", function (e) {
    if (e.target.classList.contains("btn-row-delete")) {
      e.target.closest("tr").remove();

      CountRowDTOView = document.querySelectorAll(
        "#viewDtoTableBodyUnique tr",
      ).length;
      summaryRowDTOView.textContent = CountRowDTOView || "0";
    }
  });

document.addEventListener("DOMContentLoaded", function () {
  const operationCheckboxes = [
    "opGetAllUnique",
    "opGetAllByUnique",
    "opGetOneByUnique",
    "opAddNewUnique",
    "opUpdateByUnique",
    "opDeleteByUnique",
    "opSearchUnique",
  ].map((id) => document.getElementById(id));

  const radioAllGaud = document.getElementById("opModeAllGaud");
  const radioCustom = document.getElementById("opModeCustom");

  const summarySelectedOperations = document.querySelector(
    "#summarySelectedOperations .summary-value",
  );

  function updateSelectedOperations() {
    // إذا وضع "All GAUD" مفعل، نعرض "All GAUD"
    if (radioAllGaud.checked) {
      summarySelectedOperations.textContent = "All GAUD";
      return;
    }

    // إذا Custom مفعل أو لا يوجد اختيار للـ radio، نعرض العمليات المختارة فقط
    const selectedOps = operationCheckboxes
      .filter((chk) => chk.checked)
      .map((chk) => chk.nextElementSibling.textContent.trim());

    summarySelectedOperations.textContent =
      selectedOps.length > 0 ? selectedOps.join(", ") : "None";
  }

  // إضافة حدث onchange لكل checkbox
  operationCheckboxes.forEach((chk) => {
    chk.addEventListener("change", updateSelectedOperations);
  });

  // إضافة حدث onchange للراديو
  radioAllGaud.addEventListener("change", updateSelectedOperations);
  radioCustom.addEventListener("change", updateSelectedOperations);

  // تحديث أولي عند تحميل الصفحة
  updateSelectedOperations();
});
document.addEventListener("DOMContentLoaded", function () {
  const init = document.getElementById("opModeAllGaudFile");
  const extend = document.getElementById("AllGAUDWhitOutCreateFile");

  const configSection = document.getElementById("projectConfigSection");

 

  document.querySelectorAll('input[name="operationsMode"]').forEach((radio) => {
    radio.addEventListener("change", toggleProjectFields);
  });

  // تشغيل أولي لو كان في خيار محدد مسبقًا

});

const complexCheckbox = document.getElementById("opComplexQuery");
const complexContainer = document.getElementById("complexQueryContainer");

complexCheckbox.addEventListener("change", function () {
  if (this.checked) {
    complexContainer.style.display = "block";
  } else {
    complexContainer.style.display = "none";
  }
});

document.addEventListener("DOMContentLoaded", function () {
  const tableScript = `DECLARE @TableName NVARCHAR(128) = '';
DECLARE @Result NVARCHAR(MAX);

SELECT @Result = STRING_AGG(
    '||' +
    c.COLUMN_NAME + ',' +
    CASE c.DATA_TYPE
        WHEN 'int' THEN 'int'
        WHEN 'bigint' THEN 'long'
        WHEN 'smallint' THEN 'short'
        WHEN 'tinyint' THEN 'byte'
        WHEN 'bit' THEN 'bool'
        WHEN 'decimal' THEN 'decimal'
        WHEN 'numeric' THEN 'decimal'
        WHEN 'float' THEN 'double'
        WHEN 'real' THEN 'float'
        WHEN 'nvarchar' THEN 'string'
        WHEN 'varchar' THEN 'string'
        WHEN 'char' THEN 'string'
        WHEN 'nchar' THEN 'string'
        WHEN 'datetime' THEN 'DateTime'
        WHEN 'date' THEN 'DateTime'
        WHEN 'time' THEN 'TimeSpan'
        ELSE 'object'
    END + ',' +
    CASE WHEN fk.parent_column_id IS NOT NULL THEN 'True' ELSE 'False' END + ',' +
    CASE 
        WHEN fk.parent_column_id IS NOT NULL 
        THEN rt.name + ',' + rc.name
        ELSE 'NULL,NULL'
    END +
    '||'
, CHAR(13))
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN sys.columns sc
    ON sc.name = c.COLUMN_NAME
    AND sc.object_id = OBJECT_ID(@TableName)
LEFT JOIN sys.foreign_key_columns fk
    ON fk.parent_object_id = sc.object_id
    AND fk.parent_column_id = sc.column_id
LEFT JOIN sys.tables rt
    ON fk.referenced_object_id = rt.object_id
LEFT JOIN sys.columns rc
    ON rc.object_id = rt.object_id
    AND rc.column_id = fk.referenced_column_id
WHERE c.TABLE_NAME = @TableName;

SELECT @Result AS Result;`;

  const queryScript = `



DECLARE @SQL NVARCHAR(MAX) = N'
 Quere

';

DECLARE @Columns NVARCHAR(MAX);
DECLARE @CSharpTypes NVARCHAR(MAX);

SELECT 
    @Columns = STRING_AGG(name, ',') WITHIN GROUP (ORDER BY column_ordinal),
    @CSharpTypes = STRING_AGG(
        CASE 
            WHEN system_type_name LIKE '%int%' THEN 'int'
            WHEN system_type_name LIKE '%bigint%' THEN 'long'
            WHEN system_type_name LIKE '%smallint%' THEN 'short'
            WHEN system_type_name LIKE '%tinyint%' THEN 'byte'
            WHEN system_type_name LIKE '%bit%' THEN 'bool'
            WHEN system_type_name LIKE '%decimal%' THEN 'decimal'
            WHEN system_type_name LIKE '%numeric%' THEN 'decimal'
            WHEN system_type_name LIKE '%float%' THEN 'double'
            WHEN system_type_name LIKE '%real%' THEN 'float'
            WHEN system_type_name LIKE '%nvarchar%' THEN 'string'
            WHEN system_type_name LIKE '%varchar%' THEN 'string'
            WHEN system_type_name LIKE '%char%' THEN 'string'
            WHEN system_type_name LIKE '%nchar%' THEN 'string'
            WHEN system_type_name LIKE '%datetime%' THEN 'DateTime'
            WHEN system_type_name LIKE '%date%' THEN 'DateTime'
            WHEN system_type_name LIKE '%time%' THEN 'TimeSpan'
            ELSE 'object'
        END
    , ',') 
    WITHIN GROUP (ORDER BY column_ordinal)
FROM sys.dm_exec_describe_first_result_set(@SQL, NULL, 1)
WHERE is_hidden = 0;  -- يتجاهل أي أعمدة system أو duplicates

SELECT @Columns + '&&' + @CSharpTypes AS Result;
`;

  document.getElementById("tableMetadataInput").value = tableScript;
  document.getElementById("queryMetadataInput").value = queryScript;
});
function copyToClipboard(text) {
  navigator.clipboard
    .writeText(text)
    .then(() => alert("Copied successfully ✅"))
    .catch(() => alert("Copy failed ❌"));
}

document
  .getElementById("btnCopyTableMetadata")
  .addEventListener("click", function () {
    const text = document.getElementById("tableMetadataInput").value;
    copyToClipboard(text);
  });

document
  .getElementById("btnCopyQueryMetadata")
  .addEventListener("click", function () {
    const text = document.getElementById("queryMetadataInput").value;
    copyToClipboard(text);
  });
function parseDtoMetadata(text) {
  const rows = [];
  if (!text) return rows;

  const regex = /\|\|([^|]+)\|\|/g;
  let match;

  while ((match = regex.exec(text)) !== null) {
    const parts = match[1].trim().split(",");

    if (parts.length >= 5) {
      rows.push({
        name: parts[0].trim(),
        type: parts[1].trim(),
        isFk: parts[2].trim().toLowerCase() === "true",
        refTable: parts[3].trim() !== "NULL" ? parts[3].trim() : "",
        refColumn: parts[4].trim() !== "NULL" ? parts[4].trim() : "",
      });
    }
  }

  return rows;
}

document
  .getElementById("btnFillDtoTable")
  .addEventListener("click", function () {
    const text = document.getElementById("dtoAutoFillInput").value;
    const parsed = parseDtoMetadata(text);
    const tbody = document.getElementById("dtoTableBodyUnique");

    tbody.innerHTML = "";

    parsed.forEach((col, index) => {
      const row = document.createElement("tr");
      CountRowDTO++;
      row.innerHTML = `
      <td><input type="text" class="table-input" value="${col.name}" /></td>

      <td>
        <select class="table-select">
          <option value="int">int</option>
          <option value="long">long</option>
          <option value="string">string</option>
          <option value="DateTime">DateTime</option>
          <option value="bool">bool</option>
          <option value="decimal">decimal</option>
        </select>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox" ${index === 0 ? "checked" : ""}/>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox" ${col.isFk ? "checked" : ""}/>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox"/>
      </td>

      <td>
        <select class="table-select">
          <option value="0">None</option>
          <option value="1">Delete</option>
          <option value="2">Update</option>
          <option value="3">Both</option>
        </select>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox"/>
      </td>

      <td>
        <input type="text" class="table-input" value="${col.refTable}" />
      </td>

      <td>
        <input type="text" class="table-input" value="${col.refColumn}" />
      </td>

      <td class="table-action-cell">
        <button type="button" class="btn-row-delete">✕</button>
      </td>
    `;

      tbody.appendChild(row);

      row.querySelector("select").value = col.type;
    });
    CountRowDTO--;
    summaryRowDTO.textContent = CountRowDTO || "0";
  });

function parseViewMetadata(text) {
  if (!text) return [];

  const parts = text.split("&&");
  if (parts.length !== 2) return [];

  const columns = parts[0].split(",").map((x) => x.trim());
  const types = parts[1].split(",").map((x) => x.trim());

  const result = [];

  for (let i = 0; i < columns.length; i++) {
    result.push({
      name: columns[i],
      type: types[i] || "string",
    });
  }

  return result;
}

document
  .getElementById("btnFillViewDtoTable")
  .addEventListener("click", function () {
    const text = document.getElementById("viewDtoAutoFillInput").value;
    const parsed = parseViewMetadata(text);
    const tbody = document.getElementById("viewDtoTableBodyUnique");

    tbody.innerHTML = "";

    parsed.forEach((col, index) => {
      const row = document.createElement("tr");

      CountRowDTOView++;
      row.innerHTML = `
      <td><input type="text" class="table-input" value="${col.name}" /></td>

      <td>
        <select class="table-select">
          <option value="int">int</option>
          <option value="long">long</option>
          <option value="string">string</option>
          <option value="DateTime">DateTime</option>
          <option value="bool">bool</option>
          <option value="decimal">decimal</option>
        </select>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox" ${index === 0 ? "checked" : ""}/>
      </td>

      <td class="table-checkbox-cell">
        <input type="checkbox" class="table-checkbox"/>
      </td>

      <td>
        <select class="table-select">
          <option value="0">None</option>
          <option value="1">GetAll</option>
          <option value="2">GetOne</option>
          <option value="3">Both</option>
        </select>
      </td>

      <td class="table-action-cell">
        <button type="button" class="btn-row-delete">✕</button>
      </td>
    `;

      tbody.appendChild(row);

      row.querySelector("select").value = col.type;
    });
    CountRowDTOView--;
    summaryRowDTOView.textContent = CountRowDTOView || "0";
  });
