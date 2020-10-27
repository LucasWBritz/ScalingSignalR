$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
    connection.start().then(() => {
        bindHandlers();
        AddMessage("Connected to signalR server.");
    })
    .catch(error => console.error(error.message));    

    function bindHandlers() {
        connection.on("MessageReceived", function (message) {
            AddIncomingMessage(message);
        });

        $('#btn_send_message').click(function () {
            var messageToSend = $('#message_to_send').val();
            connection.invoke("broadcastMessage", messageToSend).catch(function (err) {
                return console.error(err.toString());
            });
            AddMessage(messageToSend);
            $('#message_to_send').val("");
        });
    }    

    function AddMessage(message) {
        $('#message_area').append(`
            <div class="outgoing_msg">
                <div class="sent_msg">
                    <p>
                        ${message}
                        </p>
                    <span class="time_date"> Now </span>
                </div>
            </div>`);
    }

    function AddIncomingMessage(message) {
        $('#message_area').append(`
            <div class="incoming_msg">
                    <div class="received_msg">
                        <div class="received_withd_msg">
                            <p>
                                ${message}
                            </p>
                            <span class="time_date"> Now </span>
                        </div>
                    </div>
                </div>`);
    }
});