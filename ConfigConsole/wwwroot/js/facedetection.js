﻿async function findFaces(src, dst, facesDetected) {
    const video = document.getElementById(src);
    const result = document.getElementById(facesDetected);

    const model = faceDetection.SupportedModels.MediaPipeFaceDetector;
    const detectorConfig = { runtime: 'tfjs', maxFaces: 10, modelType: 'short' };
    const detector = await faceDetection.createDetector(model, detectorConfig);

    const estimationConfig = { flipHorizontal: false };
    try {
        const faces = await detector.estimateFaces(video, estimationConfig);
        await markFacesOnImage(src, dst, faces);
        result.innerHTML = 'Faces Detected: ' + faces.length;
    } catch (err) {
        console.log(err.message);
    };

    detector.dispose();

}

async function markFacesOnImage(src, dst, faces) {
    video = document.getElementById(src);
    canvas = document.getElementById(dst);

    // update the size based on the video dimensions
    //video.width = video.videoWidth;
    //video.height = video.videoHeight;
    //canvas.height = video.videoHeight;
    //canvas.width = video.videoWidth;

    video.width = window.realVideoWidth;
    video.height = window.realVideoHeight;
    canvas.width = window.realVideoWidth;
    canvas.height = window.realVideoHeight;


    context = canvas.getContext('2d');

    // -- updates needed for front-facing camera
    // -----------------------------------------
    //context.translate(video.videoWidth, 0);
    //context.scale(-1, 1);
    //context.drawImage(video, 0, 0, canvas.width, canvas.height);

    context.strokeStyle = 'red'
    context.lineWidth = 3;

    faces.forEach(face => {
        context.strokeRect(
            face.box.xMin,
            face.box.yMin,
            face.box.xMax - face.box.xMin,
            face.box.yMax - face.box.yMin);
    });
}

async function startDetection(src, dst, facesDetected) {
    return setInterval(() => findFaces(src, dst, facesDetected), 150);
}

async function stopDetection(intervalId) {
    clearInterval(intervalId);
}