var dtble;
$(document).ready(function () {
    loaddata();
});

function loaddata() {
    dtble = $("#mytable").DataTable({
        "ajax": {
            "url":"/Admin/Product/GetData"
        },
        "columns": [
            { "data": "name" },
            { "data": "description"},
            { "data": "price" },
            { "data": "category.name" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <a href="/Admin/Product/Edit/${data}" class="btn btn-success">Edit</a>
                            <a onClick="confirmDelete(${data})" class="btn btn-danger">Delete</a>
                            `
                    
                }

                }

            

        ]
    });
}

//function DeleteItem(url) {
//    Swal.fire({
//        title: 'Are you sure?',
//        text: "You won't be able to revert this!",
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Yes, delete it!'
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                type: "Delete",
//                url: url,
//                success: function (data) {
//                    if (data.success) {
//                        toastr.success(data.message);
//                        dtble.ajax.reload();
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            })
//        }
//    })
//}


function confirmDelete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            // If user confirms, send the delete request to the server
            deleteItem(id);
        }
    });
}
function deleteItem(id) {
    // Send AJAX request to the controller
    $.ajax({
        url: '/Admin/Product/DeleteProduct/' + id,
        type: 'POST',
        data: { id: id, "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
        success: function (response) {
            // If delete was successful
            if (response.success) {
                Swal.fire(
                    'Deleted!',
                    response.message || 'Your item has been deleted.',
                    'success'
                ).then(() => {
                    // Optional: Reload the page or update the UI
                    location.reload();
                    // Or remove the deleted item from DOM
                    // $("#item-" + id).remove();
                });
            } else {
                // If there was an error
                Swal.fire(
                    'Error!',
                    response.message || 'There was an error deleting the item.',
                    'error'
                );
            }
        },
        error: function () {
            Swal.fire(
                'Error!',
                'There was an error processing your request.',
                'error'
            );
        }
    });
}