var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable(){
    dataTable = $("#tblCategorias").DataTable({
        "ajax": {
            "url": "/admin/categorias/GetCategorias",
            "type": "GET",
            "datatype":"json"
        },
        "columns": [
            { "data":"idCategoria", "width":"5%" },
            { "data":"nombre", "width":"40%" },
            { "data": "fechaCreacion", "width": "20%" },
            {
                "data": "idCategoria",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/admin/categorias/editar/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px">
                            <i class="bi bi-pencil-square" style="margin-right:5px"></i>Editar
                            </a>
                            &nbsp;
                            <a onclick=Delete("/admin/categorias/BorrarCategoria/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                            <i class="bi bi-x-square" style="margin-right:5px"></i>Borrar
                            </a>
                        </div>
                    `;
                }
            }
        ]
    });
}

function Delete(url){
    swal({
        title: 'Esta seguro de borrar?',
        text: "Este contenido no se puede recuperar!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DD6B55',
        confirmButtonText: 'Si, borrar!',
        closeOnConfirm: true
    }, function () {
        $.ajax({
            type: 'DELETE',
            url: url,
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();
                }
                else {
                    toastr.error(data.message);
                }
            }
        })
    })
}
