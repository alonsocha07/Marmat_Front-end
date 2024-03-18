$(document).ready(function () {
    $('#TablaMantenimiento').DataTable(
        {
            "lengthMenu": [[5, 10, 15, -1], [5, 10, 15, "Todas"]],
            "language": {
                "search": "Buscar:",
                "info": "Mostrándo página _PAGE_ de _PAGES_",
                "lengthMenu": "Mostrar _MENU_ líneas por página",
                "zeroRecords": "No existen resultados",
                "infoFiltered": "(filtrado de _MAX_ cantidad de líneas)",
                "infoEmpty": "No existe ningúna línea en esta tabla",
                "paginate": {
                    "first": "Primera",
                    "last": "Última",
                    "next": "Siguiente",
                    "previous": "Anterior"
                }
            }
        });
});