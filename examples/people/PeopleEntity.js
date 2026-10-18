export const peopleEntity = {
    entity: "Peoples",
    operations: {
search: {
    endpoint: "searchPeople",
    filters: [
        { name: "Name", label: "Name", type: "string", },
],
    columns: [
        { field: "id", header: "ID" },
        { field: "name", header: "Name" },
        { field: "phoneNumber", header: "PhoneNumber" },
        { field: "address", header: "Address" },
        { field: "gender", header: "Gender" },
        { field: "age", header: "Age" },
        { field: "email", header: "Email" },
    ],
},
getAll: {
    endpoint: "all",
    columns: [
        { field: "id", header: "ID" },
        { field: "name", header: "Name" },
        { field: "phoneNumber", header: "PhoneNumber" },
        { field: "address", header: "Address" },
        { field: "gender", header: "Gender" },
        { field: "age", header: "Age" },
        { field: "email", header: "Email" },
    ],
},
add: {
    endpoint: "",
    fields: [
        { name: "name", label: "Name", type: "text" },
        { name: "phoneNumber", label: "PhoneNumber", type: "text" },
        { name: "address", label: "Address", type: "text" },
        { name: "gender", label: "Gender", type: "text" },
        { name: "age", label: "Age", type: "number" },
        { name: "email", label: "Email", type: "text" },
    ]
},
update: {
    endpoint: "{value}",
    by: "id",
    fields: [
        { name: "name", label: "Name", type: "text" },
        { name: "phoneNumber", label: "PhoneNumber", type: "text" },
        { name: "address", label: "Address", type: "text" },
        { name: "gender", label: "Gender", type: "text" },
        { name: "age", label: "Age", type: "number" },
    ]
},
delete: {
    endpoint: "{value}",
    by: "id"
},
    },
};
