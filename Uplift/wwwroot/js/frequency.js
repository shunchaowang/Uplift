var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {

    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Frequency/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "count", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href = "/Admin/Frequency/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    Edit
                                </a>
                                &nbsp;
                                <a onclick = remove("/Admin/Frequency/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer; width: 100px;">
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
