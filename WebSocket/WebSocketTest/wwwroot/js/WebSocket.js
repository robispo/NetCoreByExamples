$(document).on('ready', function () {
    $('#btnIn').on('click', function () {
        var name = $('#inpYouNikename').val();
        var uri = 'ws://' + window.location.host + '/Chat?Name=' + name;

        $('#inpYouNikename,#inpOtherNikename,#btnIn').attr('disabled', 'disabled');

        socket = new WebSocket(uri);
        $('#aUserName').val(name);

        socket.onopen = function () {
            $('#lready').html('Ready to chat');
        };

        socket.onmessage = function (event) {
            var result = JSON.parse(event.data);
            var name = result.Sender;
            var message = result.Message;

            $('#divchatConversation').append('<div class="alert alert-info cc1 ccAnswer" data-sender"' + name + '">' + message + '</div>');
        };

        $('#taMessage,#btnSend').removeAttr('disabled');
    });

    $('#btnSend').on('click', function () {
        var $eMessage = $('#taMessage');
        var $eReceiver = $('#inpOtherNikename');

        socket.send(JSON.stringify({
            Receiver: $eReceiver.val(),
            Message: $eMessage.val()
        }));

        $('#divchatConversation').append('<div class="alert alert-success cc1 ccSend" data-receiver"' + $eReceiver.val() + '">' + $eMessage.val() + '</div>');

        $eMessage.val('');
    });
});