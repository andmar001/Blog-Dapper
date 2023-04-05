var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblSliders").DataTable({
        "ajax": {
            "url": "/admin/sliders/GetSliders",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idSlider", "width": "10%" },
            { "data": "nombre", "width": "20%" },
            {
                "data": "urlImagen",
                "render": function (imagen) {
                    return `<img src="../${imagen}" width="100" height:"30px">`
                }, "width": "20%"
            },
            {
                "data": "idSlider",
                "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/admin/sliders/editar/${data}" class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                <i class="bi bi-pencil-square"></i> Editar
                                </a>                                
                                &nbsp;
                                <a onclick=Delete("/admin/sliders/BorrarSlider/${data}") class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                    <i class="bi bi-x-square"></i> Borrar
                                </a>
                           `;
                }, "width": "50%"
            }
        ]


    });
}

function Delete(url) {
    swal({
        title: "Esta seguro de borrar?",
        text: "Este contenido no se puede recuperar!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Si, borrar!",
        closeOnconfirm: true
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
        });
    });
}
