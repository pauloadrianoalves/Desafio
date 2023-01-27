//Carrega lista de clientes dinamicamente
function CarregarLista() {
    $('#table_cliente').DataTable({
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        orderMulti: false,
        bInfo: false,
        bLengthChange: false,
        stateSave: true,
        bDestroy: true,
        dom: 'ftipr',
        sDom: 'rt',
        language: {
            url: '/lib/datatables.net/js/pt-BR.json'
        },
        ajax: {
            url: '/clientes/carregar',
            type: 'POST',
            datatype: 'json'
        },
        columnDefs:
            [{
                targets: [0],
                searchable: false
            },
                { targets: 'sort-disable', orderable: false },
            ],
        aoColumns: [
            { data: 'nome', 'name': 'nome', 'autoWidth': true, 'class': 'text-center align-middle text-small' },
            { data: 'rua', 'name': 'rua', 'autoWidth': true, 'class': 'text-center align-middle' },
            { data: 'numero', 'name': 'numero', 'autoWidth': true, 'class': 'text-center align-middle' },
            { data: 'bairro', 'name': 'bairro', 'autoWidth': true, 'class': 'text-center align-middle' },
            { data: 'cidade', 'name': 'cidade', 'autoWidth': true, 'class': 'text-center align-middle' },
            { data: 'uf', 'name': 'uf', 'autoWidth': true, 'class': 'text-center align-middle' },
            {
                data: null, render: function (data, type, row) {
                    return '<a class="btn btn-sm btn-none-primary border-0" onclick="Editar(\'' + data.codigo + '\')" title="Alterar cliente"><i class="fa fa-pencil fs-15"></i></a>';
                }, 'class': 'text-center align-middle'
            },
            {
                data: null, render: function (data, type, row) {
                    return '<a class="btn btn-sm btn-none-danger border-0" onclick="Excluir(\'' + data.codigo + '\')" title="Excluir cliente"><i class="fa fa-trash fs-15"></i></a>';
                }, 'class': 'text-center align-middle'
            }
        ]
    });
}

//Ficha de cadastro
function Ficha(fch) {
    $('#Codigo').val(fch.codigo);
    $('#Nome').val(fch.nome);
    $('#Rua').val(fch.rua);
    $('#Numero').val(fch.numero);
    $('#Bairro').val(fch.bairro);
    $('#Cidade').val(fch.cidade);
    $('#Uf').val(fch.uf);
    $('#modal_ficha').modal('show');
}

//Altera cadastro
function Editar(id) {
    $('#loaderbody').removeClass('hide');
    var param = { id: id }
    $.post('/clientes/editar', param, function (response) {
        if (response.status) {
            $('#lbl_titulo').text('Editar cliente');
            Ficha(response.msg);
            $('#loaderbody').addClass('hide');
        }
        else {
            $('#loaderbody').addClass('hide');
            Swal.fire('Atenção', response.msg, 'info');
        }
    });
}

//Excluir cadastro
function Excluir(id) {
    Swal.fire({
        title: 'Atenção',
        text: 'Confirmar exclusão do cliente?',
        icon: 'question',
        showCancelButton: true,
        cancelButtonText: '<i class="fa fa-times-circle"></i> Cancelar',
        cancelButtonColor: '#90A4AE',
        confirmButtonColor: '#E53935',
        confirmButtonText: '<i class="fa fa-check"></i> Confirmar'
    }).then((result) => {
        if (result.isConfirmed) {
            $('#loaderbody').removeClass('hide');
            var param = { id: id }
            $.post('/clientes/excluir', param, function (response) {
                if (response.status) {
                    CarregarLista();
                    $('#loaderbody').addClass('hide');
                    Swal.fire('Sucesso!', 'Cliente excluído com sucesso', 'success');
                }
                else {
                    $('#loaderbody').addClass('hide');
                    Swal.fire('Atenção!', response.msg, 'info');
                }
            });
        }
    });
}

$(document).ready(function () {
    CarregarLista();
});

//Cadastrar novo
$(document).on('click', '#btn_cliente_novo', function () {
    $('#lbl_titulo').text('Cadastrar novo cliente');
    Ficha({ codigo: '0', nome: '', rua: '', numero: '', bairro: '', cidade: '', uf: '' })
});

//Salvar cliente
$(document).on('click', '#btn_cliente_confirmar', function () {
    $('#loaderbody').removeClass('hide');
    var param =
    {
        codigo: $('#Codigo').val(),
        nome: $('#Nome').val(),
        rua: $('#Rua').val(),
        numero: $('#Numero').val(),
        bairro: $('#Bairro').val(),
        cidade: $('#Cidade').val(),
        uf: $('#Uf').val()
    };
    $.post('/clientes/salvar', param, function (response) {
        if (response.status) {
            CarregarLista();
            $('#loaderbody').addClass('hide');
            $('#modal_ficha').modal('hide');
            Swal.fire('Sucesso!', 'Cliente salvo com sucesso', 'success');
        }
        else {
            $('#loaderbody').addClass('hide');
            Swal.fire('Atenção!', response.msg, 'info');
        }
    });
});