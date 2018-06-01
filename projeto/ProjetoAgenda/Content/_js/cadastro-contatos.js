(function ($, container) {
    //"use strict";
    var $container = $(container);

    var inicializar = function () {
        $container
            .on('change', '#ddlPessoas', function (e) {
                var idPessoa = $(this).val();
                if (idPessoa != "0") {
                    metodos.obterformularioedicaousuario(idPessoa);
                } else {
                    $('#formulario-cadastro').hide();
                }
            })
            .on('click', '#adicionarPessoa', function (e) {
                metodos.modalnovapessoa();
            })
            .on('click', '#salvar-pessoa', function (e) {
                metodos.salvarpessoa();
            })
            .on('click', '#salvar-usuario', function (e) {
                debugger;
                $.validator.unobtrusive.parseDynamicContent($('form'));
                $("form").validate();

                if ($("form").valid()) {
                    $("form").submit();
                } else {
                }
            });
    };


    var metodos = {
        modalnovapessoa: function () {
            $.ajax
               ({
                   type: "POST",
                   //data:{},
                   url: CaminhoRelativo() + "/Usuario/CriarNovaPessoa",
                   beforeSend: function () {
                       //  ui.exibirLoadingModal();
                   },
                   success: function (retorno) {
                       $('#triggerModalNovaPessoa').html(retorno);
                       Modal('modalNovaPessoa');
                   },
                   complete: function () {
                       //ui.ocultarLoadingModal();
                   }
               });
        },
        salvarpessoa: function () {
            var nome = $('#nomePessoa').val();
            $.ajax
               ({
                   type: "POST",
                   data: { nome: nome },
                   url: CaminhoRelativo() + "/Usuario/SalvarNovaPessoa",
                   beforeSend: function () {
                       //  ui.exibirLoadingModal();
                   },
                   success: function (retorno) {
                       if (retorno == "ok") {
                           AlertarcomCallback("Sucesso!", function () {
                               //Atualizar DDL
                           });
                       } else {
                           AlertarcomCallback("Erro ao salvar: " + retorno, function () {
                               //Atualizar DDL
                           });
                       }
                   },
                   complete: function () {
                       //ui.ocultarLoadingModal();
                   },
                   error: function (msg) {
                       AlertarcomCallback("Erro ao salvar: " + msg, function () {
                           //Atualizar DDL
                       });
                   }
               });
        },
        obterformularioedicaousuario: function (idpessoa) {
            $.ajax
               ({
                   type: "POST",
                   data: { idpessoa: idpessoa },
                   url: CaminhoRelativo() + "/Usuario/ObterFormularioEdicaoUsuario",
                   success: function (retorno) {
                       $('#formulario-cadastro').html(retorno);
                       $('#formulario-cadastro').show();
                   },
                   error: function (msg) {
                       Alertar("Erro ao obter pessoa. Tente mais tarde.");
                   }
               });
        },
        cadastrarusuariosucesso: function () {
            AlertarcomCallback('Usuario Salvo com Sucesso!', function () {
                window.reload();
            });
        }
    };

    //ready
    $(function () {
        inicializar();
    });


})(jQuery, document);