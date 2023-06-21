async function downloadFileFromStream(fileName, contentStreamReference) {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}

async function startVideo(src) {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: { facingMode: 'environment' } }).then((stream) => {
            let video = document.getElementById(src);
            video.srcObject = stream;
            //video.onloadedmetadata = (_) => video.onplay;
            video.onloadeddata = (_) => video.onplay;
            //video.style.transform = "scaleX(-1)"; // only needed for front-facing
            video.onplaying = function (e) {
                window.realVideoWidth = video.videoWidth;
                window.realVideoHeight = video.videoHeight;
            }
        }).catch(console.error);
    }
}