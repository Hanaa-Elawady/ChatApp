
document.addEventListener('DOMContentLoaded', () => {
    // DOM Elements
    const chatMessages = document.getElementById("chat-messages");
    const messageInput = document.getElementById("messageInp");
    const sendButton = document.getElementById("send-btn");
    const audioSendButton = document.getElementById("send-audio-btn");
    const startRecordingButton = document.getElementById("startRecording");
    const stopRecordingButton = document.getElementById("stopRecording");
    const deleteRecordButton = document.getElementById("delete");
    const audioPlayback = document.getElementById("audioPlayback");
    const capturePhotoButton = document.getElementById("capturePhoto");
    const cameraModal = document.getElementById("cameraModal");
    const cameraPreview = document.getElementById("cameraPreview");
    const closeCameraButton = document.getElementById("closeCamera");
    const takePhotoButton = document.getElementById("takePhoto");
    const photoPreviewContainer = document.getElementById("photoPreviewContainer");
    const photoPreview = document.getElementById("photoPreview");
    const captureVideoButton = document.getElementById("captureVideo");
    const videoModal = document.getElementById("videoModal");
    const videoPreview = document.getElementById("videoPreview");
    const closeVideoButton = document.getElementById("closeVideo");
    const startRecordingVideoButton = document.getElementById("startRecordingVideoButton");
    const stopRecordingVideoButton = document.getElementById("stopRecordingVideoButton");
    const videoPreviewContainer = document.getElementById("videoPreviewContainer");
    const capturedVideo = document.getElementById("capturedVideo");
    const attachButton = document.getElementById("attachButton");
    const attachmentInput = document.getElementById("attachmentInput");
    const AudioRecording = document.getElementById("AudioRecording");
    const timerElement = document.getElementById("timer");
    const waves = document.querySelectorAll(".wave");
    const cancelPhoto = document.getElementById("cancelPhoto");
    const cancelVideo = document.getElementById("cancelVideo");
    let seconds = 0;
    // Variables
    let audioBlob, mediaRecorder, audioChunks = [], isRecording = false, videoChunks = [];
    var messageType = "text";
    const reader = new FileReader();
    const connection = Window.ActiveContact;
    const userId = Window.userId;
    const videoConstraints = { video: true, audio: true };
    const PhotoConstraints = { video: true };
    let stream;
    var timeInterval;
    var wavesInterval;
    chatMessages.scrollTop = chatMessages.scrollHeight;


    // SignalR connection
    const proxyConnection = new signalR.HubConnectionBuilder()
        .withUrl("/chathub")
        .withAutomaticReconnect()
        .build();


    const initializeConnection = async () => {
        try {
            await proxyConnection.start();
            console.log("Connection started");
            setupEventListeners();
        } catch (error) {
            console.error("Connection error", error);
        }
    };

    //record 
    const handleRecordingStart = async () => {
        try {
            audioChunks = [];
            messageType = 'Audios'
            isRecording = true;
            const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.ondataavailable = (event) => audioChunks.push(event.data);
            mediaRecorder.start();
            timeInterval =setInterval(updateTimer, 1000);
            wavesInterval = setInterval(updateWaves, 150);
            toggleRecordingUI(true);

            audioPlayback.classList.add("d-none");

        } catch (error) {
            console.error("Error starting recording", error);
        }
    };

    const handleRecordingStop = () => {
        mediaRecorder.stop();
        mediaRecorder.onstop = () => {
            audioBlob = new Blob(audioChunks, { type: "audio/webm" });
            audioPlayback.src = URL.createObjectURL(audioBlob);
        };
        clearInterval(timeInterval);
        clearInterval(wavesInterval);
        seconds = 0;
        AudioRecording.classList.add("d-none");

        audioPlayback.classList.remove("d-none");
        stopRecordingButton.classList.add("d-none");

    };

    const handleRecordingDelete = () => {
        audioChunks = [];
        isRecording = false;
        messageType = 'text';
        toggleRecordingUI(false);
        clearInterval(timeInterval);
        clearInterval(wavesInterval);
        seconds = 0;
    };

    const updateTimer = () => {
        seconds++;
        const minutes = Math.floor(seconds / 60);
        const remainingSeconds = seconds % 60;
        timerElement.textContent = `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
    }

    const updateWaves = () => {
        waves.forEach(wave => {
            const randomHeight = Math.random() * 15 + 5;
            wave.style.height = `${randomHeight}px`;
        });
    }
    const toggleRecordingUI = (isRecording) => {
        startRecordingButton.classList.toggle("d-none", isRecording);
        AudioRecording.classList.toggle("d-none", !isRecording);
        audioPlayback.classList.toggle("d-none", !isRecording)
        stopRecordingButton.classList.toggle("d-none", !isRecording);
        deleteRecordButton.classList.toggle("d-none", !isRecording);
        messageInput.classList.toggle("d-none", isRecording);
        attachButton.classList.toggle("d-none", isRecording);
        captureVideoButton.classList.toggle("d-none", isRecording);
        capturePhotoButton.classList.toggle("d-none", isRecording);
    };

    //handle Attachment 
    const handleAttachment = (file) => {
        const fileType = file.type;
        const reader = new FileReader();
        reader.readAsDataURL(file);

        reader.onloadend = () => {
            const base64 = reader.result;
            messageType = getAttachmentType(fileType);
            invokeSendFunction('', base64, messageType);
            messageType = 'text';
        };
    };

    const getAttachmentType = (fileType) => {
        if (fileType.startsWith("image")) {
            return "Photos";
        } else if (fileType.startsWith("audio")) {
            return "Audios";
        } else if (fileType.startsWith("video")) {
            return "Videos";
        } else if (fileType === "application/pdf") {
            return "PDFs";
        } 
        return "Other"; // Default for unsupported file types
    };

    // Photo capture
    const startPhotoCapture = async () => {
        try {
            stream = await navigator.mediaDevices.getUserMedia(PhotoConstraints);
            cameraPreview.srcObject = stream;
            cameraModal.classList.remove("d-none");
        } catch (error) {
            alert("Camera access failed: " + error.message);
        }
    };

    const closeCamera = () => {
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
        }
        cameraModal.classList.add("d-none");
    };

    const capturePhoto = () => {
        messageType = "Photos";
        const canvas = document.createElement("canvas");
        const context = canvas.getContext("2d");
        canvas.width = cameraPreview.videoWidth;
        canvas.height = cameraPreview.videoHeight;
        context.drawImage(cameraPreview, 0, 0, canvas.width, canvas.height);
        photoPreview.src = canvas.toDataURL("png");
        photoPreviewContainer.classList.remove("d-none");
        closeCamera();
    };

    const deletePhoto = () => {
        messageType = "text";
        photoPreview.src = '';
        photoPreviewContainer.classList.add("d-none");
    }

    // Video capture
    const startVideoCapture = async () => {
        try {
            stream = await navigator.mediaDevices.getUserMedia(videoConstraints);
            videoPreview.muted = true;
            videoPreview.srcObject = stream;
            videoModal.classList.remove("d-none");
        } catch (error) {
            alert("Video access failed: " + error.message);
        }
    };

    const closeVideo = () => {
        messageType = "text";
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
        }
        videoModal.classList.add("d-none");
    };

    const startVideoRecording = () => {
        messageType = "Videos";
        videoChunks = [];
        mediaRecorder = new MediaRecorder(stream);
        mediaRecorder.ondataavailable = (event) => videoChunks.push(event.data);
        mediaRecorder.start();
        startRecordingVideoButton.disabled = true;
        stopRecordingVideoButton.disabled = false;
    };

    const stopVideoRecording = () => {
        if (mediaRecorder) {
            mediaRecorder.stop();
            mediaRecorder.onstop = () => {
                const videoBlob = new Blob(videoChunks, { type: "video/mp4" });
                const videoURL = URL.createObjectURL(videoBlob);

                capturedVideo.src = videoURL;
                videoPreviewContainer.classList.remove("d-none");
                if (stream) {
                    stream.getTracks().forEach(track => track.stop());
                }
            };
        }
        videoModal.classList.add("d-none");
        startRecordingVideoButton.disabled = false;
        stopRecordingVideoButton.disabled = true;
    };

    const deleteVideo =() => {
        messageType = "text";
        capturedVideo.src = '';
        videoPreviewContainer.classList.add("d-none");
    }

    //sending
    const sendMessage = async () => {
        if (messageType == 'text' && messageInput.value) {
            const textMessage = messageInput.value.trim();
            invokeSendFunction(textMessage, null, messageType);
        }
        else if (messageType == 'Audios') {
            const base64 = await getBase64Audio();
            invokeSendFunction('', base64, messageType);
            handleRecordingDelete();

        }
        else if (messageType == 'Photos') {
            const base64 = photoPreview.src;
            const textMessage = messageInput.value.trim();
            invokeSendFunction(textMessage, base64, messageType);
            photoPreviewContainer.classList.add("d-none");

        }
        else if (messageType == 'Videos') {
            const base64 = await getBase64Video();
            const textMessage = messageInput.value.trim();
            invokeSendFunction(textMessage, base64, messageType);
            closeVideo();
        }
        messageType = "text";

    };

    const invokeSendFunction = async (text , base64 , type) => {
        var messageObject = {
            Connection: connection,
            SenderId: userId,
            Seen: false,
            DateSent: new Date().toISOString(),
            Text: text,
            AttachmentType: type,
            AttachmentBase64: base64
        }
        try {
            await proxyConnection.invoke("Send", messageObject);
            messageInput.value = "";
        } catch (error) {
            console.error("Send message error", error);
        }
    }

    const getBase64Video = () => {
        return new Promise((resolve, reject) => {
            const videoBlob = new Blob(videoChunks, { type: "video/mp4" });
            const reader = new FileReader();
            reader.readAsDataURL(videoBlob);
            reader.onloadend = async () => {
                try {
                    videoPreviewContainer.classList.add("d-none");
                    resolve(reader.result);
                } catch (error) {
                    console.error("Send audio error", error);
                }
            }
        });
    };

    const getBase64Audio = () => {
        return new Promise((resolve, reject) => {
            if (mediaRecorder.state === 'recording') {
                mediaRecorder.stop();
                mediaRecorder.onstop = () => {
                    audioBlob = new Blob(audioChunks, { type: "audio/webm" });
                    reader.readAsDataURL(audioBlob);
                };
            } else {
                reader.readAsDataURL(audioBlob);
            }

            reader.onloadend = async () => {
                try {
                    resolve(reader.result);
                } catch (error) {
                    console.error("Error processing audio:", error);
                    reject(error);  // Reject the promise if an error occurs
                }
            };
        });
    };

    const setupEventListeners = () => {
        sendButton.addEventListener('click', (e) => {
            e.preventDefault();
            sendMessage();
        });

        startRecordingButton.addEventListener('click', handleRecordingStart);
        stopRecordingButton.addEventListener('click', handleRecordingStop);
        deleteRecordButton.addEventListener('click', handleRecordingDelete);
        capturePhotoButton.addEventListener("click", startPhotoCapture);
        closeCameraButton.addEventListener("click", closeCamera);
        takePhotoButton.addEventListener("click", capturePhoto);
        captureVideoButton.addEventListener("click", startVideoCapture);
        closeVideoButton.addEventListener("click", closeVideo);
        startRecordingVideoButton.addEventListener("click", startVideoRecording);
        stopRecordingVideoButton.addEventListener("click", stopVideoRecording);
        cancelPhoto.addEventListener("click", deletePhoto);
        cancelVideo.addEventListener("click", deleteVideo);

        attachButton.addEventListener('click', () => {
            attachmentInput.click();
        });

        attachmentInput.addEventListener('change', (e) => {
            const file = e.target.files[0];
            if (file) {
                handleAttachment(file);
            }
        });
    };

    const updateChatMessages = (msg) => {
        const isCurrentConnection = document.getElementById("chatName").getAttribute('contact-Id') === msg.connectionId;

        if (isCurrentConnection) {
            const messageElement = createMessageElement(msg);
            chatMessages.appendChild(messageElement);
            chatMessages.scrollTop = chatMessages.scrollHeight;
        } else {
            const contactElement = document.getElementById(`contact_${msg.connectionId}`);
            contactElement.innerHTML = parseInt(contactElement.innerHTML || "0", 10) + 1;
        }
        var updateLastMessage = document.getElementById(`lastMessage_${msg.connectionId}`)
        console.log("here");
        if (msg.attachmentType == "text") {
            updateLastMessage.innerHTML = msg.text;
        } else {
            updateLastMessage.innerHTML = msg.attachmentType.split("s")[0];
        }
    };

    const createMessageElement = (msg) => {
        const messageElement = document.createElement("div");
        if (msg.attachmentType === "Audios") {
            messageElement.innerHTML = `<div class="${msg.senderId === userId ? 'sender audio-wrapper ms-auto' : 'audio-wrapper receiver'}">
                                            <audio class="audio-player" controls src="${msg.attachmentUrl}"></audio>
                                         </div>`;
        } else if (msg.attachmentType === "Photos") {
            messageElement.innerHTML = `<div class="${msg.senderId === userId ? 'message sent ms-auto w-25' : 'message receiver'}">
                                            <img src="${msg.attachmentUrl}" alt="Photo" class="photo ms-auto" width="150" height="150"/>
                                            <p>${msg.text}</p>

                                         </div>`;
        } else if (msg.attachmentType === "PDFs") {
            var attachmentName = msg.attachmentUrl.split("PDFs/")[1].split(".pdf")[0];
            messageElement.innerHTML = `
                    <div class="${msg.senderId === userId  ? 'sent ms-auto pdf-message' : 'receiver pdf-message'}">
                        <div class="pdf-info">
                            <div class="w-100">
                                <embed src="${msg.attachmentUrl}#page=1" type="application/pdf" class="pdf-preview" />
                            </div>
                            <div class="pdf-details d-inline">
                                <p class="pdf-filename">${attachmentName}</p>
                                <p class="pdf-filesize">500 KB</p>
                            </div>
                        </div>
                        <div class="pdf-actions">
                            <a href="${msg.attachmentUrl}" class="btn btn-outline-secondary" target="_blank"><i class="fa-solid fa-eye"></i></a>
                            <a href="${msg.attachmentUrl}" class="btn btn-outline-secondary" download><i class="fa-solid fa-download"></i></a>
                        </div>
                    </div>`;
        } else if (msg.attachmentType === "Videos") {
            messageElement.innerHTML = `<div class="${msg.senderId === userId ? "message sent ms-auto w-50" : "message receiver"}">
                                            <video width="300" height="300" controls src="${msg.attachmentUrl}" class="video-player"></video>
                                            <p>${msg.text}</p>
                                         </div>`;
        } else {
            messageElement.innerHTML = `<div class="${msg.senderId === userId ? 'message sent' : 'message received'} message">${msg.text}</div>`;

        }
        return messageElement;
    };

    proxyConnection.on("ReceiveMessage", updateChatMessages);
    //=========================================================================================================================start voice and videocalls
    let localStream;
    let peerConnection;
    const callButton = document.getElementById("callButton");
    const audioElement = document.getElementById("audio");

    callButton.addEventListener('click', async () => {
        try {
            // Get the local audio stream
            localStream = await navigator.mediaDevices.getUserMedia({ audio: true });
            audioElement.srcObject = localStream;

            // Initialize RTCPeerConnection
            peerConnection = new RTCPeerConnection();

            // Add the local audio track to the connection
            const audioTrack = localStream.getTracks()[0];
            peerConnection.addTrack(audioTrack, localStream);

            // Handle remote track event
            peerConnection.ontrack = (event) => {
                audioElement.srcObject = event.streams[0];
            };

            // ICE Candidate Handling
            peerConnection.onicecandidate = (event) => {
                if (event.candidate) {
                    proxyConnection.invoke("SendIceCandidate", connection, JSON.stringify(event.candidate));
                }
            };

            // Create and send offer
            const offer = await peerConnection.createOffer();
            await peerConnection.setLocalDescription(offer);
            proxyConnection.invoke("SendOffer", connection, JSON.stringify(offer));
        } catch (error) {
            console.error("Error during call setup:", error);
        }
    });

    // Receive Offer
    proxyConnection.on("ReceiveOffer", async (offer) => {
        const answerCall = confirm("You have an incoming call. Do you want to answer?");

        if (answerCall) {
            try {
                localStream = await navigator.mediaDevices.getUserMedia({ audio: true });
                audioElement.srcObject = localStream;

                peerConnection = new RTCPeerConnection();
                peerConnection.addTrack(localStream.getTracks()[0], localStream);

                peerConnection.ontrack = (event) => {
                    console.log("Remote track received:", event.streams[0]);
                    audioElement.srcObject = event.streams[0];
                };

                peerConnection.onicecandidate = (event) => {
                    if (event.candidate) {
                        console.log("Sending ICE candidate:", event.candidate);
                        proxyConnection.invoke("SendIceCandidate", connection, JSON.stringify(event.candidate));
                    }
                };

                const parsedOffer = JSON.parse(offer);
                await peerConnection.setRemoteDescription(new RTCSessionDescription(parsedOffer));

                const answer = await peerConnection.createAnswer();
                await peerConnection.setLocalDescription(answer);
                console.log("Answer created:", answer);

                proxyConnection.invoke("SendAnswer", connection, JSON.stringify(answer));
            } catch (error) {
                console.error("Error receiving offer:", error);
            }
        } else {
            console.log("Call declined.");
            proxyConnection.invoke("CallDeclined", connection);
        }
    });

    // Receive Answer
    proxyConnection.on("ReceiveAnswer", async (answer) => {
        try {
            const parsedAnswer = JSON.parse(answer);
            await peerConnection.setRemoteDescription(new RTCSessionDescription(parsedAnswer));
            console.log("Answer received and set:", parsedAnswer);
        } catch (error) {
            console.error("Error setting answer:", error);
        }
    });

    // Receive ICE Candidate
    proxyConnection.on("ReceiveIceCandidate", (candidate) => {
        console.log("candidate");
        try {
            const parsedCandidate = JSON.parse(candidate);
            peerConnection.addIceCandidate(new RTCIceCandidate(parsedCandidate))
                .catch(error => console.error("Error adding ICE candidate:", error));
        } catch (error) {
            console.error("Error processing ICE candidate:", error);
        }
    });

    initializeConnection();
});