let productosTable;
let proveedores = [];
let tipoProductoMap = {};

function inicializarPagina() {
    if (!$.fn.DataTable.isDataTable('#productosTable')) {
        productosTable = $('#productosTable').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.25/i18n/Spanish.json'
            }
        });
    }
    cargarTiposProducto();
    buscarProductos();
}



async function cargarTiposProducto() {
    try {
        const response = await fetch('/api/TipoProducto/GetAll');
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const tipos = await response.json();

        // Crear el mapa de tipos de producto
        tipoProductoMap = tipos.reduce((map, tipo) => {
            map[tipo.id] = tipo.nombre;
            return map;
        }, {});

        const selectBusqueda = document.getElementById('tipoProductoId');
        const selectModal = document.getElementById('tipoProducto');

        // Limpiar las opciones existentes
        selectBusqueda.innerHTML = '';
        selectModal.innerHTML = '';

        // Agregar una opción en blanco
        const opcionBlanca = document.createElement('option');
        opcionBlanca.value = '';
        opcionBlanca.textContent = 'Seleccione un tipo de producto';
        selectBusqueda.appendChild(opcionBlanca.cloneNode(true));
        selectModal.appendChild(opcionBlanca);

        // Agregar opciones nuevas
        tipos.forEach(tipo => {
            const option = document.createElement('option');
            option.value = tipo.id;
            option.textContent = tipo.nombre;

            selectBusqueda.appendChild(option.cloneNode(true));
            selectModal.appendChild(option);
        });
    } catch (error) {
        console.error('Error al cargar los tipos de producto:', error);
    }
}

// Llama a la función cuando el DOM esté completamente cargado
document.addEventListener('DOMContentLoaded', cargarTiposProducto);



function buscarProductos() {
    const clave = $('#clave').val();
    const tipoProductoId = $('#tipoProductoId').val();

    $.ajax({
        url: '/ProductosView/Buscar',
        type: 'GET',
        data: { clave: clave, tipoProductoId: tipoProductoId },
        success: function (data) {
            actualizarTablaProductos(data);
        },
        error: function () {
            Swal.fire('Error', 'Error al buscar productos.', 'error');
        }
    });
}

function actualizarTablaProductos(productos) {
    productosTable.clear();

    productos.forEach(function (producto) {
        productosTable.row.add([
            producto.clave,
            producto.nombre,
            tipoProductoMap[producto.tipoProductoId] || 'Desconocido',
            producto.esActivo ? 'Sí' : 'No',
            producto.precio ? '$' + producto.precio.toFixed(2) : 'N/A',
            `<button class="btn btn-sm btn-primary" onclick="showEditModal(${producto.id})">Editar</button>
             <button class="btn btn-sm btn-danger" onclick="deleteProducto(${producto.id})">Eliminar</button>`
        ]);
    });

    productosTable.draw();
}
function showAddModal() {
    document.getElementById('productoForm').reset();
    document.getElementById('productoId').value = '';
    document.getElementById('proveedoresTableBody').innerHTML = '';
    $('#productoModalLabel').text('Agregar Producto');
    cargarProveedores();
    $('#productoModal').modal('show');
}

function showEditModal(id) {
    $.ajax({
        url: `/api/Productos/${id}`,
        type: 'GET',
        success: function (producto) {
            $('#productoId').val(producto.id);
            $('#txtClave').val(producto.clave);
            $('#nombre').val(producto.nombre);
            $('#tipoProducto').val(producto.tipoProductoId);
            $('#esActivo').prop('checked', producto.esActivo);
            $('#precio').val(producto.precio);

            const proveedoresTableBody = document.getElementById('proveedoresTableBody');
            proveedoresTableBody.innerHTML = '';
            producto.proveedores.forEach(proveedor => {
                agregarFilaProveedor(proveedor);
            });

            $('#productoModalLabel').text('Editar Producto');
            cargarProveedores();
           
            $('#productoModal').modal('show');
        },
        error: function () {
            Swal.fire('Error', 'No se pudo cargar el producto', 'error');
        }
    });
}
async function cargarProveedores() {
    try {
        const response = await fetch('/api/Proveedor/getAll');
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }
        const proveedores = await response.json();
        const select = document.getElementById('nuevoProveedorId');
        const select2 = document.getElementById('editNombreProveedor');

        // Limpia los comboboxes
        select.innerHTML = '<option value="">Seleccione un proveedor</option>';
        select2.innerHTML = '<option value="">Seleccione un proveedor</option>';

        
        console.log('Proveedores cargados:', proveedores);

        // Agrega las opciones a ambos comboboxes
        proveedores.forEach(proveedor => {
            const option = new Option(proveedor.nombre, proveedor.id);
            select.add(option);
            select2.add(option.cloneNode(true)); // Clona la opción para agregarla al segundo combobox
        });
    } catch (error) {
        console.error('Error al cargar los proveedores:', error);
    }
}

// Llama a la función para cargar los proveedores cuando se necesite
cargarProveedores();




function agregarProveedor() {
    const proveedorId = $('#nuevoProveedorId').val();
    const proveedorNombre = $('#nuevoProveedorId option:selected').text();
    const claveProducto = $('#nuevoClaveProducto').val();
    const costo = $('#nuevoCosto').val();

    if (!proveedorId || !claveProducto || !costo) {
        Swal.fire('Error', 'Por favor, complete todos los campos del proveedor.', 'error');
        return;
    }

    const nuevoProveedor = {
        proveedorId: parseInt(proveedorId),
        nombreProveedor: proveedorNombre,
        claveProducto: claveProducto,
        costo: parseFloat(costo)
    };

    agregarFilaProveedor(nuevoProveedor);

    $('#nuevoProveedorId').val('');
    $('#nuevoClaveProducto').val('');
    $('#nuevoCosto').val('');
}

