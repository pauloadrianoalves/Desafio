create table tb_cliente (
  codigo serial,
  nome text not null,
  rua text not null,
  numero text not null,
  bairro text not null,
  cidade text not null,
  uf text not null,
  dtcad timestamp not null,
  primary key (codigo)
);
