$(document).on('ready', function () {
    var $eNav = $('#navChats');
    var $eConversations = $('#divChatConversations');
    var $aUser = $('#aUserName');
    var $eMessage = $('#taMessage');

    var $dChatWith = $('#divChatWith');
    var $dChatGeneral = $('#divChatGeneral');
    var $dWhoAreYou = $('#divWhoAreYou');

    $('#btnIn').on('click', function () {
        var name = $('#inpYouNikename').val();
        var uri = 'ws://' + window.location.host + '/Chat?Name=' + name;

        $('#inpYouNikename,#btnIn').attr('disabled', 'disabled');
        $aUser.html(name);
        $aUser.attr('data-username', name);

        socket = new WebSocket(uri);

        socket.onopen = function () {
            console.log('Ready to chat');
        };

        socket.onmessage = function (event) {
            var result = JSON.parse(event.data);
            var name = result.Sender;
            var message = result.Message;

            $eConversations.find("[data-username='" + name + "']").append('<div class="alert alert-info cc1 ccAnswer" data-sender"' + name + '">' + message + '</div>');
        };

        $('#divWhoAreYou').hide();
        $('#divChatWith').show();
        $eNav.find('.active').removeClass('active');
        $eNav.find("[data-username='with']").parent().addClass('active');
        $('#taMessage,#btnSend').removeAttr('disabled');
    });

    $('#btnSend').on('click', function () {
        var dataUserName = $eNav.find('.active').children().attr('data-username');

        socket.send(JSON.stringify({
            Receiver: dataUserName,
            Message: $eMessage.val()
        }));

        $eConversations.find("[data-username='" + dataUserName + "']").append('<div class="alert alert-success cc1 ccSend" data-receiver"' + dataUserName + '">' + $eMessage.val() + '</div>');

        $eMessage.val('');
    });

    $('#btnChatWith').on('click', function () {
        var $eReceiver = $('#inpOtherNikename');

        $('#divChatWith').hide();
        $eNav.find('.active').removeClass('active');
        $eNav.append('<li class="active"><a class="aMenu" href="#" data-username="' + $eReceiver.val() + '">' + $eReceiver.val() + '</a></li>');
        $eConversations.find('[data-username]').hide();
        $eConversations.append('<div data-username="' + $eReceiver.val() + '" class="cc2"></div>');
        $('#divChatGeneral').show();
        $eReceiver.val('');

    });

    $('#navChats').on('click', 'a', function () {
        var $eReceiver = $(this);
        var dataUserName = $eReceiver.attr('data-username');

        switch (dataUserName) {
            case 'who':
                $dChatWith.hide();
                $dChatGeneral.hide();
                $dWhoAreYou.show();
                break;
            case 'with':
                $dWhoAreYou.hide();
                $dChatGeneral.hide();
                $dChatWith.show();
                break;
            default:
                $dWhoAreYou.hide();
                $dChatWith.hide();

                $dChatGeneral.show();
                break;
        }

        $eNav.find('.active').removeClass('active');
        $eReceiver.parent().addClass('active');
        $eConversations.find('[data-username]').hide();
        $eConversations.find("[data-username='" + $eReceiver.attr('data-username') + "']").show();
    });


});