function agregarFilaProveedor(proveedor) {
    const tableBody = document.getElementById('proveedoresTableBody');
    const newRow = tableBody.insertRow();
    newRow.innerHTML = `
        <td>${proveedor.nombreProveedor}</td>
        <td>${proveedor.claveProducto}</td>
        <td>${proveedor.costo}</td>
        <td>
            <button type="button" class="btn btn-sm btn-warning" onclick="editarFilaProveedor(this)">Editar</button>
            <button type="button" class="btn btn-sm btn-danger" onclick="eliminarFilaProveedor(this)">Eliminar</button>
        </td>
    `;
    newRow.setAttribute('data-proveedor-id', proveedor.proveedorId);
}

function editarFilaProveedor(button) {
    const row = button.parentElement.parentElement;
    const proveedorId = row.getAttribute('data-proveedor-id');

    // Obtén los valores actuales
    const nombreProveedor = row.cells[0].textContent;
    const claveProducto = row.cells[1].textContent;
    const costo = row.cells[2].textContent;

    // Llena el formulario de edición con los valores actuales
    document.getElementById('editProveedorId').value = proveedorId;
    document.getElementById('editNombreProveedor').value = nombreProveedor;
    document.getElementById('editClaveProducto').value = claveProducto;
    document.getElementById('editCosto').value = costo;

    // Muestra el modal de edición
    $('#editProveedorModal').modal('show');
}
function actualizarFilaProveedor() {
    const proveedorId = document.getElementById('editProveedorId').value;
    const nombreProveedor = $('#editNombreProveedor option:selected').text();
    const claveProducto = document.getElementById('editClaveProducto').value;
    const costo = document.getElementById('editCosto').value;

    const rows = document.querySelectorAll('#proveedoresTableBody tr');
    for (const row of rows) {
        if (row.getAttribute('data-proveedor-id') === proveedorId) {
            row.cells[0].textContent = nombreProveedor;
            row.cells[1].textContent = claveProducto;
            row.cells[2].textContent = costo;
            break;
        }
    }

    // Oculta el modal de edición
    $('#editProveedorModal').modal('hide');
}

function eliminarFilaProveedor(button) {
    const row = button.parentNode.parentNode;
    const proveedorId = row.getAttribute('data-proveedor-id');
    proveedores = proveedores.filter(p => p.id != proveedorId);
    row.remove();
    console.log('Proveedor eliminado:', proveedorId);
    console.log('Lista de proveedores actualizada:', proveedores);
}


function eliminarFilaProveedor(button) {
    button.closest('tr').remove();
}

function saveProducto() {
    const form = document.getElementById('productoForm');
    const formData = new FormData(form);
    const producto = Object.fromEntries(formData.entries());

    // Convertir los valores a los tipos correctos
    producto.id = producto.id ? parseInt(producto.id) : null;  // Convertir el ID a número o null
    producto.tipoProductoId = parseInt(producto.tipoProductoId);
    producto.esActivo = producto.esActivo === 'on';
    producto.precio = producto.precio ? parseFloat(producto.precio) : null;

    // Obtener proveedores de la tabla
    const proveedoresRows = document.querySelectorAll('#proveedoresTableBody tr');
    producto.proveedores = Array.from(proveedoresRows).map(row => ({
        proveedorId: parseInt(row.getAttribute('data-proveedor-id')),
        claveProducto: row.cells[1].textContent,
        costo: parseFloat(row.cells[2].textContent),
        nombreProveedor: row.cells[0].textContent
    }));

    // Validar campos requeridos
    if (!producto.clave || !producto.nombre || !producto.tipoProductoId) {
        Swal.fire('Error', 'Por favor, complete todos los campos requeridos', 'error');
        return;
    }

    // Asegúrate de que el campo `Id` esté correctamente manejado
    if (producto.id === "" || producto.id === null) {
        delete producto.id;  // Elimina el campo si está vacío o es null
    }

    console.log('Datos del producto a enviar:', JSON.stringify(producto)); // Verifica los datos enviados

    // Enviar la solicitud AJAX
    $.ajax({
        url: '/api/Productos',
        type: producto.id ? 'PUT' : 'POST',
        contentType: 'application/json',
        data: JSON.stringify(producto),
        success: function (data) {
            $('#productoModal').modal('hide');
            buscarProductos();
            Swal.fire('Éxito', 'Producto guardado correctamente', 'success');
        },
        error: function (xhr, status, error) {
            console.error('Error response:', xhr.responseJSON);
            let errorMessage = 'Hubo un error al guardar el producto: ';
            if (xhr.responseJSON && xhr.responseJSON.errors) {
                if (Array.isArray(xhr.responseJSON.errors)) {
                    errorMessage += xhr.responseJSON.errors.join('\n');
                } else if (typeof xhr.responseJSON.errors === 'object') {
                    errorMessage += Object.values(xhr.responseJSON.errors).flat().join('\n');
                }
            } else if (xhr.responseText) {
                errorMessage += xhr.responseText;
            }
            Swal.fire('Error', errorMessage, 'error');
        }
    });
}




function deleteProducto(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "No podrás revertir esta acción",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/api/Productos/${id}`,
                type: 'DELETE',
                success: function () {
                    buscarProductos();
                    Swal.fire('Eliminado', 'El producto ha sido eliminado', 'success');
                },
                error: function () {
                    Swal.fire('Error', 'Hubo un error al eliminar el producto', 'error');
                }
            });
        }
    });
}
function cerrarModales() {
    $('.modal').modal('hide');
}

document.addEventListener('DOMContentLoaded', function () {
    inicializarPagina();
});
