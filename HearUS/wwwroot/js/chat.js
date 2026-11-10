(function () {
    "use strict";

    const thread = document.getElementById("chatThread");
    if (!thread || typeof signalR === "undefined") {
        return;
    }

    const conversationId = Number(thread.dataset.conversationId);
    const currentUserId = thread.dataset.currentUserId;
    const form = document.getElementById("chatForm");
    const input = document.getElementById("chatInput");
    const status = document.getElementById("chatStatus");
    const emptyHint = document.getElementById("chatEmptyHint");

    function scrollToBottom() {
        thread.scrollTop = thread.scrollHeight;
    }

    function appendMessage(message, isOwn) {
        if (emptyHint) {
            emptyHint.remove();
        }

        const wrapper = document.createElement("div");
        wrapper.className = "d-flex mb-2 " + (isOwn ? "justify-content-start" : "justify-content-end");

        const bubble = document.createElement("div");
        bubble.className = "chat-bubble " + (isOwn ? "chat-bubble-own" : "chat-bubble-other");

        const name = document.createElement("div");
        name.className = "small fw-semibold mb-1";
        name.textContent = message.senderName || "";

        const body = document.createElement("div");
        body.textContent = message.content || "";

        const time = document.createElement("div");
        time.className = "small opacity-75 mt-1 text-end";
        time.textContent = new Date(message.sentAt).toLocaleTimeString("en-GB", {
            hour: "2-digit",
            minute: "2-digit",
            hour12: false
        });

        bubble.append(name, body, time);
        wrapper.appendChild(bubble);
        thread.appendChild(wrapper);
        scrollToBottom();
    }

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .withAutomaticReconnect()
        .build();

    connection.on("ReceiveMessage", function (message) {
        appendMessage(message, message.senderId === currentUserId);
        if (message.senderId === currentUserId && input) {
            input.value = "";
        }
    });

    connection.onreconnecting(function () {
        if (status) {
            status.textContent = "جارٍ إعادة الاتصال...";
        }
    });

    connection.onreconnected(function () {
        if (status) {
            status.textContent = "متصل — الرسائل تظهر لحظياً";
        }
        connection.invoke("JoinConversation", conversationId).catch(function () { });
    });

    connection.onclose(function () {
        if (status) {
            status.textContent = "الدردشة اللحظية غير متصلة — سيتم الإرسال عبر النموذج التقليدي";
        }
    });

    if (form) {
        form.addEventListener("submit", function (event) {
            const value = input ? input.value.trim() : "";
            if (!value) {
                event.preventDefault();
                return;
            }

            if (connection.state === signalR.HubConnectionState.Connected) {
                event.preventDefault();
                input.value = "";
                connection.invoke("SendMessage", conversationId, value).catch(function () {
                    input.value = value;
                    form.submit();
                });
            }
        });
    }

    connection.start()
        .then(function () {
            return connection.invoke("JoinConversation", conversationId);
        })
        .then(function () {
            if (status) {
                status.textContent = "متصل — الرسائل تظهر لحظياً";
            }
        })
        .catch(function () {
            if (status) {
                status.textContent = "الدردشة اللحظية غير متاحة حالياً؛ يمكنك الإرسال عبر النموذج.";
            }
        });

    scrollToBottom();
})();
