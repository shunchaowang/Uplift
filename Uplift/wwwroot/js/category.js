var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/admin/category/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {"data": "name", "width": "50%"},
            {"data": "displayOrder", "gwidth": "20%"},
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    Edit
                                </a>
                                &nbsp;
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    Delete
                                </a>
                            </div>
                            `;
                },
                "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No record found."
        },
        "width": "100%"
    });
}