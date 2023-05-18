var dtable;
$(document).ready
(
    function (url)
    {
        dtable = $('#myTable').DataTable
        ({
        "ajax": 
        {
            "url": "/Admin/Product/AllProducts"
        },
        

        "columns":
        [
            { data: "name" },
            { data: "description" },
            { data: "price" },
            { data: "category.name" },
                {
                    data: "id",
                    render: function (data) {
                        return `<a href="/Admin/Product/CreateUpdate?id=${data}"><i class="btn btn-info bi bi-pen"></i></a>
                        <a onclick="RemoveProduct('/Admin/Product/Delete/${data}')"><i class=" btn btn-danger bi bi-trash-fill"></i></a>`
                    }
                }
        ]
        })
    }
);
function RemoveProduct(url)
{
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success:function(data) {
                    if (data.success) {
                        dtable.ajax.reload();
                        toastr.success(data.message)
                    }
                    else {
                        toastr.error(data.message)
                    }
                },
                error: function (xhr,status, error) {
                    toastr.error(error);
                }
            }) 
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Your imaginary file is safe :)',
                'error'
            )
        }
    })
    
}