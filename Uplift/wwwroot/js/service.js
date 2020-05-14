var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Service/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "category.name", "width": "20%" },
            { "data": "price", "width": "15%" },
            { "data": "frequency.count", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Service/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    Edit
                                </a>
                                &nbsp;
                                <a onclick=remove("/Admin/Service/Delete/${data}") class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
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

function remove(url) {
    swal({
        title: 'Are you sure you want to delete?',
        text: 'You will not be able to restore the content!',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Yes, delete it!',
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    showMessage(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        });
    });
}

function showMessage(msg) {
    toastr.success(msg);
}
