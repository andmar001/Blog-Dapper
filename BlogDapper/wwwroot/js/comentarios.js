var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable(){
    dataTable = $("#tblComentarios").DataTable({
        "ajax": {
            "url": "/admin/comentarios/GetComentarios",
            "type": "GET",
            "datatype":"json"
        },
        "columns": [
            { "data":"idComentario", "width":"5%" },
            { "data":"titulo", "width":"20%" },
            { "data":"mensaje", "width":"20%" },
            { "data": "articulo.titulo", "width": "20%" },
            { "data": "fechaCreacion", "width": "20%" },
            {
                "data": "idCategoria",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a onclick=Delete("/admin/comentarios/BorrarComentario/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px">
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
